using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] public float CurrentTime = 3f;
    //   [SerializeField] private Image progressImage;
    private bool stopTimer = false;
    public static TimerUp Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if (stopTimer == false)
        {
            CurrentTime += Time.deltaTime;

            float minutes = Mathf.FloorToInt(CurrentTime / 60);
            float seconds = Mathf.FloorToInt(CurrentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            //timerText.text = CurrentTime.ToString("00");

        }
        else if (stopTimer == false)
        {
            StopTimer(true);
            GameOverPanelUI.ShowUI();
            GameOverPanelUI.Instance.SetText("BetterLuck next time !!!", WordManager.Instance.WordToGuess, WordManager.Instance.WordDefinition);
        }

    }
    public void ResetTimer()
    {

        CurrentTime = 0;

    }
    public void StopTimer(bool status)
    {       
        stopTimer = status;
        timerText.text = string.Format("{0:00}:{1:00}", 0, 0);
    }
}
