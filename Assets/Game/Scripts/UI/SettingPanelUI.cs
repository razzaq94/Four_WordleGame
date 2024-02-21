using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingPanelUI : MonoBehaviour
{
	public static SettingPanelUI Instance;
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
		UserIdUI.text = "USER_ID:"+GlobalData.Instance.userId;
		panelExitAnimationCondition.Add("IsLeftOut");
		panelExitAnimationCondition.Add("IsRightOut");
		panelExitAnimationCondition.Add("IsBottomOut");

	}
    public static SettingPanelUI ShowUI()
	{
		if (Instance == null)
		{
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
		GUIUtility.systemCopyBuffer = GlobalData.Instance.userId;
	}
	public void Logout()
    {
		string newUsername = "Anonymous"+ Random.Range(0,100);
		InGameStorage.Instance.SetUserId("");
		InGameStorage.Instance.SetUserName(newUsername);
		GlobalData.Instance.userId = "";
		GlobalData.Instance.UserName = newUsername;

		Application.Quit();
    }
	public void OnBackPressed()
	{
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
	
	
}
