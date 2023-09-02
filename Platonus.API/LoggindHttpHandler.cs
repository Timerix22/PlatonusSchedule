using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Platonus.API;

public class LoggindHttpHandler : DelegatingHandler
{
    public LoggindHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    { }

    void LogHttpRequest(HttpRequestMessage req)
    {
        Console.WriteLine($"REQUEST {req.Method} to {req.RequestUri}: " +
                          req.Content?.ReadAsStringAsync().GetAwaiter().GetResult());
    }

    void LogHttpResponse(HttpResponseMessage res)
    {
        Console.WriteLine($"RESPONSE {res.StatusCode} ({(int)res.StatusCode}) from {res.RequestMessage?.RequestUri}: " +
                          res.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        if (res.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
            foreach (var cookieHeader in cookieHeaders)
            {
                Console.WriteLine($"    Set-Cookie: {cookieHeader}");
            }
    }

    #if !NETSTANDARD2_0
    protected override HttpResponseMessage Send(HttpRequestMessage req, CancellationToken cancellationToken)
    {
        LogHttpRequest(req);
        var res = base.Send(req, cancellationToken);
        LogHttpResponse(res);
        return res;
    }
    #endif

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken cancellationToken)
    {
        LogHttpRequest(req);
        var res = await base.SendAsync(req, cancellationToken);
        LogHttpResponse(res);
        return res;
    }
}