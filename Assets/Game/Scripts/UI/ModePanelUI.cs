using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModePanelUI : MonoBehaviour
{
	public static ModePanelUI Instance;

	private void Awake()
	{
		Instance = this;
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
		Destroy(gameObject);

	}
	public void CallToSetGamemode(int gm)
	{
		GlobalData.Instance.SetGamemode(gm);
		GlobalData.Instance.Loadlevel("Game");
	}


	
	


}
