using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    namespace Network
    {
        public class NetworkManager : UnitySingleton<NetworkManager>
        {
            private Uri _baseuri = null;
            private Network.WebClient _client = null;

            protected override void Awake()
            {
                base.Awake();
                _client = this.gameObject.AddComponent<Network.WebClient>();
                _client.transmissionBegin = TransmissionBegin;
                _client.transmissionEnd = TransmissionEnd;
            }

            private string DataPath
            {
                get
                {
        #if UNITY_EDITOR
                    return "file://" + Application.streamingAssetsPath + "/";
        #elif UNITY_ANDROID
                    return Application.streamingAssetsPath + "/";
        #elif UNITY_IOS
                    return Application.dataPath + "/Raw/";
        #endif
                }
            }

            private string Resolve(string filename)
            {
                return this.DataPath + filename;
            }

            public void SetDefaultUri(string uri)
            {
                _baseuri = new Uri(uri);
            }

            public void GetLocalData(string api, WebClient.WebResponse response)
            {
                Uri combine = new Uri(_baseuri,"/api" + api);
                _client.Request(Network.WebRequest.GET(Resolve(Path.Combine(combine.ToString(), api))), response);
            }

            public void Get(string api, WebClient.WebResponse response)
            {
                Uri combine = new Uri(_baseuri,"/api" + api);
                _client.Request(Network.WebRequest.GET(combine.ToString()), response);
            }

            public void Post(string api, JsonObject json, WebClient.WebResponse response)
            {
                Uri combine = new Uri(_baseuri,"/api" + api);
                _client.Request(Network.WebRequest.POST(combine.ToString(), json), response);
            }

            public void Put(string api, JsonObject json, WebClient.WebResponse response)
            {
                Uri combine = new Uri(_baseuri,"/api" + api);
                _client.Request(Network.WebRequest.PUT(combine.ToString(), json), response);
            }

            public void Delete(string api, JsonObject json, WebClient.WebResponse response)
            {
                Uri combine = new Uri(_baseuri,"/api" + api);
                _client.Request(Network.WebRequest.DELETE(combine.ToString(), json), response);
            }

            private void TransmissionBegin()
            {
                Indicator.Instance.Show(true);
            }

            private void TransmissionEnd()
            {
                Indicator.Instance.Show(false);
            }
        }
    }
}