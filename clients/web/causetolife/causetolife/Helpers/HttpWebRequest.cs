using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;


namespace causetolife.Helpers
{
    public class HttpWebRequestHelper
    {
        public static HttpResponse GetResponseFromServer(WebRequest request)
        {
            using (var response = (WebResponse)request.GetResponse())
            {
                var httpResponse = new HttpResponse(response);
                using (var dataStream = response.GetResponseStream())
                {
                    if (dataStream != null)
                    {
                        using (var reader = new StreamReader(dataStream))
                        {
                            var data = reader.ReadToEnd();
                            httpResponse.Data = data;
                        }
                    }
                }
                return httpResponse;
            }
        }

        public static void WriteStringToRequestStream(WebRequest request, string data)
        {
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
            }
        }

        public static IDictionary<string, string> GetHeaders(WebHeaderCollection webHeaders)
        {
            var headers = new Dictionary<string, string>();
            foreach (string key in webHeaders.AllKeys)
            {
                headers.Add(key, webHeaders[key]);
            }
            return headers;
        }
    }


    public class HttpWebRequestManager
    {
        private bool _canPublish;

        public HttpWebRequestManager(bool publish = true)
        {
            _canPublish = publish;
        }

        private WebRequest Compose(string uri, RequestSettings settings, IDictionary<HttpRequestHeader, string> headers, string postData)
        {
            var request = Compose(uri, settings, headers, null, postData);
            HttpWebRequestHelper.WriteStringToRequestStream(request, postData);
            return request;
        }

        private WebRequest Compose(string uri, RequestSettings settings, IDictionary<HttpRequestHeader, string> headers, IDictionary<string, string> customHeaders, string postData)
        {
            //var url = new Uri(uri);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            if (settings != null)
            {
                request.Method = settings.Method.ToString().ToUpper();
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    switch (header.Key)
                    {
                        case HttpRequestHeader.UserAgent:
                            request.UserAgent = headers[HttpRequestHeader.UserAgent];
                            break;
                        case HttpRequestHeader.ContentType:
                            request.ContentType = headers[HttpRequestHeader.ContentType];
                            break;
                        case HttpRequestHeader.Authorization:
                            request.Headers.Add(HttpRequestHeader.Authorization, headers[HttpRequestHeader.Authorization]);
                            break;
                        case HttpRequestHeader.Accept:
                            request.Accept = headers[HttpRequestHeader.Accept];
                            break;
                    }
                }
            }

            if (customHeaders != null)
            {
                foreach (var header in customHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(postData))
            {
                HttpWebRequestHelper.WriteStringToRequestStream(request, postData);
            }

            return request;
        }


        private HttpResponse GetResponse(WebRequest request)
        {
            return HttpWebRequestHelper.GetResponseFromServer(request);
        }
        
        public string GetResponse(string uri, RequestSettings settings, IDictionary<HttpRequestHeader, string> headers, IDictionary<string, string> customHeaders, string postData)
        {
            Exception error = null;
            HttpResponse response = null;

            var request = Compose(uri, settings, headers, customHeaders, postData);
            var httpRequest = new HttpRequest((HttpWebRequest)request) { Data = postData };

            try
            {
                response = GetResponse(request);
            }
            catch (Exception exception)
            {
                error = exception;
            }

            if (error != null)
                throw error;
            else
                return response.Data;
        }
    }

    public class HttpRequest
    {
        public HttpRequest(HttpWebRequest request)
        {
            Request = request;
            Headers = request.Headers;
            Timestamp = DateTime.UtcNow;
            Server = request.Host;
        }
        public WebRequest Request { get; private set; }
        public string Data { get; set; }
        public WebHeaderCollection Headers { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Server { get; private set; }
    }

    public enum HttpMethod
    {
        Connect,
        Get,
        Head,
        Post,
        Put,
        Delete
    }

    public class HttpResponse
    {
        public HttpResponse(WebResponse response)
        {
            Headers = response.Headers;
        }
        public string Data { get; set; }
        public WebHeaderCollection Headers { get; private set; }
    }

    public class RequestSettings
    {
        public HttpMethod Method { get; set; }
    }

    public class ServiceHelper
    {
        public static Dictionary<HttpRequestHeader, string> AddHeaders(string mediaType = null)
        {
            return new Dictionary<HttpRequestHeader, string>
                       {
                           {HttpRequestHeader.ContentType, mediaType ?? "text/plain"},
                           {HttpRequestHeader.UserAgent, "Browser"}
                       };
        }
    }

    public static class JsonConvert
    {
        public static string Serialize<T>(T obj, bool dictionary = false) where T : class
        {
            //throw new Exception("Test error in serialize");
            DataContractJsonSerializer serializer;
            if (dictionary)
            {
                var settings = new DataContractJsonSerializerSettings();
                settings.UseSimpleDictionaryFormat = true;
                serializer = new DataContractJsonSerializer(obj.GetType(), settings);
            }
            else
            {
                serializer = new DataContractJsonSerializer(obj.GetType());
            }
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                var retVal = Encoding.Default.GetString(ms.ToArray());
                return retVal;
            }
        }

        public static string Serialize<T>(T obj, string rootName, Type[] types, bool dictionary = false) where T : class
        {
            //throw new Exception("Test error in serialize");
            DataContractJsonSerializer serializer;
            if (dictionary)
            {
                var settings = new DataContractJsonSerializerSettings();
                settings.UseSimpleDictionaryFormat = true;
                settings.RootName = rootName;
                settings.MaxItemsInObjectGraph = 1000;
                settings.KnownTypes = types;
                settings.EmitTypeInformation = System.Runtime.Serialization.EmitTypeInformation.AsNeeded;
                serializer = new DataContractJsonSerializer(obj.GetType(), settings);
            }
            else
            {
                serializer = new DataContractJsonSerializer(obj.GetType());
            }
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                var retVal = Encoding.Default.GetString(ms.ToArray());
                return retVal;
            }
        }

        public static T Deserialize<T>(string json, bool dictionary = false) where T : class
        {
            //throw new Exception("Test error in de-serialize");
            var obj = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer serializer;
                if (dictionary)
                {
                    var settings = new DataContractJsonSerializerSettings();
                    settings.UseSimpleDictionaryFormat = true;
                    serializer = new DataContractJsonSerializer(obj.GetType(), settings);
                }
                else
                {
                    serializer = new DataContractJsonSerializer(obj.GetType());
                }
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
                return obj;
            }
        }

        public static T Deserialize<T>(string json, string rootName, Type[] types, bool dictionary = false) where T : class
        {
            //throw new Exception("Test error in de-serialize");
            var obj = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer serializer;
                if (dictionary)
                {
                    var settings = new DataContractJsonSerializerSettings();
                    settings.UseSimpleDictionaryFormat = true;
                    settings.RootName = rootName;
                    settings.MaxItemsInObjectGraph = 1000;
                    settings.KnownTypes = types;
                    settings.EmitTypeInformation = System.Runtime.Serialization.EmitTypeInformation.AsNeeded;
                    serializer = new DataContractJsonSerializer(obj.GetType(), settings);
                }
                else
                {
                    serializer = new DataContractJsonSerializer(obj.GetType());
                }
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
                return obj;
            }
        }
    }
}