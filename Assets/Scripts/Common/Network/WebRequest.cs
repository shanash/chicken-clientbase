using System.Collections.Generic;
using UnityEngine;


namespace Common
{
    namespace Network
    {
        public class WebRequest
        {
            private const string kContentType = "application/json; charset=UTF-8";

            public enum Method
            {
                GET = 0,
                POST,
                PUT,
                DELETE
            }

            public Dictionary<string, string> headers { get; private set; }
            public Method method = Method.GET;
            public string uri = string.Empty;
            public string body = string.Empty;

            public WebRequest(string uri)
            {
                this.SetHeader("Content-Type", kContentType);
                this.method = Method.GET;
                this.uri = uri;
                this.body = string.Empty;
            }

            public WebRequest(Method method, string uri, string body = null)
            {
                this.SetHeader("Content-Type", kContentType);
                this.method = method;
                this.uri = uri;
                this.body = body;
            }

            public WebRequest(Method method, string uri, JsonObject json)
            {
                this.SetHeader("Content-Type", kContentType);
                this.method = method;
                this.uri = uri;
                this.body = (json != null) ? json.ToJson() : string.Empty;
            }

            public void SetHeader(string name, string value)
            {
                if (this.headers == null) this.headers = new Dictionary<string, string>();
                if (this.headers.ContainsKey(name)) this.headers[name] = value;
                else this.headers.Add(name, value);
            }

            public static WebRequest GET    (string uri)                    { return new WebRequest(Method.GET, uri); }
            public static WebRequest POST   (string uri, string body)       { return new WebRequest(Method.POST, uri, body); }
            public static WebRequest POST   (string uri, JsonObject json)   { return new WebRequest(Method.POST, uri, json); }
            public static WebRequest PUT    (string uri, string body)       { return new WebRequest(Method.PUT, uri, body); }
            public static WebRequest PUT    (string uri, JsonObject json)   { return new WebRequest(Method.PUT, uri, json); }
            public static WebRequest DELETE (string uri, string body)       { return new WebRequest(Method.DELETE, uri, body); }
            public static WebRequest DELETE (string uri, JsonObject json)   { return new WebRequest(Method.DELETE, uri, json); }
        }
    }
}