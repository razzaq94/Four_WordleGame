using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfilePanelUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI Name;
	[SerializeField] TextMeshProUGUI LevelNumber;
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





	public static ProfilePanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static ProfilePanelUI ShowUI()
	{
		if (Instance == null)
		{
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
		Destroy(gameObject);
	}

	public void SetProfileData()
    {
		Name.text = GlobalData.Instance.UserName;
		LevelNumber.text = GlobalData.Instance.level.ToString();
		HighestScore.text = GlobalData.Instance.highestScore.ToString();
		WonGameCount.text = GlobalData.Instance.totalGamesWon.ToString();
		PlayedGameCount.text = GlobalData.Instance.totalGamesPlayed.ToString();
		UpdateEasyButton();




	}
	public void SetUserName()
    {
		Name.text = GlobalData.Instance.UserName;
	}
	public void OnClick_UpdateUserName()
    {
		UpdateUserNamePanelUI.ShowUI();
		
    }
	public void OnClick_LinkAccountPanelUI()
    {
		LinkUserAccountPanelUI.ShowUI();
		
    }
    public void UpdateEasyButton()
    {
		easyButton.GetComponent<Image>().sprite = modeSelectionBgSelected;
		mediumButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;
		hardButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;
		
		FasterText.text = "YOU ARE FASTER THAN " + 30 + "% OF PEOPLE";
		QuickestText.text = "YOU USE LESS WORDS THAN " + 10 + "% OF PEOPLE";  
	}

    public void UpdateMediumButton()
    {
		easyButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;
		mediumButton.GetComponent<Image>().sprite = modeSelectionBgSelected;
		hardButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;

		FasterText.text = "YOU ARE FASTER THAN " + 32 + "% OF PEOPLE";
		QuickestText.text = "YOU USE LESS WORDS THAN " + 12 + "% OF PEOPLE";  
	}

    public void UpdateHardButton()
    {
		easyButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;
		mediumButton.GetComponent<Image>().sprite = modeSelectionBgUnSelected;
		hardButton.GetComponent<Image>().sprite = modeSelectionBgSelected;

		FasterText.text = "YOU ARE FASTER THAN " + 33 + "% OF PEOPLE";
		QuickestText.text = "YOU USE LESS WORDS THAN " + 13 + "% OF PEOPLE";  
	}
}