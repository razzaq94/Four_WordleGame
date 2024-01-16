using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePanelUI : MonoBehaviour
{
	public static ProfilePanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static ProfilePanelUI ShowUI()
	{
		if (Instance == null)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/ProfilePanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<ProfilePanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		Destroy(gameObject);
	}

}