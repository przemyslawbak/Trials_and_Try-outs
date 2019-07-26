using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HelloWebAPI.Controllers
{
    //https://www.tutorialsteacher.com/webapi/web-api-tutorials
    public class HelloController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }
    }
}
