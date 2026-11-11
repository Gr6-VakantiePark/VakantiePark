using VakantiePark.Shared.Common;

namespace VakantiePark.UI.Interfaces
{
	public interface IHttpService
	{
		Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url);
		Task<ApiResponse<TResponse>> PostAsync<TResponse, TRequest>(string url, TRequest request);
		Task<ApiResponse<TResponse>> PutAsync<TResponse, TRequest>(string url, TRequest request);
		Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url);
		Task<ApiResponse<byte[]>> GetFileAsync(string url);
	}
}
