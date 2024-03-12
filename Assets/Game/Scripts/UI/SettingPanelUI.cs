using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class SettingPanelUI : MonoBehaviour
{
	public static SettingPanelUI Instance;
	[SerializeField] private GameObject MusicUI;
	[SerializeField] private GameObject SFXUI;
	[SerializeField] private GameObject VibrationUI;

	[SerializeField] private TextMeshProUGUI UserIdUI;
	[SerializeField] private Animator PanelAnimator;
	[SerializeField] private Animator BackButtonAnimator;
	[SerializeField] private Animator LogOutButtonAnimator;
	[SerializeField] private Animator PanelFadingAnimator;
	
	private List<string> panelExitAnimationCondition = new List<string>();
	private void Awake()
	{
		Instance = this;
	}
    private void Start()
    {
		UserIdUI.text = GlobalData.Instance.userId;
		panelExitAnimationCondition.Add("IsLeftOut");
		panelExitAnimationCondition.Add("IsRightOut");
		panelExitAnimationCondition.Add("IsBottomOut");
		
		SetButtonStatus();
	}
    public static SettingPanelUI ShowUI()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		if (Instance == null)
		{
			SoundManager.instance.Play_PANEL_INSTANTIATE_Sound();
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/SettingPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<SettingPanelUI>();
		}
		return Instance;
	}
	public void CopyOldUserId()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		GUIUtility.systemCopyBuffer = GlobalData.Instance.userId;
	}
	public void Logout()
    {
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		string newUsername = "Anonymous"+ Random.Range(0,100);
		InGameStorage.Instance.SetUserId("");
		InGameStorage.Instance.SetUserName(newUsername);
		GlobalData.Instance.userId = "";
		GlobalData.Instance.UserName = newUsername;

		Application.Quit();
    }
	public void OnBackPressed()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		SoundManager.instance.Play_PANEL_DESTROY_Sound();

		StartCoroutine(waitAndDestroy());
	}
	IEnumerator waitAndDestroy()
    {
		PanelAnimator.SetBool(panelExitAnimationCondition[Random.Range(0, 2)], true);
		BackButtonAnimator.SetBool("IsOut", true);
		LogOutButtonAnimator.SetBool("IsOut", true);
		PanelFadingAnimator.SetBool("IsOut", true);
		yield return new WaitForSeconds(0.8f);
		Destroy(gameObject);

    }

	void PlayExitAnim()
    {
		//int num = Random.Range(0, 3);

    }

	// Sound And Vibration Manager

	public void SetButtonStatus()
    {
		MusicUI.GetComponent<Slider>().value = ((float)PlayerPrefs.GetInt("MUSIC", 50)) / 100f;
		SFXUI.GetComponent<Slider>().value =  ((float)PlayerPrefs.GetInt("SFX", 50)) / 100f;
		VibrationUI.GetComponent<Slider>().value = ((float)PlayerPrefs.GetInt("VIBRATE", 500)) / 1000 ;
		SoundManager.instance.Set_SOUNDS_STATUS();

	}

	public void OnChangeMusic()
    {
		SoundManager.instance.Change_Music_Volume(MusicUI.GetComponent<Slider>().value);
    }
	public void OnChangeSFX()
	{
		SoundManager.instance.Change_SFX_Volume(SFXUI.GetComponent<Slider>().value);

	}
	public void OnChangeVibration()
	{
		SoundManager.instance.Change_VIBRATE_VOLUME(VibrationUI.GetComponent<Slider>().value);
	}

}
