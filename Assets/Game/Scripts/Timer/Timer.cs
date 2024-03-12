using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] public float MaxTime ;
    [SerializeField] public float CurrentTime ;
    [SerializeField] public GameObject Needle ;
    //   [SerializeField] private Image progressImage;
    private bool stopTimer = false;
    private bool isRingTimer = false;
    public static Timer Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Needle.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
    }
    void Update()
    {
        if (CurrentTime > 0f && stopTimer == false)
        {
            CurrentTime -= Time.deltaTime;


            float minutes = Mathf.FloorToInt(CurrentTime / 60);
            float seconds = Mathf.FloorToInt(CurrentTime % 60);
            if (CurrentTime > 0f)
            {
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            float deltaRotation = (360 / MaxTime) * Time.deltaTime;
           
            Needle.transform.localEulerAngles = new Vector3(0f, 0f, Needle.transform.localEulerAngles.z - deltaRotation);

            //timerText.text = CurrentTime.ToString("00");
            if (CurrentTime <=11f && isRingTimer ==false)
            {
                isRingTimer = true;
                SoundManager.instance.Play_COUNT_DOWN_Sound();
            }

        }
        else if (stopTimer == false)
        {
            stopTimer = true;
            CurrentTime = 0f;
            timerText.text = string.Format("{0:00}:{1:00}", 0, 0);

          //  StopTimer(true);
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
        isRingTimer = false;
        Needle.transform.localEulerAngles = new Vector3(0f, 0f, 90f);

    }
    public void StopTimer(bool status)
    {
        //    print("Timer : " + status);

        stopTimer = status;
     //   timerText.text = string.Format("{0:00}:{1:00}", 0, 0);

    }

}
