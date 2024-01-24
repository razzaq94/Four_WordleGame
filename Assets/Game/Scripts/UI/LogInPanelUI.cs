using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LogInPanelUI : MonoBehaviour
{
	public static LogInPanelUI Instance;
	[SerializeField] TextMeshProUGUI UserNameUI;

	string userName;
	private void Awake()
	{
		Instance = this;
	}
	// Start is called before the first frame update
	void Start()
	{
		userName = null;
	
	}

	public static LogInPanelUI ShowUI()
	{
		if (Instance == null)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/LogInPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<LogInPanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		Destroy(gameObject);
	}

	public void SetUserName()
	{
		userName = UserNameUI.text;
		InGameStorage.Instance.SetUserName(userName);
	}
	public void OnClick_LogIn()
	{
		SetUserName();
		APIManager.Instance.LogIn();
		LogInManager.Instance.OpenGameScene();
	}



	
}
