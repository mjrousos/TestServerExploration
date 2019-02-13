using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static int CallCount = 0;

        private IConfiguration Configuration { get; }

        public ValuesController(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            Interlocked.Increment(ref CallCount);
            return Configuration["ReturnValue"];
        }

        [HttpGet("CallCount")]
        public ActionResult<int> GetCallCount() => CallCount;
    }
}
