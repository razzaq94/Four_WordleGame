using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Unity.VisualScripting.FullSerializer;

public class NetworkAPIManager : SingletonBehaviourGameObject<NetworkAPIManager>
{
    
    public void CheckWordOnline(string word, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        print("NetworkAPIManager....");
        new WebRequest(this, null, "https://api.dictionaryapi.dev/api/v2/entries/en/" + word, RequestMethod.GET, EncodeMethod.JSON, OnSuccess, OnFail);
    } 
    public void LogIn(string word, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        print("NetworkAPIManager....");
        new WebRequest(this, null, "https://api.dictionaryapi.dev/api/v2/entries/en/" + word, RequestMethod.GET, EncodeMethod.JSON, OnSuccess, OnFail);
    }
}
