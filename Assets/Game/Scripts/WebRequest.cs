using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;


public class WebRequest
{


    Coroutine coroutine;

    public WebRequest(MonoBehaviour mono, object data, string url, RequestMethod method, EncodeMethod encode, Action<string> onSuccess = null, Action<string, UnityWebRequest.Result> onFail = null)
    {
        coroutine = mono.StartCoroutine(ToSendWebRequest(data, url, null, method, encode, onSuccess, onFail));
    }

    public WebRequest(MonoBehaviour mono, object data, string url, string authToken, RequestMethod method, EncodeMethod encode, Action<string> onSuccess = null, Action<string, UnityWebRequest.Result> onFail = null)
    {
        coroutine = mono.StartCoroutine(ToSendWebRequest(data, url, authToken, method, encode, onSuccess, onFail));
    }

    public WebRequest(MonoBehaviour mono, string url, Action<Sprite> onSuccess = null, Action<string, UnityWebRequest.Result> onFail = null)
    {
        coroutine = mono.StartCoroutine(ToSendTextureWebRequest(url, onSuccess, onFail));
    }

    public void Stop(MonoBehaviour mono)
    {
        if (coroutine != null)
        {
            mono.StopCoroutine(coroutine);
        }
    }

    IEnumerator ToSendTextureWebRequest(string url, Action<Sprite> onSuccess, Action<string, UnityWebRequest.Result> onFail)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            onFail?.Invoke("Internet not connected", UnityWebRequest.Result.ConnectionError);
            yield break;
        }

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        request.downloadHandler = new DownloadHandlerTexture();
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.ConnectionError)
        {
            if (request.result != UnityWebRequest.Result.ProtocolError)
            {
                Texture2D obtainedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite s = Sprite.Create(obtainedTexture, new Rect(0f, 0f, obtainedTexture.width, obtainedTexture.height), Vector2.zero);
                onSuccess?.Invoke(s);
            }
            else
            {
                onFail?.Invoke(request.downloadHandler.text, request.result);
            }
        }
        else
        {
            onFail?.Invoke(request.error, request.result);
        }
        request.Dispose();
    }

    IEnumerator ToSendWebRequest(object data, string url, string authToken, RequestMethod method, EncodeMethod encode, Action<string> onSuccess, Action<string, UnityWebRequest.Result> onFail)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            onFail?.Invoke("Internet not connected", UnityWebRequest.Result.ConnectionError);
            yield break;
        }

        UnityWebRequest request = null;
        if (method == RequestMethod.POST)
        {
            if (encode == EncodeMethod.JSON)
            {
                string s = JsonUtility.ToJson(data);
                Debug.Log("DATA::: " + s);
                byte[] jsonBinary = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
                UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
                uploadHandlerRaw.contentType = "application/json";
                request = new UnityWebRequest(url, method.ToString(), downloadHandlerBuffer, uploadHandlerRaw);
            }
            else if (encode == EncodeMethod.FORM)
            {
                if (data.GetType().Equals(typeof(WWWForm)))
                {
                    WWWForm formData = (WWWForm)data;
                    request = UnityWebRequest.Post(url, formData);
                    request.downloadHandler = new DownloadHandlerBuffer();
                }
                else
                {
                    List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
                    foreach (PropertyInfo property in data.GetType().GetRuntimeProperties())
                    {
                        formData.Add((IMultipartFormSection)property.GetValue(data));
                    }
                    request = UnityWebRequest.Post(url, formData);
                    request.downloadHandler = new DownloadHandlerBuffer();
                }
            }
        }
        else if (method == RequestMethod.GET)
        {
            if (data != null)
            {
                url += "?";
                foreach (PropertyInfo property in data.GetType().GetRuntimeProperties())
                {
                    url += string.Format("{0}={1}&", property.Name, property.GetValue(data));
                }
                url = url.Remove(url.Length - 1);
            }
            request = UnityWebRequest.Get(url);
            request.downloadHandler = new DownloadHandlerBuffer();
        }

        if (authToken != null)
        {
            request.SetRequestHeader("x-auth-token", authToken);
        }


        Debug.Log("New Header: " + SystemInfo.deviceUniqueIdentifier + "/" + Application.platform + "/" + SystemInfo.operatingSystem);
        request.SetRequestHeader("User-Agent", (SystemInfo.deviceUniqueIdentifier + "/" + Application.platform + "/" + SystemInfo.operatingSystem).ToString());
        //request.SetRequestHeader("x-real-ip", IP);


        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.ConnectionError)
        {
            if (request.result != UnityWebRequest.Result.ProtocolError)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onFail?.Invoke(request.downloadHandler.text, request.result);
            }
        }
        else
        {
            onFail?.Invoke(request.error, request.result);
        }
        request.Dispose();
    }

}

public enum RequestMethod
{
    GET,
    POST
}

public enum EncodeMethod
{
    JSON,
    FORM
}
