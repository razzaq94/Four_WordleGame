using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MainPanelUI : MonoBehaviour
{
	[SerializeField] Image ScoreUI;
	[SerializeField] TextMeshProUGUI LevelUI;
	[SerializeField] TextMeshProUGUI UserNameUI;

	public static MainPanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}
	

	// Start is called before the first frame update
	void Start()
	{
		UpdateScoreUI();
		UpdateLevelUI();
		UpdateNameUI();
	//	APIManager.Instance.GetLeaderboardData();
	}
	public static MainPanelUI ShowUI()
	{
		if (Instance == null)
		{
			SoundManager.instance.Play_PANEL_INSTANTIATE_Sound();
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/MainPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<MainPanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		SoundManager.instance.Play_PANEL_DESTROY_Sound();
		Destroy(gameObject);
	}

	public void UpdateScoreUI()
	{
		ScoreUI.fillAmount = ((float)(GlobalData.Instance.totalScore % 1000f)) / 1000f;

	}
	public void UpdateLevelUI()
	{
//		LevelUI.text = (InGameStorage.Instance.GetLevel()).ToString();
		LevelUI.text = ((GlobalData.Instance.totalScore / 1000) +1).ToString();
	}
	public void UpdateNameUI()
	{
	//	UserNameUI.text = (InGameStorage.Instance.GetUserName()).ToString();
		UserNameUI.text = (GlobalData.Instance.UserName).ToString();
	}

	public void OnClick_PlayButton()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		ModePanelUI.ShowUI();

	}
	public void OnClick_SettingButton()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		SettingPanelUI.ShowUI();
	}
	public void OnClick_ShopButton()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		ShopPanelUI.ShowUI();
	}
	public void OnClick_LeaderboardButton()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		APIManager.Instance.GetLeaderboardData();
	}
	public void OnClick_ProfileButton()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		ProfilePanelUI.ShowUI();
		ProfilePanelUI.Instance.SetProfileData();
	}

}
