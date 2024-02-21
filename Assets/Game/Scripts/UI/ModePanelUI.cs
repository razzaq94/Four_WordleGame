using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModePanelUI : MonoBehaviour
{
	public static ModePanelUI Instance;
	[SerializeField] private Animator PanelAnimator;
	[SerializeField] private Animator BackButtonAnimator;
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

	public static ModePanelUI ShowUI()
	{
		if (Instance == null)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/ModePanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<ModePanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
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
	public void CallToSetGamemode(int gm)
	{
		GlobalData.Instance.SetGamemode(gm);
		GlobalData.Instance.Loadlevel("Game");
	}


	
	


}
