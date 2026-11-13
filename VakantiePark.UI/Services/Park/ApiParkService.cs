using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Park;
using VakantiePark.UI.Interfaces;

namespace VakantiePark.UI.Services
{
    public class ApiParkService(IHttpService http) : IParkService
    {
        private ApiResponse<ParkInfoDto>? _cached;

        public async Task<ApiResponse<ParkInfoDto>> GetInfoAsync()
        {
            if (_cached != null)
            {
                return _cached;
            }

            var response = await http.GetAsync<ParkInfoDto>("park");
            if (response.Succeeded && response.Value != null)
            {
                _cached = response;
            }

            return response;
        }
    }
}
