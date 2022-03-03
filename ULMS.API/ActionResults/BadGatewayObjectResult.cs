using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ULMS.ActionResults
{
    public class BadGatewayObjectResult : ObjectResult
    {
        public BadGatewayObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status502BadGateway;
        }
    }
}
