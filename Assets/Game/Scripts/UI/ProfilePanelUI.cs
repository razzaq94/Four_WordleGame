using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Numerics;

public class ProfilePanelUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI Name;
	[SerializeField] TextMeshProUGUI LevelNumber;
	[SerializeField] Image FillImage;
	[SerializeField] TextMeshProUGUI TargetText;


	[SerializeField] TextMeshProUGUI HighestScore;
	[SerializeField] TextMeshProUGUI WonGameCount;
	[SerializeField] TextMeshProUGUI PlayedGameCount;
	[SerializeField] TextMeshProUGUI FasterText;
	[SerializeField] TextMeshProUGUI QuickestText;

	[SerializeField] Button easyButton;
	[SerializeField] Button mediumButton;
	[SerializeField] Button hardButton;

	[SerializeField] Sprite modeSelectionBgSelected;
	[SerializeField] Sprite modeSelectionBgUnSelected;


	[SerializeField] private Animator PanelAnimator;
	[SerializeField] private Animator BackButtonAnimator;
	[SerializeField] private Animator PanelFadingAnimator;
	private List<string> panelExitAnimationCondition = new List<string>();



	public static ProfilePanelUI Instance;

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
	public static ProfilePanelUI ShowUI()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		if (Instance == null)
		{
			SoundManager.instance.Play_PANEL_INSTANTIATE_Sound();
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/ProfilePanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<ProfilePanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		SoundManager.instance.Play_PANEL_DESTROY_Sound();
		StartCoroutine(waitAndDestroy());
	}
	IEnumerator waitAndDestroy()
	{
		PanelAnimator.SetBool(panelExitAnimationCondition[UnityEngine.Random.Range(0, 3)], true);
		BackButtonAnimator.SetBool("IsOut", true);
		PanelFadingAnimator.SetBool("IsOut", true);
		yield return new WaitForSeconds(0.8f);
		Destroy(gameObject);

	}
	public void SetProfileData()
    {
		Name.text = GlobalData.Instance.UserName;
		LevelNumber.text = ((GlobalData.Instance.totalScore / 1000) + 1).ToString();
		FillImage.fillAmount = ((float)(GlobalData.Instance.totalScore % 1000f)) / 1000f;
		TargetText.text = (GlobalData.Instance.totalScore % 1000) + "/1000"; 
		HighestScore.text = GlobalData.Instance.highestScore.ToString();
		WonGameCount.text = GlobalData.Instance.totalGamesWon.ToString();
		PlayedGameCount.text = GlobalData.Instance.totalGamesPlayed.ToString();
	//	UpdateEasyButton();




	}
	public void SetUserName()
    {
		Name.text = GlobalData.Instance.UserName;
		MainPanelUI.Instance.UpdateNameUI();
	}
	public void OnClick_UpdateUserName()
    {
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		UpdateUserNamePanelUI.ShowUI();
		
    }
	public void OnClick_LinkAccountPanelUI()
    {
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		LinkUserAccountPanelUI.ShowUI();
		
    }
    public void UpdateEasyButton()
    {
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		easyButton.GetComponent<Image>().sprite = modeSelectionBgSelected;
		mediumButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;
		hardButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;

		float zScoreTime = (GlobalData.Instance.UserStats.EasyModeStats.Time.Median  - GlobalData.Instance.UserStats.EasyModeStats.Time.Median) / GlobalData.Instance.UserStats.EasyModeStats.Time.StandardDeviation;
	//	float percentFasterTime = Math.Round((1 - normalDistribution.CumulativeDistribution(zScoreTime)) * 100);


		FasterText.text = "YOU ARE FASTER THAN " + GlobalData.Instance.EasyFasterTimePercent + "% OF PEOPLE";
		QuickestText.text = "YOU USE LESS WORDS THAN " + GlobalData.Instance.EasyLessGuessPercent + "% OF PEOPLE";  
	}

    public void UpdateMediumButton()
    {
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		easyButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;
		mediumButton.GetComponent<Image>().sprite = modeSelectionBgSelected;
		hardButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;

		FasterText.text = "YOU ARE FASTER THAN " + GlobalData.Instance.MediumFasterTimePercent + "% OF PEOPLE";
		QuickestText.text = "YOU USE LESS WORDS THAN " + GlobalData.Instance.MediumLessGuessPercent + "% OF PEOPLE";
	}

	public void UpdateHardButton()
    {
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		easyButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;
		mediumButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;
		hardButton.GetComponent<Image>().sprite = modeSelectionBgSelected;

		FasterText.text = "YOU ARE FASTER THAN " + GlobalData.Instance.HardFasterTimePercent + "% OF PEOPLE";
		QuickestText.text = "YOU USE LESS WORDS THAN " + GlobalData.Instance.HardLessGuessPercent + "% OF PEOPLE";
	}
}