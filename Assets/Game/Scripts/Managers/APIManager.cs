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


        if (GlobalData.Instance.IsRestart == false)
        {
        //    print("111");
            GlobalData.Instance.logInRequest.user_id = InGameStorage.Instance.GetUserId();
        }
        else
        {
          //  print("222");
            GlobalData.Instance.logInRequest.user_id = GlobalData.Instance.userId;
            if (GlobalData.Instance.logInRequest.user_id.Length >= 24)
            {
                GlobalData.Instance.logInRequest.user_id = GlobalData.Instance.logInRequest.user_id.Substring(0, 24);
            }
        }
        for(int i =0; i < GlobalData.Instance.logInRequest.user_id.Length; i++ )
        {
            //print(GlobalData.Instance.logInRequest.user_id[i]);
        }
        GlobalData.Instance.logInRequest.username = InGameStorage.Instance.GetUserName().ToLower();
        
        

        NetworkAPIManager.Instance.LogIn((GlobalData.Instance.logInRequest),
                (resposne) =>
                {
                    GlobalData.Instance.IsRestart = false;

                    LogInManager.Instance.UpdateSceneActivationStatus(true);
                    if (LogInManager.Instance != null)
                    {
                     //   LogInManager.Instance.OpenGameScene();
                    }
                    JSONNode body = JSONNode.Parse(resposne);
              //      print("logIn Data ....  : " + body.ToString());
                    JSONArray boosters = body["user"]["boosters"].AsArray;
                    GlobalData.Instance.token = body["token"].Value;
                    GlobalData.Instance.userId = body["user"]["_id"].Value;
                    InGameStorage.Instance.SetUserId(GlobalData.Instance.userId);
                    GlobalData.Instance.UserName = body["user"]["username"].Value.ToUpper();
                    InGameStorage.Instance.SetUserName(GlobalData.Instance.UserName.ToUpper());
                    GlobalData.Instance.level = Convert.ToInt32(body["user"]["level"].Value);
                    GlobalData.Instance.highestScore = Convert.ToInt32(body["user"]["highest_obtained_score"].Value);
                    GlobalData.Instance.isPremium = Convert.ToBoolean(body["user"]["isPremium"].Value);
                    InGameStorage.Instance.SetPremiumStatus(GlobalData.Instance.isPremium);
                    GlobalData.Instance.totalScore = Convert.ToInt32(body["user"]["total_obtained_score"].Value);
                    GlobalData.Instance.totalGamesPlayed = Convert.ToInt32(body["total_games_played"].Value);
                    GlobalData.Instance.totalGamesWon = Convert.ToInt32(body["total_games_won"].Value);
                    for(int i= 0; i < boosters.Count;i++)
                    {
                       if(boosters[i]["name"].Value == "Booster1")
                        {
                            GlobalData.Instance.RevealBoosterCount = Convert.ToInt32(body["user"]["boosters"][i]["count"].Value);
                            //GlobalData.Instance.RevealBoosterCount = 5;
                            GlobalData.Instance.RevealBoosterId = body["user"]["boosters"][i]["_id"].Value;

                        }
                        else if(boosters[i]["name"] == "Booster2")
                        {
                            GlobalData.Instance.AutoColorBoosterCount = Convert.ToInt32(body["user"]["boosters"][i]["count"].Value);
                            GlobalData.Instance.AutoColorBoosterId = body["user"]["boosters"][i]["_id"].Value;

                        }
                        else if(boosters[i]["name"] == "Booster3")
                        {
                            GlobalData.Instance.EliminateBoosterCount = Convert.ToInt32(body["user"]["boosters"][i]["count"].Value);
                            GlobalData.Instance.EliminateBoosterId = body["user"]["boosters"][i]["_id"].Value;

                        }
                    }

                    //if(body.HasKey("faster_time_percentages") && body["faster_time_percentages"].HasKey("easy_mode"))
                    //{

                    //}



                    GlobalData.Instance.EasyFasterTimePercent =  (body["faster_time_percentages"].HasKey("easy_mode"))? float.Parse(body["faster_time_percentages"]["easy_mode"].Value): 0f;
                    GlobalData.Instance.MediumFasterTimePercent = (body["faster_time_percentages"].HasKey("medium_mode"))? float.Parse(body["faster_time_percentages"]["medium_mode"].Value):0f;
                    GlobalData.Instance.HardFasterTimePercent =   (body["faster_time_percentages"].HasKey("hard_mode"))? float.Parse(body["faster_time_percentages"]["hard_mode"].Value):0f;
                    
                    GlobalData.Instance.EasyLessGuessPercent = ((body["less_guesses_percentages"].HasKey("easy_mode"))) ?  float.Parse(body["less_guesses_percentages"]["easy_mode"].Value):0f;
                    GlobalData.Instance.MediumLessGuessPercent = ((body["less_guesses_percentages"].HasKey("medium_mode"))) ?float.Parse(body["less_guesses_percentages"]["medium_mode"].Value):0f;
                    GlobalData.Instance.HardLessGuessPercent =  ((body["less_guesses_percentages"].HasKey("hard_mode"))) ? float.Parse(body["less_guesses_percentages"]["hard_mode"].Value):0f;


             //       GlobalData.Instance.RevealBoosterCount = Convert.ToInt32(body["user"]["boosters"][0]["count"].Value);
               //     GlobalData.Instance.AutoColorBoosterCount = Convert.ToInt32(body["user"]["boosters"][1]["count"].Value);
 //                   GlobalData.Instance.EliminateBoosterCount = Convert.ToInt32(body["user"]["boosters"][2]["count"].Value);


                  /*  if (body["user_stats"]["easy_mode"].Value != "null")
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
                    if ((body["user_stats"]["hard_mode"].Value) != "null")
                    {
                        GlobalData.Instance.UserStats.HardModeStats.Time.StandardDeviation = float.Parse(body["user_stats"]["hard_mode"]["time"]["sd"].Value);
                        GlobalData.Instance.UserStats.HardModeStats.Time.Median = float.Parse(body["user_stats"]["hard_mode"]["time"]["median"].Value);
                        GlobalData.Instance.UserStats.HardModeStats.Guess.StandardDeviation = float.Parse(body["user_stats"]["hard_mode"]["guesses"]["sd"].Value);
                        GlobalData.Instance.UserStats.HardModeStats.Guess.Median = float.Parse(body["user_stats"]["hard_mode"]["guesses"]["median"].Value);
                    }
                  */





                    /*if ((body["global_data"]["easy_mode"].Value).ToString() != "null")
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
                    //}*/
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
                        InvalidUserIdMessagePanelUI.ShowUI();
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
                    }
                    else
                    {
                    }
                });
    }
    public void UpdatePremiumStatus( )
    {
        LoadingPanel.ShowUI();
        NetworkAPIManager.Instance.UpdatePremiumStatus(
                (resposne) =>
                {
                    JSONNode body = JSONNode.Parse(resposne);
                    if (Convert.ToBoolean(body["success"].Value) == true)
                    {
                       
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
                    }
                    else
                    {
                    }
                });
    }
    public void SaveGameData(bool isSolved)
    {
        LoadingPanel.ShowUI();
        NetworkAPIManager.Instance.SaveGameData((isSolved),
                (resposne) =>
                {
                   // AdsManager.instance.ShowInterstitialAd();

                    if (isSolved)
                    {
                        SoundManager.instance.Play_VICTORY_COMPLETE_Sound();
                        SoundManager.instance.Play_BUTTON_Vibrate();

                    }
                    else
                    {
                        SoundManager.instance.Play_LEVEL_FAILED_Sound();
                        SoundManager.instance.Play_BUTTON_Vibrate();


                    }
                    LoadingPanel.Instance.OnBackPressed();
                    JSONNode body = JSONNode.Parse(resposne);
                    if (Convert.ToBoolean(body["success"].Value) == true)
                    {

                        GlobalData.Instance.PeopleSolvedThisWordInPercent = float.Parse(body["people_solved_this_word_in_percent"].Value);
                        GlobalData.Instance.FasterTimeInPercent = float.Parse(body["faster_time_in_percent"].Value);
                        GlobalData.Instance.LessGuessesInPercent = float.Parse(body["less_guesses_in_percent"].Value);


                        GameOverPanelUI.ShowUI();
                        GameOverPanelUI.Instance.SetText("Congratulations !!!", WordManager.Instance.WordToGuess, WordManager.Instance.WordDefinition);
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
    public void UpdateUserData()
    {
       // LoadingPanel.ShowUI();
        NetworkAPIManager.Instance.UpdateUserData(
                (resposne) =>
                {
           //         LoadingPanel.Instance.OnBackPressed();
                    JSONNode body = JSONNode.Parse(resposne);
                    if (Convert.ToBoolean(body["success"].Value) == true)
                    {

                    }
                    print(body.ToString());
                },
                (error, type) =>
                {
                    print("Error : " + error);
                    print("Type : " + type);
         //           LoadingPanel.Instance.OnBackPressed();

                    if (type == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
                    {
                        ErrorMessagePanelUI.ShowUI();

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
