using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class LinkUserAccountPanelUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI currentUserId;
	[SerializeField] TextMeshProUGUI newUserId;
	
	public static LinkUserAccountPanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}
    private void Start()
    {
		currentUserId.text = GlobalData.Instance.userId;
    }

    public static LinkUserAccountPanelUI ShowUI()
	{
		if (Instance == null)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/LinkUserAccountPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<LinkUserAccountPanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		Destroy(gameObject);
	}
	public void LinkUserAccount()
	{
		GlobalData.Instance.IsRestart = true;
		GlobalData.Instance.userId = newUserId.text;
		Destroy(NetworkAPIManager.Instance.gameObject);
		SceneManager.LoadScene("SplashScene");
		OnBackPressed();
	}
	public void CopyOldUserId()
    {
		string oldUserID = currentUserId.text.ToString();
		print("oldUserID length : " + oldUserID);
		GUIUtility.systemCopyBuffer = oldUserID;  

	}

}