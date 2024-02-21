using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErrorMessagePanelUI : MonoBehaviour
{
	public static ErrorMessagePanelUI Instance;
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

	public static ErrorMessagePanelUI ShowUI()
	{
		if (Instance == null)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/ErrorMessagePanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<ErrorMessagePanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
			StartCoroutine(waitAndDestroy());
	}
	IEnumerator waitAndDestroy()
	{
		PanelAnimator.SetBool(panelExitAnimationCondition[Random.Range(0, 3)], true);
		PanelFadingAnimator.SetBool("IsOut", true);
		yield return new WaitForSeconds(0.8f);
		Destroy(gameObject);

	}
	public void RestartGame()
    {
		Application.Quit();
		//SceneManager.LoadScene("SplashScene");
		OnBackPressed();
    }
}
