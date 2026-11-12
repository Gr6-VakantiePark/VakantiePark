using System.Text.Json;
using System.Net.Http.Json;
using VakantiePark.UI.Interfaces;
using VakantiePark.Shared.Common;

namespace VakantiePark.UI.Services
{
	public class HttpService : IHttpService
	{
		private readonly HttpClient _httpClient;
		private const string Error = "Er ging iets mis bij het verwerken van je verzoek. Probeer het opnieuw.";

		public HttpService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url)
		{
			try
			{
				var response = await _httpClient.GetAsync(url);
				return await ReadAnswer<TResponse>(response);
			}
			catch (Exception)
			{
				return ApiResponse<TResponse>.Fail(Error);
			}
		}

		public async Task<ApiResponse<TResponse>> PostAsync<TResponse, TRequest>(string url, TRequest request)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync(url, request, JsonDefaults.Options);
				return await ReadAnswer<TResponse>(response);
			}
			catch (Exception)
			{
				return ApiResponse<TResponse>.Fail(Error);
			}
		}

		public async Task<ApiResponse<TResponse>> PutAsync<TResponse, TRequest>(string url, TRequest request)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync(url, request, JsonDefaults.Options);
				return await ReadAnswer<TResponse>(response);
			}
			catch (Exception)
			{
				return ApiResponse<TResponse>.Fail(Error);
			}
		}

		public async Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string url)
		{
			try
			{
				var response = await _httpClient.DeleteAsync(url);
				return await ReadAnswer<TResponse>(response);
			}
			catch (Exception)
			{
				return ApiResponse<TResponse>.Fail(Error);
			}
		}

		public async Task<ApiResponse<byte[]>> GetFileAsync(string url)
		{
			try
			{
				var response = await _httpClient.GetAsync(url);
				if (response.IsSuccessStatusCode && response.Content.Headers.ContentType?.MediaType != "application/json")
					return ApiResponse<byte[]>.Ok(await response.Content.ReadAsByteArrayAsync());

				var answer = await ReadAnswer<object>(response);
				return ApiResponse<byte[]>.Fail(answer.Message ?? Error);
			}
			catch (Exception)
			{
				return ApiResponse<byte[]>.Fail(Error);
			}
		}

		private static async Task<ApiResponse<TResponse>> ReadAnswer<TResponse>(HttpResponseMessage response)
		{
			try
			{
				var result = await response.Content.ReadFromJsonAsync<ApiResponse<TResponse>>(JsonDefaults.Options);
				if (result is not null)
					return result;
			}
			catch (Exception exception) when (exception is JsonException or NotSupportedException)
			{
			}

			return response.IsSuccessStatusCode
				? ApiResponse<TResponse>.Ok()
				: ApiResponse<TResponse>.Fail(Error);
		}
	}
}
