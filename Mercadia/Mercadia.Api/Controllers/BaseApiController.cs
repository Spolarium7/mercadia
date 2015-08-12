using Mercadia.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Mercadia.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        protected MercadiaDbContext db = new MercadiaDbContext();
        public object ThrowError(string message)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            responseMessage.Content = new ObjectContent<string>(message, new JsonMediaTypeFormatter());
            throw new HttpResponseException(responseMessage);
        }
    }
}
