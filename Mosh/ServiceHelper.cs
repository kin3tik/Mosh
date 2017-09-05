using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace Mosh
{
    public class ServiceHelper
    {
        private static string UserAgent
        {
            get
            {
                var version = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location ?? "").FileVersion;
                return $"Mosh/{version}";
            }
        }

        public static async Task<WebServiceResult<T>> GetAsync<T>(WebServiceParameters<object> parameters) where T : new()
        {
            try
            {
                if (string.IsNullOrEmpty(parameters.EndpointPath))
                    throw new Exception("ServicePath or EndpointPath not set.");

                var client = new RestClient(parameters.Uri) { UserAgent = UserAgent, Timeout = parameters.Timeout };
                var request = new RestRequest(parameters.EndpointPath, parameters.HttpMethod);

                if (!string.IsNullOrEmpty(parameters.RootElement))
                    request.RootElement = parameters.RootElement;

                foreach (var segment in parameters.UrlSegments)
                    request.AddUrlSegment(segment.Key, segment.Value);

                var taskCompletion = new TaskCompletionSource<IRestResponse>();
                client.ExecuteAsync(request, r => taskCompletion.SetResult(r));
                var response = (RestResponse) await taskCompletion.Task;

                var data = JsonConvert.DeserializeObject<T>(response.Content);

                return new WebServiceResult<T>
                {
                    StatusCode = response.StatusCode,
                    Message = response.ErrorMessage,
                    ResultObject = data,
                    RawContent = response.Content
                };
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error during GetAsync");
                return new WebServiceResult<T> { Message = e.Message };
            }
        }
    }

    public class WebServiceParameters<T>
    {
        public string Uri { get; set; }
        public Method HttpMethod { get; set; }
        public string EndpointPath { get; set; }
        public Dictionary<string, string> UrlSegments { get; set; }
        public T SendObject { get; set; }
        public T RawSendObject { get; set; } //this will override SendObject
        public string RawSendObjectMimeType { get; set; }
        public bool UseDotNetSerialisation { get; set; }
        public int Timeout { get; set; } //miliseconds
        public string RootElement { get; set; }

        public WebServiceParameters()
        {
            //Uri = WebServiceHelper.Uri;
            HttpMethod = Method.POST;
            UrlSegments = new Dictionary<string, string>();
            Timeout = 0; //should default to 100 seconds from HttpWebRequest
            RawSendObjectMimeType = "application/zip";
        }
    }

    public class WebServiceResult<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T ResultObject { get; set; }
        public bool IsSuccessful { get; set; }
        public string RawContent { get; set; }

        public WebServiceResult()
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
