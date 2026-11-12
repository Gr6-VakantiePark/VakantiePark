using VakantiePark.Domain.Entities;

namespace VakantiePark.Domain.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) CreateToken(Employee employee);
    }
}
