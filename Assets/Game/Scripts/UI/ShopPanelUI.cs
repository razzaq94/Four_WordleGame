using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelUI : MonoBehaviour
{
	[SerializeField] Button RestorePurchaseButton;
	[SerializeField] Button PurchasePremiumButton;
	[SerializeField] TextMeshProUGUI PurchasePremiumText;
	[SerializeField] private Animator PanelAnimator;
	[SerializeField] private Animator BackButtonAnimator;
	[SerializeField] private Animator PanelFadingAnimator;
	private List<string> panelExitAnimationCondition = new List<string>();

	public static ShopPanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		#if UNITY_ANDROID
		RestorePurchaseButton.gameObject.SetActive(false);
	    #endif
        if (GlobalData.Instance.isPremium ==  true)
        {
			PurchasePremiumButton.interactable = false;
			PurchasePremiumText.text = "PURCHASED";
        }
		panelExitAnimationCondition.Add("IsLeftOut");
		panelExitAnimationCondition.Add("IsRightOut");
		panelExitAnimationCondition.Add("IsBottomOut");

	}
	public static ShopPanelUI ShowUI()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		if (Instance == null)
		{
			SoundManager.instance.Play_PANEL_INSTANTIATE_Sound();
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/ShopPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<ShopPanelUI>();
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

	public void PurchaseEliminateBooster()
    {
	//	GlobalData.Instance.EliminateBoosterCount += 10;
		GlobalData.Instance.UpdateBoosterCount("Eliminate", 10);

	}
	public void PurchaseRevealBooster()
    {
		//GlobalData.Instance.RevealBoosterCount += 5;
		GlobalData.Instance.UpdateBoosterCount("Reveal", 10);

	}
	public void PurchasePremiumBooster()
    {
		GlobalData.Instance.isPremium = true;
		PurchasePremiumButton.interactable = false;
        PurchasePremiumText.text = "PURCHASED";
		InGameStorage.Instance.SetPremiumStatus(true);
		//APIManager.Instance.UpdatePremiumStatus();



    }

}