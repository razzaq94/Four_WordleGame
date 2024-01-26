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
                    if (LogInManager.Instance != null)
                    {
                        LogInManager.Instance.OpenGameScene();
                    }
                    JSONNode body = JSONNode.Parse(resposne);
                    print("logIn Data ....  : " + body.ToString());
                    JSONArray boosters = body["user"]["boosters"].AsArray;
                    GlobalData.Instance.token = body["token"].Value;
                    GlobalData.Instance.userId = body["user"]["_id"].Value;
                    GlobalData.Instance.UserName = body["user"]["username"].Value;
                    GlobalData.Instance.level = Convert.ToInt32(body["user"]["level"].Value);
                    GlobalData.Instance.highestScore = Convert.ToInt32(body["user"]["highest_obtained_score"].Value);
                    GlobalData.Instance.isPremium = Convert.ToBoolean(body["user"]["isPremium"].Value);
                    GlobalData.Instance.totalScore = Convert.ToInt32(body["user"]["total_obtained_score"].Value);
                    GlobalData.Instance.totalGamesPlayed = Convert.ToInt32(body["user_stats"]["total_games_played"].Value);
                    GlobalData.Instance.totalGamesWon = Convert.ToInt32(body["user_stats"]["total_games_won"].Value);

                    GlobalData.Instance.RevealBoosterCount = Convert.ToInt32(body["user"]["boosters"][0]["count"].Value);
                    GlobalData.Instance.AutoColorBoosterCount = Convert.ToInt32(body["user"]["boosters"][1]["count"].Value);
 //                   GlobalData.Instance.EliminateBoosterCount = Convert.ToInt32(body["user"]["boosters"][2]["count"].Value);


                    if (body["user_stats"]["easy_mode"].Value != "null")
                    {
                        GlobalData.Instance.UserStats.EasyModeStats.Time.StandardDeviation = float.Parse(body["user_stats"]["easy_mode"]["time"]["sd"].Value);
                        GlobalData.Instance.UserStats.EasyModeStats.Time.Median = float.Parse(body["user_stats"]["easy_mode"]["time"]["median"].Value);
                        GlobalData.Instance.UserStats.EasyModeStats.Guess.StandardDeviation = float.Parse(body["user_stats"]["easy_mode"]["guesses"]["sd"].Value);
                        GlobalData.Instance.UserStats.EasyModeStats.Guess.Median = float.Parse(body["user_stats"]["easy_mode"]["guesses"]["median"].Value);
                    }
                    if (body["user_stats"]["medium_mode"].Value != "null")
                    {
                        GlobalData.Instance.UserStats.MediumModeStats.Time.StandardDeviation = float.Parse(body["user_stats"]["medium_mode"]["time"]["sd"].Value);
                        GlobalData.Instance.UserStats.MediumModeStats.Time.Median = float.Parse(body["user_stats"]["medium_mode"]["time"]["median"].Value);
                        GlobalData.Instance.UserStats.MediumModeStats.Guess.StandardDeviation = float.Parse(body["user_stats"]["medium_mode"]["guesses"]["sd"].Value);
                        GlobalData.Instance.UserStats.MediumModeStats.Guess.Median = float.Parse(body["user_stats"]["medium_mode"]["guesses"]["median"].Value);
                    }
                    print("hard mode : " + (body["user_stats"]["hard_mode"].Value).ToString());
                    if ((body["user_stats"]["hard_mode"].Value).ToString() != "null")
                    {
                        GlobalData.Instance.UserStats.HardModeStats.Time.StandardDeviation = float.Parse(body["user_stats"]["hard_mode"]["time"]["sd"].Value);
                        GlobalData.Instance.UserStats.HardModeStats.Time.Median = float.Parse(body["user_stats"]["hard_mode"]["time"]["median"].Value);
                        GlobalData.Instance.UserStats.HardModeStats.Guess.StandardDeviation = float.Parse(body["user_stats"]["hard_mode"]["guesses"]["sd"].Value);
                        GlobalData.Instance.UserStats.HardModeStats.Guess.Median = float.Parse(body["user_stats"]["hard_mode"]["guesses"]["median"].Value);
                    }





                    if ((body["global_data"]["easy_mode"].Value).ToString() != "null")
                    {
                        GlobalData.Instance.GlobalStats.EasyModeStats.Score.StandardDeviation = float.Parse(body["global_data"]["easy_mode"]["score"]["sd"].Value);
                        GlobalData.Instance.GlobalStats.EasyModeStats.Score.Median = float.Parse(body["global_data"]["easy_mode"]["score"]["median"].Value);
                        GlobalData.Instance.GlobalStats.EasyModeStats.Time.StandardDeviation = float.Parse(body["global_data"]["easy_mode"]["time"]["sd"].Value);
                        GlobalData.Instance.GlobalStats.EasyModeStats.Time.Median = float.Parse(body["global_data"]["easy_mode"]["time"]["median"].Value);
                        GlobalData.Instance.GlobalStats.EasyModeStats.Guess.StandardDeviation = float.Parse(body["global_data"]["easy_mode"]["guesses"]["sd"].Value);
                        GlobalData.Instance.GlobalStats.EasyModeStats.Guess.Median = float.Parse(body["global_data"]["easy_mode"]["guesses"]["median"].Value);
                    }
                    if ((body["global_data"]["hard_mode"].Value).ToString() != "null")
                    {
                        GlobalData.Instance.GlobalStats.MediumModeStats.Score.StandardDeviation = float.Parse(body["global_data"]["medium_mode"]["score"]["sd"].Value);
                        GlobalData.Instance.GlobalStats.MediumModeStats.Score.Median = float.Parse(body["global_data"]["medium_mode"]["score"]["median"].Value);
                        GlobalData.Instance.GlobalStats.MediumModeStats.Time.StandardDeviation = float.Parse(body["global_data"]["medium_mode"]["time"]["sd"].Value);
                        GlobalData.Instance.GlobalStats.MediumModeStats.Time.Median = float.Parse(body["global_data"]["medium_mode"]["time"]["median"].Value);
                        GlobalData.Instance.GlobalStats.MediumModeStats.Guess.StandardDeviation = float.Parse(body["global_data"]["medium_mode"]["guesses"]["sd"].Value);
                        GlobalData.Instance.GlobalStats.MediumModeStats.Guess.Median = float.Parse(body["global_data"]["medium_mode"]["guesses"]["median"].Value);
                    }
                    //if ((body["global_data"]["hard_mode"].Value).ToString() != "null")
                    //{
                    //    GlobalData.Instance.GlobalStats.HardModeStats.Score.StandardDeviation = float.Parse(body["global_data"]["hard_mode"]["score"]["sd"].Value);
                    //    GlobalData.Instance.GlobalStats.HardModeStats.Score.Median = float.Parse(body["global_data"]["hard_mode"]["score"]["median"].Value);
                    //    GlobalData.Instance.GlobalStats.HardModeStats.Time.StandardDeviation = float.Parse(body["global_data"]["hard_mode"]["time"]["sd"].Value);
                    //    GlobalData.Instance.GlobalStats.HardModeStats.Time.Median = float.Parse(body["global_data"]["hard_mode"]["time"]["median"].Value);
                    //    GlobalData.Instance.GlobalStats.HardModeStats.Guess.StandardDeviation = float.Parse(body["global_data"]["hard_mode"]["guesses"]["sd"].Value);
                    //    GlobalData.Instance.GlobalStats.HardModeStats.Guess.Median = float.Parse(body["global_data"]["hard_mode"]["guesses"]["median"].Value);
                    //}
                },
                (error, type) =>
                {
                    print("Error : " + error);
                    print("Type : " + type);

                    if (type == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
                    {
                        ErrorMessagePanelUI.ShowUI();
                    //    LogInManager.Instance.UpdateSceneActivationStatus(false);
                        





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
        LoadingPanel.ShowUI();
        NetworkAPIManager.Instance.UpdateUserName((newuserName),
                (resposne) =>
                {
                    JSONNode body = JSONNode.Parse(resposne);
                    if(Convert.ToBoolean( body["success"].Value) == true )
                    {
                        InGameStorage.Instance.SetUserName(newuserName);
                        GlobalData.Instance.UserName = newuserName;
                        ProfilePanelUI.Instance.SetUserName();
                        LoadingPanel.Instance.OnBackPressed();
                    }
                    else
                    {

                    }
                    print(body.ToString());
                },
                (error, type) =>
                {
                    LoadingPanel.Instance.OnBackPressed();

                    if (type == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
                    {
                        ErrorMessagePanelUI.ShowUI();
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
                        ErrorMessagePanelUI.ShowUI();

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
    public void GetLeaderboardData()
    {
        LoadingPanel.ShowUI();
        NetworkAPIManager.Instance.GetLeaderboardData((null),
                (resposne) =>
                {
                    LoadingPanel.Instance.OnBackPressed();
                    LeaderboardPanelUI.ShowUI();
                    JSONNode body = JSONNode.Parse(resposne);
                    JSONArray data = JSON.Parse(body["users"]).AsArray;

                    print("Leaderboard : " + body["users"][0]["username"].Value);
                    print("Leaderboard : " + body["users"].Count);
                    if (Convert.ToBoolean(body["success"].Value) == true)
                    {
                        LeaderboardPanelUI.Instance.SetLeaderboardData(body);

                    }
                    else
                    {

                    }
                 //   print(body.ToString());
                },
                (error, type) =>
                {
                    print("Error : " + error);
                    print("Type : " + type);
                    LoadingPanel.Instance.OnBackPressed();

                    if (type == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
                    {
                        ErrorMessagePanelUI.ShowUI();

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
