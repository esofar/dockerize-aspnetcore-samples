using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HelloController(
            IDistributedCache distributedCache,
            IHttpContextAccessor httpContextAccessor)
        {
            _distributedCache = distributedCache;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var connection = _httpContextAccessor.HttpContext.Connection;
            var ipv4 = connection.LocalIpAddress.MapToIPv4().ToString();
            var message = $"Hello from Docker Container：{ipv4}";

            return message;
        }

        [HttpGet("{name}")]
        public ActionResult<string> Get(string name)
        {
            var defaultKey = $"hello:{name}";
            _distributedCache.SetString(defaultKey, $"Hello {name} form Redis");
            var message = _distributedCache.GetString(defaultKey);

            return message;
        }
    }
}