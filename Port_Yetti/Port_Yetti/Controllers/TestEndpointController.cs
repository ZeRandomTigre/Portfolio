using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Port_Yetti.Controllers
{
    public class TestEndpointController : Controller
    {
        // POST api/test/post1
        [HttpPost("post1", Name = "test/Post1")]
        public IActionResult Index1(object blahblah)
        {
            return new OkResult();
        }

        // POST api/test/post2
        [HttpPost("post2", Name = "test/Post2")]
        public IActionResult Index2(object blahblah)
        {
            return new OkResult();
        }
    }
}