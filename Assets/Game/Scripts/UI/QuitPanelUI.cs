using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitPanelUI : MonoBehaviour
{
	public static QuitPanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static QuitPanelUI ShowUI()
	{
		if (Instance == null)
		{
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
		Destroy(gameObject);
	}
	public void CallToQuitGame()
    {
		GameManager.Instance.BackToMainMenu();
	}

}
