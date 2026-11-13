using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using VakantiePark.API;
using VakantiePark.Shared.Common;

namespace VakantiePark.UnitTests.Security
{
    public partial class AuthorizationRulesTests
    {
        private static readonly HashSet<string> KnownPolicies = [.. Permissions.All, Permissions.Owner];

        private static readonly HashSet<string> LoginOnlyActions = ["AuthController.Me"];

        private static readonly HashSet<string> LoginOnlyPages = ["/beheer", "/beheer/inloggen"];

        [Fact]
        public void EveryEndpoint_IsPublicOrRequiresARight()
        {
            var problems = new List<string>();

            var controllers = typeof(ApiResults).Assembly.GetTypes()
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var controller in controllers)
            {
                string route = controller.GetCustomAttribute<RouteAttribute>()?.Template ?? string.Empty;
                var actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => m.GetCustomAttributes<HttpMethodAttribute>().Any());

                foreach (var action in actions)
                {
                    string name = $"{controller.Name}.{action.Name}";
                    bool anonymous = controller.GetCustomAttribute<AllowAnonymousAttribute>() is not null
                        || action.GetCustomAttribute<AllowAnonymousAttribute>() is not null;

                    if (anonymous)
                    {
                        if (route.StartsWith("admin", StringComparison.OrdinalIgnoreCase))
                            problems.Add($"{name} is public, but admin endpoints must require a right.");
                        continue;
                    }

                    var policies = controller.GetCustomAttributes<AuthorizeAttribute>()
                        .Concat(action.GetCustomAttributes<AuthorizeAttribute>())
                        .Select(a => a.Policy)
                        .OfType<string>()
                        .ToList();

                    if (policies.Count == 0 && !LoginOnlyActions.Contains(name))
                        problems.Add($"{name} needs [Authorize(Policy = Permissions.X)] or [AllowAnonymous].");

                    problems.AddRange(policies.Where(p => !KnownPolicies.Contains(p))
                        .Select(p => $"{name} uses unknown policy '{p}'."));
                }
            }

            Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems));
        }

        [Fact]
        public void EveryStaffPage_RequiresARight()
        {
            var problems = new List<string>();
            string pages = Path.Combine(FindRepositoryRoot(), "VakantiePark.UI", "Pages");

            foreach (string file in Directory.EnumerateFiles(pages, "*.razor", SearchOption.AllDirectories))
            {
                string text = File.ReadAllText(file);
                var route = PageRoute().Match(text);
                if (!route.Success
                    || !route.Groups[1].Value.StartsWith("/beheer", StringComparison.OrdinalIgnoreCase)
                    || LoginOnlyPages.Contains(route.Groups[1].Value))
                    continue;

                var policy = PagePolicy().Match(text);
                if (!policy.Success)
                {
                    problems.Add($"{Path.GetFileName(file)} ({route.Groups[1].Value}) needs @attribute [Authorize(Policy = Permissions.X)].");
                    continue;
                }

                string? value = typeof(Permissions).GetField(policy.Groups[1].Value)?.GetValue(null) as string;
                if (value is null || !KnownPolicies.Contains(value))
                    problems.Add($"{Path.GetFileName(file)} uses unknown policy '{policy.Groups[1].Value}'.");
            }

            Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems));
        }

        [Fact]
        public void KnownPolicies_AreTheRightsPlusOwner()
        {
            Assert.Equal(Permissions.All.Count + 1, KnownPolicies.Count);
            Assert.DoesNotContain(Permissions.Owner, Permissions.All);
        }

        private static string FindRepositoryRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory is not null && !directory.EnumerateFiles("*.slnx").Any())
                directory = directory.Parent;

            return directory?.FullName ?? throw new InvalidOperationException("De map van de solution is niet gevonden.");
        }

        [GeneratedRegex("@page\\s+\"([^\"]+)\"")]
        private static partial Regex PageRoute();

        [GeneratedRegex("@attribute\\s+\\[Authorize\\(Policy\\s*=\\s*(?:VakantiePark\\.Shared\\.Common\\.)?Permissions\\.(\\w+)\\)\\]")]
        private static partial Regex PagePolicy();
    }
}
