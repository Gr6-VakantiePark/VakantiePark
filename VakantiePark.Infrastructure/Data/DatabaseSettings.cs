using System.ComponentModel.DataAnnotations;

namespace VakantiePark.Infrastructure.Data
{
    public class DatabaseSettings
    {
        [Required(ErrorMessage = "DataSource ontbreekt in de configuratie.")]
        [MinLength(1, ErrorMessage = "DataSource ontbreekt in de configuratie.")]
        public string DataSource { get; set; } = "app.db";

        public string? DataFolder { get; set; }

        public string CreateConnectionString(string root)
        {
            if (string.IsNullOrWhiteSpace(root))
                throw new ArgumentException("De hoofdmap van de applicatie ontbreekt.", nameof(root));

            string folder = string.IsNullOrWhiteSpace(DataFolder)
                ? Path.GetFullPath(Path.Combine(root, "..", "VakantiePark.Infrastructure", "Data"))
                : Path.GetFullPath(DataFolder);

            Directory.CreateDirectory(folder);

            return $"Data Source={Path.Combine(folder, DataSource)}";
        }
    }
}
