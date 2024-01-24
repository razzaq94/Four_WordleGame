using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using Unity.VisualScripting;

public class APIManager : MonoBehaviour
{

    public static APIManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LogIn()
    {
        GlobalData.Instance.logInRequest.user_id = InGameStorage.Instance.GetUserId();
        GlobalData.Instance.logInRequest.username = InGameStorage.Instance.GetUserName();
        NetworkAPIManager.Instance.LogIn((GlobalData.Instance.logInRequest),
                (resposne) =>
                {
                    JSONNode body = JSONNode.Parse(resposne);
                    print("logIn Data ....  : " + body.ToString());
                    JSONArray array = body["user"]["boosters"].AsArray;
                    GlobalData.Instance.token = body["token"].Value;
                    GlobalData.Instance.userId = body["user"]["_id"].Value;
                    GlobalData.Instance.UserName = body["user"]["username"].Value;
                    GlobalData.Instance.level = Convert.ToInt32(body["user"]["level"].Value);
                    GlobalData.Instance.highestScore = Convert.ToInt32(body["user"]["highest_obtained_score"].Value);
                    GlobalData.Instance.isPremium = Convert.ToBoolean(body["user"]["isPremium"].Value);
                    GlobalData.Instance.totalScore = Convert.ToInt32(body["user"]["total_obtained_score"].Value);
                    GlobalData.Instance.totalGamesPlayed = Convert.ToInt32(body["user_stats"]["total_games_played"].Value);
                    GlobalData.Instance.totalGamesWon = Convert.ToInt32(body["user_stats"]["total_games_won"].Value);
                //    GlobalData.Instance.UserStats.EasyModeStats.Score.StandardDeviation = Convert.ToInt64 (body["user_stats"]["easy_mode"]["score"]["sd"].Value);
                  //  GlobalData.Instance.UserStats.EasyModeStats.Score.Median = Convert.ToInt32 (body["user_stats"]["easy_mode"]["score"]["median"].Value);
               /*     GlobalData.Instance.UserStats.EasyModeStats.Time.StandardDeviation = Convert.ToInt64 (body["user_stats"]["easy_mode"]["time"]["sd"].Value);
                    GlobalData.Instance.UserStats.EasyModeStats.Time.Median = Convert.ToInt32 (body["user_stats"]["easy_mode"]["time"]["median"].Value);
                    GlobalData.Instance.UserStats.EasyModeStats.Guess.StandardDeviation = Convert.ToInt64 (body["user_stats"]["easy_mode"]["guesses"]["sd"].Value);
                    GlobalData.Instance.UserStats.EasyModeStats.Guess.Median = Convert.ToInt32 (body["user_stats"]["easy_mode"]["guesses"]["median"].Value);
                    
                    GlobalData.Instance.UserStats.MediumModeStats.Time.StandardDeviation = Convert.ToInt64(body["user_stats"]["medium_mode"]["time"]["sd"].Value);
                    GlobalData.Instance.UserStats.MediumModeStats.Time.Median = Convert.ToInt32(body["user_stats"]["medium_mode"]["time"]["median"].Value);
                    GlobalData.Instance.UserStats.MediumModeStats.Guess.StandardDeviation = Convert.ToInt64(body["user_stats"]["medium_mode"]["guesses"]["sd"].Value);
                    GlobalData.Instance.UserStats.MediumModeStats.Guess.Median = Convert.ToInt32(body["user_stats"]["medium_mode"]["guesses"]["median"].Value);

                    GlobalData.Instance.UserStats.HardModeStats.Time.StandardDeviation = Convert.ToInt64(body["user_stats"]["hard_mode"]["time"]["sd"].Value);
                    GlobalData.Instance.UserStats.HardModeStats.Time.Median = Convert.ToInt32(body["user_stats"]["hard_mode"]["time"]["median"].Value);
                    GlobalData.Instance.UserStats.HardModeStats.Guess.StandardDeviation = Convert.ToInt64(body["user_stats"]["hard_mode"]["guesses"]["sd"].Value);
                    GlobalData.Instance.UserStats.HardModeStats.Guess.Median = Convert.ToInt32(body["user_stats"]["hard_mode"]["guesses"]["median"].Value);
               */







             /*       GlobalData.Instance.GlobalStats.EasyModeStats.Score.StandardDeviation = Convert.ToInt64 (body["global_data"]["easy_mode"]["score"]["sd"].Value);
                    GlobalData.Instance.GlobalStats.EasyModeStats.Score.Median = Convert.ToInt32 (body["global_data"]["easy_mode"]["score"]["median"].Value);
                    GlobalData.Instance.GlobalStats.MediumModeStats.Time.StandardDeviation = Convert.ToInt64 (body["global_data"]["medium_mode"]["time"]["sd"].Value);
                    GlobalData.Instance.GlobalStats.MediumModeStats.Time.Median = Convert.ToInt32 (body["global_data"]["medium_mode"]["time"]["median"].Value);
 */               //    GlobalData.Instance.GlobalStats.HardModeStats.Guess.StandardDeviation = Convert.ToInt64 (body["global_data"]["hard_mode"]["guesses"]["sd"].Value);
                //    GlobalData.Instance.GlobalStats.HardModeStats.Guess.Median = Convert.ToInt32 (body["global_data"]["hard_mode"]["guesses"]["median"].Value); 
                    
                },
                (error, type) =>
                {
                    print("Error : " + error);
                    print("Type : " + type);

                    if (type == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
                    {

                        //....... For testing only 
                        //  GameManager.Instance.ResetGame();
                        //..........

                        //            string title = "Network Error";
                        //         string definition = "No internet connection. Please try again.";
                        //             ErrorPanelUI.ShowUI();
                        //       ErrorPanelUI.instance.setText(title, definition);
                    }
                    else
                    {
                        //     if (LoadingPanelUI.instance != null)
                        //   {
                        //     LoadingPanelUI.instance.OnBackPressed();
                        //}

                        //    UIManager.Instance.UpdatePlayerGhostUI(false, CharList, CharacterColorList);
                    }
                });

    }


    public void UpdateUserName(string newuserName)
    {
        NetworkAPIManager.Instance.UpdateUserName((newuserName),
                (resposne) =>
                {
                    JSONNode body = JSONNode.Parse(resposne);
                    if(Convert.ToBoolean( body["success"].Value) == true )
                    {

                    }
                    else
                    {

                    }
                    print(body.ToString());
                },
                (error, type) =>
                {
                    print("Error : " + error);
                    print("Type : " + type);

                    if (type == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
                    {

                        //....... For testing only 
                        //  GameManager.Instance.ResetGame();
                        //..........

                        //            string title = "Network Error";
                        //         string definition = "No internet connection. Please try again.";
                        //             ErrorPanelUI.ShowUI();
                        //       ErrorPanelUI.instance.setText(title, definition);
                    }
                    else
                    {
                        //     if (LoadingPanelUI.instance != null)
                        //   {
                        //     LoadingPanelUI.instance.OnBackPressed();
                        //}

                        //    UIManager.Instance.UpdatePlayerGhostUI(false, CharList, CharacterColorList);
                    }
                });
    }
    public void SaveGameData(string CurrentWord, int CurrenGameTime, int _score, int noOfGuesses, bool isSolved, string gameMode)
    {
        GlobalData.Instance.logInRequest.user_id = InGameStorage.Instance.GetUserId();
        GlobalData.Instance.logInRequest.username = InGameStorage.Instance.GetUserName();
        var  obj = new { word = CurrentWord, time = CurrenGameTime, score = _score, no_of_guesses = noOfGuesses, is_solved = isSolved, game_mode = gameMode };
        NetworkAPIManager.Instance.SaveGameData((JsonUtility.ToJson(obj)),
                (resposne) =>
                {
                    JSONNode body = JSONNode.Parse(resposne);
                    if (Convert.ToBoolean(body["success"].Value) == true)
                    {

                    }
                    else
                    {

                    }
                    print(body.ToString());
                },
                (error, type) =>
                {
                    print("Error : " + error);
                    print("Type : " + type);

                    if (type == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
                    {

                        //....... For testing only 
                        //  GameManager.Instance.ResetGame();
                        //..........

                        //            string title = "Network Error";
                        //         string definition = "No internet connection. Please try again.";
                        //             ErrorPanelUI.ShowUI();
                        //       ErrorPanelUI.instance.setText(title, definition);
                    }
                    else
                    {
                        //     if (LoadingPanelUI.instance != null)
                        //   {
                        //     LoadingPanelUI.instance.OnBackPressed();
                        //}

                        //    UIManager.Instance.UpdatePlayerGhostUI(false, CharList, CharacterColorList);
                    }
                });
    }
}
