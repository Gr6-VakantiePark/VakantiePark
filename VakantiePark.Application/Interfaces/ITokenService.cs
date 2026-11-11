using VakantiePark.Domain.Entities;

namespace VakantiePark.Application.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) CreateToken(Employee employee);
    }
}
