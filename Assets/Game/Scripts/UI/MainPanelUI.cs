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
	}
	public static MainPanelUI ShowUI()
	{
		if (Instance == null)
		{
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
		Destroy(gameObject);
	}

	public void UpdateScoreUI()
	{
		//ScoreUI.fillAmount = InGameStorage.Instance.GetScore() / 100;
		ScoreUI.fillAmount = GlobalData.Instance.totalScore / 100;
	}
	public void UpdateLevelUI()
	{
//		LevelUI.text = (InGameStorage.Instance.GetLevel()).ToString();
		LevelUI.text = (GlobalData.Instance.level).ToString();
	}
	public void UpdateNameUI()
	{
	//	UserNameUI.text = (InGameStorage.Instance.GetUserName()).ToString();
		UserNameUI.text = (GlobalData.Instance.UserName).ToString();
	}

	public void OnClick_PlayButton()
	{
		ModePanelUI.ShowUI();

	}
	public void OnClick_SettingButton()
	{
		SettingPanelUI.ShowUI();
	}
	public void OnClick_ShopButton()
	{
		ShopPanelUI.ShowUI();
	}
	public void OnClick_LeaderboardButton()
	{
		LeaderboardPanelUI.ShowUI();
	}
	public void OnClick_ProfileButton()
	{
		LeaderboardPanelUI.ShowUI();
	}
}
