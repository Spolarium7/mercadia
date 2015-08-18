using Mercadia.Web.Securities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mercadia.Web.Controllers
{
    [InPrivate]
    public class BaseController : Controller
    {
        protected string ErrorMessage = "";
        private RestClient client = new RestClient();
        private string ApiPrefix = ConfigurationManager.AppSettings["ApiPrefix"];

        #region Get
        protected Dto<T> Get<T>(string endpoint)
        {
            RestRequest request = new RestRequest(endpoint, Method.GET);
            return Ping<T>(request);
        }

        protected Dto<T> Get<T>(string endpoint, params object[] args)
        {
            return Get<T>(endpoint: String.Format(endpoint, args));
        }


        #endregion

        #region Post

        public Dto<T> Post<T>(string endpoint, object objectParams)
        {
            /* request */
            var request = new RestRequest(endpoint, Method.POST);
            request.AddJsonBody(objectParams);

            return Ping<T>(request);
        }
        #endregion

        #region Put

        public Dto<T> Put<T>(string endpoint, object objectParams)
        {
            /* request */
            var request = new RestRequest(endpoint, Method.PUT);
            request.AddJsonBody(objectParams);

            return Ping<T>(request);
        }
        #endregion

        #region Delete

        public Dto<T> Delete<T>(string endpoint, params object[] args)
        {

            /* request */
            RestRequest request = new RestRequest(String.Format(endpoint, args), Method.DELETE);
            return Ping<T>(request);

        }
        #endregion
        private Dto<T> Ping<T>(RestRequest request)
        {

            Dto<T> dto = new Dto<T>();

            try
            {
                client = new RestClient(ApiPrefix);
                IRestResponse response = client.Execute(request);
                dto.Status = response.StatusCode;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    dto.Data = JsonConvert.DeserializeObject<T>(response.Content);
                }
                /* validation error */
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    dto.Message = JsonConvert.DeserializeObject<string>(response.Content);
                    this.ErrorMessage = JsonConvert.DeserializeObject<string>(response.Content);
                }
                /* application authentication error */
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    dto.Message = JsonConvert.DeserializeObject<string>(response.Content);
                    this.ErrorMessage = JsonConvert.DeserializeObject<string>(response.Content);
                }
                else
                {
                    // LOG ERROR
                }
            }
            catch (WebException ex)
            {
                // LOG ERROR
            }

            return dto;
        }
    }

    public class Dto<T>
    {
        public Dto()
        {
            this.Status = HttpStatusCode.OK;
        }

        public T Data { get; set; }

        public string Message { get; set; }

        public HttpStatusCode Status { get; set; }
    }
}