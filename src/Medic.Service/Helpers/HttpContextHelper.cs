using Microsoft.AspNetCore.Http;

namespace Medic.Service.Helpers;

public static class HttpContextHelper
{
    public static IHttpContextAccessor Accessor { get; set; }
    public static HttpContext HttpContext => Accessor?.HttpContext;
    public static IHeaderDictionary ResponseHeaders => HttpContext?.Response?.Headers;
    public static long? GetUserId => long.TryParse(HttpContext?.User?.FindFirst("Id")?.Value, out _tempUserId) ? _tempUserId : null;
    public static string UserRole => HttpContext?.User?.FindFirst("Role")?.Value;

    private static long _tempUserId;
}