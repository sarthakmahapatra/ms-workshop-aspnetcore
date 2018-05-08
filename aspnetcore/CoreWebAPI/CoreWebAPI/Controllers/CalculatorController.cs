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
        [HttpGet("add/{value1}/{value2}")]
        public int Add(int value1, int value2)
        {
            return value1 + value2;
        }

        [HttpGet("substract/{value1}/{value2}")]
        public int Substract(int value1, int value2)
        {
            return value1 - value2;
        }

        [HttpGet("multiply/{value1}/{value2}")]
        public int Multiply(int value1, int value2)
        {
            return value1 * value2;
        }

        [HttpGet("divide/{value1}/{value2}")]
        public int Divide(int value1, int value2)
        {
            return value1 / value2;
        }

        [HttpGet]
        public string Get()
        {
            return "default";
        }
    }
}
