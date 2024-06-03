using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.SceneManagement;
public class GameOverPanelUI : MonoBehaviour
{
	//[SerializeField] TextMeshProUGUI resultMessage;
	[SerializeField] TextMeshProUGUI ScoreUI;
	[SerializeField] TextMeshProUGUI TimeUI;
	[SerializeField] TextMeshProUGUI GuessesUI;
	[SerializeField] TextMeshProUGUI FasterUI;
	[SerializeField] TextMeshProUGUI QuickestUI;
	[SerializeField] TextMeshProUGUI WordSolutionUI;

	[SerializeField] TextMeshProUGUI word;
	[SerializeField] TextMeshProUGUI definition;

	[SerializeField] private Animator PanelAnimator;
	[SerializeField] private Animator PanelFadingAnimator;
	private List<string> panelExitAnimationCondition = new List<string>();

	public static GameOverPanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}
    private void Start()
    {
		panelExitAnimationCondition.Add("IsLeftOut");
		panelExitAnimationCondition.Add("IsRightOut");
		panelExitAnimationCondition.Add("IsBottomOut");

	}
	public static GameOverPanelUI ShowUI()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();

		if (Instance == null)
		{
			SoundManager.instance.Play_PANEL_INSTANTIATE_Sound();
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/GameOverPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<GameOverPanelUI>();
		}
		return Instance;
	}
	public void SetText(string rMessage, string wrd, string def )
    {


		ScoreUI.text = ScoreManager.Instance.currentGameScore.ToString();
		float time = ((int)Timer.Instance.MaxTime - (int)Timer.Instance.CurrentTime);
		float minutes = Mathf.FloorToInt(time / 60);
		float seconds = Mathf.FloorToInt(time % 60);
		TimeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);

		//TimeUI.text = ((int)Timer.Instance.MaxTime - (int)Timer.Instance.CurrentTime).ToString();
		GuessesUI.text = (UIManager.Instance.ContentHolder.transform.childCount +1).ToString();

		/*
		FasterUI.text = "YOU ARE FASTER THAN " + GlobalData.Instance.FasterTimeInPercent + "% OF PEOPLE";
		QuickestUI.text = "YOU USE LESS WORDS THAN " + GlobalData.Instance.LessGuessesInPercent + "% OF PEOPLE";
		WordSolutionUI.text = "ONLY " + GlobalData.Instance.PeopleSolvedThisWordInPercent + "% OF PEOPLE SOLVED THIS WORD";
		*/
		//resultMessage.text = rMessage;
		word.text = wrd;
		definition.text = def;
    }
	public void OnBackPressed()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		SoundManager.instance.Play_PANEL_DESTROY_Sound();
		StartCoroutine(waitAndDestroy());
	}
	IEnumerator waitAndDestroy()
	{
		PanelAnimator.SetBool(panelExitAnimationCondition[Random.Range(0, 3)], true);
		PanelFadingAnimator.SetBool("IsOut", true);
		yield return new WaitForSeconds(0.8f);
		Destroy(gameObject);

	}
	public void CallToQuitGame()
	{
		if (GlobalData.Instance.isPremium == false)
		{
			AdsManager.instance.ShowInterstitialAd();
		} 
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		GameManager.Instance.BackToMainMenu();
	}
	public void CallToPlayAgainGame()
	{
        if (GlobalData.Instance.isPremium == false)
        {
            AdsManager.instance.ShowInterstitialAd();
        }
        SoundManager.instance.Play_BUTTON_CLICK_Sound();
		GameManager.Instance.ReloadLevel();
	}
}
