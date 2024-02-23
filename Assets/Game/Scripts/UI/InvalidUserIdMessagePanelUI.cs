using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InvalidUserIdMessagePanelUI : MonoBehaviour
{
	public static InvalidUserIdMessagePanelUI Instance;
	[SerializeField] private Animator PanelAnimator;
	[SerializeField] private Animator PanelFadingAnimator;
	private List<string> panelExitAnimationCondition = new List<string>();

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
	public static InvalidUserIdMessagePanelUI ShowUI()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		if (Instance == null)
		{
			SoundManager.instance.Play_PANEL_INSTANTIATE_Sound();
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/InvalidUserIdMessagePanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<InvalidUserIdMessagePanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		SoundManager.instance.Play_PET_DESRTOY_Sound();
		StartCoroutine(waitAndDestroy());
	}
	IEnumerator waitAndDestroy()
	{
		PanelAnimator.SetBool(panelExitAnimationCondition[Random.Range(0, 3)], true);
		PanelFadingAnimator.SetBool("IsOut", true);
		yield return new WaitForSeconds(0.8f);
		Destroy(gameObject);

	}
	public void QuitGame()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		Application.Quit();
		//SceneManager.LoadScene("SplashScene");
		OnBackPressed();
	}
	public void RestartAsGuest()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		GlobalData.Instance.IsRestart = false;
		Destroy(NetworkAPIManager.Instance.gameObject);
		SceneManager.LoadScene("SplashScene");
		//GlobalData.Instance.IsRestart = false;
		//APIManager.Instance.LogIn();
		//OnBackPressed();

	}
}
