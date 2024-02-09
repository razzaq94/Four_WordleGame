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
        if (logInRequest.user_id != "")
        {
            print("1");
            new WebRequest(this, new { user_id = logInRequest.user_id, username = logInRequest.username }, "https://playwordage.com/api/login", RequestMethod.POST, EncodeMethod.JSON, OnSuccess, OnFail);
        }
        else
        {
            print("2");
            new WebRequest(this, new LogInRequest { user_id = null, username = logInRequest.username }, "https://playwordage.com/api/login", RequestMethod.POST, EncodeMethod.JSON, OnSuccess, OnFail);
        }
    }
    public void UpdateUserName(string newuserName, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        new WebRequest(this, new { username = newuserName }, "https://playwordage.com/api/update-username", GlobalData.Instance.token , RequestMethod.POST, EncodeMethod.JSON, OnSuccess, OnFail);
    } 
    public void SaveGameData(bool IsSolved, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        string mode = null;
        if(GlobalData.GameMode.Easy == GlobalData.Instance.gameMode)
        {
            mode = "Easy";
        }
        else if (GlobalData.GameMode.Medium == GlobalData.Instance.gameMode)
        {
            mode = "Medium";
        }
        else if (GlobalData.GameMode.Hard == GlobalData.Instance.gameMode)
        {
            mode = "Hard";
        }
        print("Is solved : " + IsSolved);
        new WebRequest(this, new { word = WordManager.Instance.WordToGuess, time = ((Timer.Instance.MaxTime+2) - Timer.Instance.CurrentTime), score = ScoreManager.Instance.currentGameScore, no_of_guesses = (UIManager.Instance.ContentHolder.transform.childCount+1), is_solved = IsSolved, game_mode = mode, boosters= GlobalData.Instance.boosters }, "https://playwordage.com/api/save-game-data", GlobalData.Instance.token , RequestMethod.POST, EncodeMethod.JSON, OnSuccess, OnFail);
    }
    public void UpdateUserData(Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        
        new WebRequest(this, new { level = GlobalData.Instance.level, boosters = GlobalData.Instance.saveBoosters }, "https://playwordage.com/api/update-user-data", GlobalData.Instance.token , RequestMethod.POST, EncodeMethod.JSON, OnSuccess, OnFail);

    }
    public void GetLeaderboardData(string userData, Action<string> OnSuccess, Action<string, UnityWebRequest.Result> OnFail)
    {
        new WebRequest(this, null, "https://playwordage.com/api/get-leaderboard-data", GlobalData.Instance.token , RequestMethod.GET, EncodeMethod.JSON, OnSuccess, OnFail);
    }
}
