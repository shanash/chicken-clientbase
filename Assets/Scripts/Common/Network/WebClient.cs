using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    namespace Network
    {
        public class WebClient : MonoBehaviour
        {
            public delegate void WebResponse(long statusCode, string body);
            public System.Action transmissionBegin = null;
            public System.Action transmissionEnd = null;

            enum State
            {
                Ready = 0,
                Busy,
            }

            class RequestItem
            {
                public WebRequest request = null;
                public WebResponse response = null;

                public RequestItem(WebRequest req, WebResponse res)
                {
                    this.request = req;
                    this.response = res;
                }
            }

            Queue<RequestItem> _requestQueue = new Queue<RequestItem>();
            State _state = State.Ready;

            public bool IsBusy { get { return _state != State.Ready; } }

            public void Clear()
            {
                if (this.IsBusy)
                {
                    StopCoroutine("ProcessRequest");
                }

                _state = State.Ready;
                _requestQueue.Clear();
            }

            public void Request(WebRequest req, WebResponse res)
            {
                _requestQueue.Enqueue(new RequestItem(req, res));
                this.NextRequest();
            }

            private void NextRequest()
            {
                if (_requestQueue.Count <= 0) return;
                if (this.IsBusy) return;
                RequestItem[] items = _requestQueue.ToArray();
                _requestQueue.Clear();
                StartCoroutine("ProcessRequest", items);
            }

            private IEnumerator ProcessRequest(RequestItem[] items)
            {
                _state = State.Busy;

                foreach (var item in items)
                {
                    using(UnityWebRequest www = new UnityWebRequest())
                    {
                        foreach (KeyValuePair<string, string> header in item.request.headers)
                        {
                            www.SetRequestHeader(header.Key, header.Value);
                        }

                        www.method = this.MethodString(item.request.method);
                        www.url = item.request.uri;
                        www.downloadHandler = new DownloadHandlerBuffer();

                        if (!string.IsNullOrEmpty(item.request.body))
                        {
                            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(item.request.body));
                        }

                        if(transmissionBegin != null) transmissionBegin();
    
                        yield return www.SendWebRequest();

                        if(transmissionEnd != null) transmissionEnd();

                        if (www.isNetworkError || www.isHttpError)
                        {
                            Debug.LogError(www.error);
                            Debug.LogError(www.downloadHandler.text);
                        }

                        item.response(www.responseCode, www.downloadHandler.text);
                    }
                }

                _state = State.Ready;
                this.NextRequest();
            }

            private string MethodString(WebRequest.Method method)
            {
                if (method == WebRequest.Method.POST) return UnityWebRequest.kHttpVerbPOST;
                else if (method == WebRequest.Method.PUT) return UnityWebRequest.kHttpVerbPUT;
                else if (method == WebRequest.Method.DELETE) return UnityWebRequest.kHttpVerbDELETE;
                else return UnityWebRequest.kHttpVerbGET;
            }
        }
    }
}