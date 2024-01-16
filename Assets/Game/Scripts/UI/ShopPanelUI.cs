using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanelUI : MonoBehaviour
{
	public static ShopPanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static ShopPanelUI ShowUI()
	{
		if (Instance == null)
		{
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
		Destroy(gameObject);
	}

}