using System.Net.Http.Json;
using System.Text.Json;
namespace RaceDay.Mvc.Services;
public class RaceDayApiClient {
    private readonly IHttpClientFactory _factory;
    private readonly IHttpContextAccessor _http;
    public RaceDayApiClient(IHttpClientFactory factory, IHttpContextAccessor http) { _factory = factory; _http = http; }

    public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, object? body = null) {
        var client = _factory.CreateClient("RaceDayApi");
        using var request = new HttpRequestMessage(method, url);
        string? cookie = _http.HttpContext?.Session.GetString("ApiSessionCookie");
        if (cookie != null) request.Headers.TryAddWithoutValidation("Cookie", cookie);
        if (body != null) request.Content = JsonContent.Create(body);
        return await client.SendAsync(request);
    }
    public async Task<HttpResponseMessage> UploadFileAsync(string url, IFormFile file) {
        var client = _factory.CreateClient("RaceDayApi");
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        string? cookie = _http.HttpContext?.Session.GetString("ApiSessionCookie");
        if (cookie != null) request.Headers.TryAddWithoutValidation("Cookie", cookie);
        using var form = new MultipartFormDataContent();
        using var stream = file.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        form.Add(fileContent, "file", file.FileName);
        request.Content = form;
        return await client.SendAsync(request);
    }
    public async Task<T?> ReadAsync<T>(HttpResponseMessage response) {
        if (!response.IsSuccessStatusCode) return default;
        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    public static string? ExtractSessionCookie(HttpResponseMessage res) {
        if (!res.Headers.TryGetValues("Set-Cookie", out var vals)) return null;
        return vals.FirstOrDefault(v => v.StartsWith(".AspNetCore.Session="))?.Split(';', 2)[0];
    }
}
