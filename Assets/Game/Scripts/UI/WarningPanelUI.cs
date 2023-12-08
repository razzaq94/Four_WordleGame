using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPanelUI : MonoBehaviour
{
	public static WarningPanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static WarningPanelUI ShowUI()
	{
		if (Instance == null)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/WarningPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<WarningPanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		Destroy(gameObject);
	}
	public void CallToQuitPanelAutomatically()
	{
		StartCoroutine(waitAndQuitPanel());
	}
	IEnumerator waitAndQuitPanel()
    {
		yield return new WaitForSeconds(2f);
		OnBackPressed();
    }
}
