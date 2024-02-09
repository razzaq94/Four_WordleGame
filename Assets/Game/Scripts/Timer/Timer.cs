using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] public float MaxTime ;
    [SerializeField] public float CurrentTime ;
    //   [SerializeField] private Image progressImage;
    private bool stopTimer = false;
    public static Timer Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if (CurrentTime > 0f && stopTimer == false)
        {
            CurrentTime -= Time.deltaTime;

            float minutes = Mathf.FloorToInt(CurrentTime / 60);
            float seconds = Mathf.FloorToInt(CurrentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            //timerText.text = CurrentTime.ToString("00");

        }
        else if (stopTimer == false)
        {
            StopTimer(true);
            APIManager.Instance.SaveGameData(false);
            ScoreManager.Instance.CalculateScore(UIManager.Instance.ContentHolder.transform.childCount + 1, ((int)Timer.Instance.MaxTime - (int)Timer.Instance.CurrentTime),false);

            KeyboardManager.Instance.IsInterectable = true;
            UIManager.Instance.UpdateInterectabilityBossterButtons(true);
            UIManager.Instance.UpdateInterectabilityBackButton(true);
            //GameOverPanelUI.ShowUI();
            //GameOverPanelUI.Instance.SetText("BetterLuck next time !!!", WordManager.Instance.WordToGuess,WordManager.Instance.WordDefinition);
        }

    }
    public void ResetTimer()
    {

        CurrentTime = MaxTime;

    }
    public void StopTimer(bool status)
    {
        //    print("Timer : " + status);

        stopTimer = status;
        timerText.text = string.Format("{0:00}:{1:00}", 0, 0);

    }

}
