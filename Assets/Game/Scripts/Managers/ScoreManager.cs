using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
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
    public void CalculateScore(int numberOfTurns, int time)
    {
        int turnFactor = (numberOfTurns < 1) ? 1 : numberOfTurns;
        int timeFactor = (time < 30) ? 1 : time % 30;
        int score = ((100 / turnFactor) + (1000 / timeFactor)) * ((int)GlobalData.Instance.gameMode - 3);
        UpdateScore(score);
        UpdateLevel();
        
    }
    public void UpdateScore(int scr)
    {
        InGameStorage.Instance.SetScore(scr);
    }  
    public void UpdateLevel()
    {
        int scr = InGameStorage.Instance.GetScore();
        int  lvl = ((scr < 1000) ? 0 : (scr / 1000));
        print("Score : " + scr + " Level : " + lvl);
        InGameStorage.Instance.SetLevel(lvl);
    }

}
