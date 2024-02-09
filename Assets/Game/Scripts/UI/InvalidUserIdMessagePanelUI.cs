using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InvalidUserIdMessagePanelUI : MonoBehaviour
{
	public static InvalidUserIdMessagePanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static InvalidUserIdMessagePanelUI ShowUI()
	{
		if (Instance == null)
		{
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
		Destroy(gameObject);
	}
	public void QuitGame()
	{
		Application.Quit();
		//SceneManager.LoadScene("SplashScene");
		OnBackPressed();
	}
	public void RestartAsGuest()
	{
		GlobalData.Instance.IsRestart = false;
		Destroy(NetworkAPIManager.Instance.gameObject);
		SceneManager.LoadScene("SplashScene");
		//GlobalData.Instance.IsRestart = false;
		//APIManager.Instance.LogIn();
		//OnBackPressed();

	}
}
