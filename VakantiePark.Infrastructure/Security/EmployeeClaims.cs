using System.Security.Claims;
using VakantiePark.Domain.Entities;
using VakantiePark.Shared.Common;

namespace VakantiePark.Infrastructure.Security
{
    public static class EmployeeClaims
    {
        public static IEnumerable<Claim> For(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            if (!employee.IsActive)
                yield break;

            if (employee.IsOwner)
                yield return new Claim(Permissions.OwnerClaimType, "true");

            var codes = employee.IsOwner
                ? Permissions.All
                : employee.Permissions.Select(p => p.Code).Distinct();

            foreach (string code in codes)
                yield return new Claim(Permissions.ClaimType, code);
        }

        public static bool IsRightsClaim(Claim claim) =>
            claim.Type == Permissions.ClaimType || claim.Type == Permissions.OwnerClaimType;
    }
}
