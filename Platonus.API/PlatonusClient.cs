using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Platonus.API.DataModels;

namespace Platonus.API;

public class PlatonusClient
{
    private const string base_url = "https://platonus.iitu.edu.kz/";
    public bool DebugHttpMessages;


    private LoginResponse? _loginResponse;
    private PlatonusLanguage? _language;
    private HttpClient _http;
    private CookieContainer _cookies;

    public PlatonusClient()
    {
#if DEBUG
        // TODO disable http messages logging
        DebugHttpMessages = true;
#endif
        _cookies = new CookieContainer();
        var httpClientHandler = new HttpClientHandler
        {
            CookieContainer = _cookies
        };
        _http = new HttpClient(new LoggindHttpHandler(httpClientHandler))
        {
            BaseAddress = new Uri(base_url)
        };
    }

    public async Task LoginAsync(LoginCredentials userCredentials)
    {
        string json = $$"""
            {
              "login": "{{userCredentials.Login}}",
              "password": "{{userCredentials.Password}}",
              "authForDeductedStudentsAndGraduates": "false"
            }
            """;
        var reqContent = new StringContent(json);
        reqContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        _language = userCredentials.Language;
        reqContent.Headers.Add("language", _language.Number.ToString());
        

        try
        {
            // send POST request
            var res = await _http.PostAsync("rest/api/login", reqContent);
            // parse json
            if (res.Content.Headers.ContentLength < 1)
                throw new Exception("responce doesn't have any content");
            _loginResponse = await res.Content.ReadFromJsonAsync<LoginResponse>()
                             ?? throw new Exception("invalid login request rezult");
            if(_loginResponse.login_status != "success")
                throw new Exception("invalid login request rezult");
            
            // set session id and user token headers in HttpClient
            _http.DefaultRequestHeaders.Add("sid", _loginResponse.sid);
            _http.DefaultRequestHeaders.Add("token", _loginResponse.auth_token);
        }
        catch (HttpRequestException re)
        {
            HandleRequestException(re);
        }
    }

    public async Task GetScheduleAsync()
    {
        if (_language is null)
            throw new Exception("language is null, you need to login");
        if (_loginResponse is null)
            throw new Exception("session id is null, you need to login");

        try
        {
            var req = JsonContent.Create<ScheduleRequest>(new ScheduleRequest());
            var res = await _http.PostAsync(
                $"rest/schedule/userSchedule/student/initial/{_language.LiteralCode}",
                req);
            // var rezult = await res.Content.ReadFromJsonAsync<ScheduleResponseData>();
        }
        catch (HttpRequestException re)
        {
            HandleRequestException(re);
        }

    }

    private void HandleRequestException(HttpRequestException re)
    {
        // TODO http exception handling
        throw re;
    }
}