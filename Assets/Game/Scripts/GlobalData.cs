using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalData : MonoBehaviour
{
    public GameMode gameMode = GameMode.Hard;
    public List<string> WordList;
    public List<string> TempWordList;
    public LogInRequest logInRequest = new LogInRequest();

    //User data 
    public string token = null;
    public string userId = null;
    public string UserName = null;    
    public int level = 0;
    public int highestScore = 0;
    public int totalScore = 0;
    public bool isPremium = false;
    public int totalGamesPlayed = 0;
    public int totalGamesWon = 0;
    public Stats UserStats = new Stats();
    public Stats GlobalStats = new Stats();
    public float EasyFasterTimePercent = 0;
    public float MediumFasterTimePercent = 0;
    public float HardFasterTimePercent = 0;
    public float EasyLessGuessPercent = 0;
    public float MediumLessGuessPercent = 0;
    public float HardLessGuessPercent = 0;

    public float PeopleSolvedThisWordInPercent = 0;
    public float FasterTimeInPercent = 0;
    public float LessGuessesInPercent = 0;


    public int RevealBoosterCount = 0;
    public int AutoColorBoosterCount = 0;
    public int EliminateBoosterCount = 0;
    
    public string RevealBoosterId = null;
    public string AutoColorBoosterId = null;
    public string EliminateBoosterId = null;

    public bool IsRestart = false;

    public Boosters[] boosters = new Boosters[3];
    public SaveBoosters[] saveBoosters = new SaveBoosters[3];

    public bool isRevealBoosterAdShown = false;
    public bool isEliminateBoosterAdShown = false;





    public static GlobalData Instance;
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
    public void SetGamemode(int gm)
    {
        if(gm == 1)
        {
            gameMode = GameMode.Easy;
        }
        else if(gm == 2)
        {
            gameMode = GameMode.Medium;

        }
        else if(gm == 3)
        {
            gameMode = GameMode.Hard;

        }
    }
    public void Loadlevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public enum GameMode
    {
        Easy = 4,
        Medium = 5,
        Hard = 6
    }
    public void UpdateBoosterCount(string boosterName, int change)
    {
        string boosterId;
        if (boosterName == "Reveal")
        {
            RevealBoosterCount += change;
            boosterId = RevealBoosterId;

            Boosters mybooster = new Boosters();
            mybooster.id = RevealBoosterId;
            mybooster.count = RevealBoosterCount;
            boosters[0] = mybooster; 
            
            SaveBoosters mysavebooster = new SaveBoosters();
            mysavebooster.name = "Booster1";
            mysavebooster.count = RevealBoosterCount;
            saveBoosters[0] = mysavebooster;
        }
        else if (boosterName == "AutoColor")
        {
            AutoColorBoosterCount += change;
            //  boosterId = AutoColorBoosterId;
            
            Boosters mybooster = new Boosters();
            mybooster.id = AutoColorBoosterId;
            mybooster.count = AutoColorBoosterCount;
            boosters[1] = mybooster;

            SaveBoosters mysavebooster = new SaveBoosters();
            mysavebooster.name = "Booster2";
            mysavebooster.count = AutoColorBoosterCount;
            saveBoosters[1] = mysavebooster;


        }
        else if (boosterName == "Eliminate")
        {
            EliminateBoosterCount += change;
            //boosterId = EliminateBoosterId;
            
            Boosters mybooster = new Boosters();
            mybooster.id = EliminateBoosterId;
            mybooster.count = EliminateBoosterCount;
            boosters[2] = mybooster;


            SaveBoosters mysavebooster = new SaveBoosters();
            mysavebooster.name = "Booster3";
            mysavebooster.count = EliminateBoosterCount;
            saveBoosters[2] = mysavebooster;

        }

          APIManager.Instance.UpdateUserData();

    }

}
