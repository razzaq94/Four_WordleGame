using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Unity.VisualScripting.FullSerializer;
using SimpleJSON;

public class NetworkAPIManager : SingletonBehaviourGameObject<NetworkAPIManager>
{
    
    public void CheckWordOnline(string word, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        new WebRequest(this, null, "https://api.dictionaryapi.dev/api/v2/entries/en/" + word, RequestMethod.GET, EncodeMethod.JSON, OnSuccess, OnFail);
    } 
    public void LogIn(LogInRequest logInRequest, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        
        new WebRequest(this, new { user_id =logInRequest.user_id, username =logInRequest.username}, "https://playwordage.com/api/login", RequestMethod.POST, EncodeMethod.JSON, OnSuccess, OnFail);
    }
    public void UpdateUserName(string newuserName, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        new WebRequest(this, new { username = newuserName }, "https://playwordage.com/api/update-username", GlobalData.Instance.token , RequestMethod.POST, EncodeMethod.JSON, OnSuccess, OnFail);
    } 
    public void SaveGameData(string userData, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        new WebRequest(this, new { word = "Take", time = 322, score = 176, no_of_guesses = 13, is_solved = true, game_mode = "Easy" }, "https://playwordage.com/api/save-game-data", GlobalData.Instance.token , RequestMethod.POST, EncodeMethod.JSON, OnSuccess, OnFail);
    }
    public void GetLeaderboardData(string userData, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        new WebRequest(this, null, "https://playwordage.com/api/get-leaderboard-data", GlobalData.Instance.token , RequestMethod.GET, EncodeMethod.JSON, OnSuccess, OnFail);
    }
}
