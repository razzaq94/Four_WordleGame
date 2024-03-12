using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentGameScore = 0;
    public static ScoreManager Instance;
    // Start is called before the first frame update
    private void Awake()
    {

        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CalculateScore(int numberOfTurns, int time,bool isSolved)
    {
        if (isSolved)
        {
            int turnFactor = (numberOfTurns < 1) ? 1 : numberOfTurns;
            int timeFactor = (time < 30) ? 1 : time % 30;
           // currentGameScore = ((100 / turnFactor) + (100 / timeFactor)) * (((int)GlobalData.Instance.gameMode - 3));
            currentGameScore = ((100 / turnFactor) + (100 / timeFactor)) * (((int)GlobalData.Instance.gameMode))/5;
        }
        else
        {
            currentGameScore = 0;
        }
        GlobalData.Instance.totalScore += currentGameScore;
        UpdateScore(currentGameScore);
        UpdateLevel();
        
    }
    public void UpdateScore(int scr)
    {
        InGameStorage.Instance.SetScore(scr);
    }  
    public void UpdateLevel()
    {
        //  int scr = InGameStorage.Instance.GetScore();
        int scr = GlobalData.Instance.totalScore;

        //int  lvl = ((scr < 1000) ? 0 : (scr / 1000));
        int  lvl =scr / 1000 ;
        print("Score : " + scr + " Level : " + lvl);
        InGameStorage.Instance.SetLevel(lvl);
        GlobalData.Instance.level = lvl;
    }

}
