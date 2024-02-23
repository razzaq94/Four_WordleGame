using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitPanelUI : MonoBehaviour
{
	[SerializeField] private Animator PanelAnimator;
	[SerializeField] private Animator PanelFadingAnimator;
	private List<string> panelExitAnimationCondition = new List<string>();

	public static QuitPanelUI Instance;

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
	public static QuitPanelUI ShowUI()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		if (Instance == null)
		{
			SoundManager.instance.Play_PANEL_INSTANTIATE_Sound();
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/QuitPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<QuitPanelUI>();
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
		PanelFadingAnimator.SetBool("IsOut", true);
		yield return new WaitForSeconds(0.8f);
		Destroy(gameObject);

	}
	public void CallToQuitGame()
    {
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		BoosterManager.Instance.UpdateAllBoosterCount(-1);
		GameManager.Instance.BackToMainMenu();
	}

}
