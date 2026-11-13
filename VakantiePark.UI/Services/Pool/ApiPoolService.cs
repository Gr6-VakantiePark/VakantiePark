using System.Globalization;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Pool;
using VakantiePark.UI.Interfaces;

namespace VakantiePark.UI.Services
{
    public class ApiPoolService(IHttpService http) : IPoolService
    {
        public Task<ApiResponse<PoolDayDto>> GetTimeSlotsAsync(DateOnly date) =>
            http.GetAsync<PoolDayDto>($"pool/time-slots?date={date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
    }
}
