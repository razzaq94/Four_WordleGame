using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateUserNamePanelUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI newUserName;
	public static UpdateUserNamePanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static UpdateUserNamePanelUI ShowUI()
	{
		if (Instance == null)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/UpdateUserNamePanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<UpdateUserNamePanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		Destroy(gameObject);
	}
	public void UpdateUserName()
    {
		APIManager.Instance.UpdateUserName(newUserName.text.ToString());
		OnBackPressed();
	}

}
