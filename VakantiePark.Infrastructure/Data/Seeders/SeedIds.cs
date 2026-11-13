using System.Security.Cryptography;
using System.Text;

namespace VakantiePark.Infrastructure.Data.Seeders
{
    public static class SeedIds
    {
        public static readonly Guid ParkSettings = For("park-settings", "default");

        public static Guid AccommodationType(string slug) => For("accommodation-type", slug);
        public static Guid Amenity(string code) => For("amenity", code);
        public static Guid Accommodation(string code) => For("accommodation", code);
        public static Guid Photo(string typeSlug, string fileName) => For("photo", $"{typeSlug}/{fileName}");
        public static Guid Permission(string code) => For("permission", code);
        public static Guid Employee(string email) => For("employee", email);

        public static Guid For(string kind, string key)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(kind);
            ArgumentException.ThrowIfNullOrWhiteSpace(key);

            byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes($"vakantiepark:{kind}:{key.Trim().ToLowerInvariant()}"));
            hash[6] = (byte)((hash[6] & 0x0F) | 0x30);
            hash[8] = (byte)((hash[8] & 0x3F) | 0x80);
            return new Guid(hash, bigEndian: true);
        }
    }
}
