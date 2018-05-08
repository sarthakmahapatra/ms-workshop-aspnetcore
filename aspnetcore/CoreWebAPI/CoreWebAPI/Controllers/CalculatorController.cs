using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CalculatorController : Controller
    {
        [HttpPost("add")]
        public int Add([FromBody]Value value)
        {
            return value.Value1 + value.Value2;
        }

        [HttpPost("substract")]
        public int Substract([FromBody]Value value)
        {
            return value.Value1 - value.Value2;
        }

        [HttpPost("multiply")]
        public int Multiply([FromBody]Value value)
        {
            return value.Value1 * value.Value2;
        }

        [HttpPost("divide")]
        public int Divide([FromBody]Value value)
        {
            return value.Value1 / value.Value2;
        }

        [HttpGet]
        public string Get()
        {
            return "default";
        }
    }

    public class Value
    {
        public int Value1
        {
            get;
            set;

        }  
            public int Value2
        {
            get;
            set;
        }
    }
}
