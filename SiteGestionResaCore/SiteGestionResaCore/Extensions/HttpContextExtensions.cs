using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Extensions
{
    public static class HttpContextExtensions
    {
        public static void AddToSession(this HttpContext context, string key, object data) 
            => context.Session.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));

        public static T GetFromSession<T>(this HttpContext context, string key)
        {
            if(context.Session.TryGetValue(key, out byte[] byteData))
            {
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(byteData));
            }
            return default;
        }

        public static string GetUserId(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
