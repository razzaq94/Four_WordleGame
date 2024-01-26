using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using System;

public class LeaderboardPanelUI : MonoBehaviour
{
	[SerializeField] GameObject First;
	[SerializeField] GameObject Second;
	[SerializeField] GameObject Third;
	[SerializeField] GameObject CurrentUser;
	[SerializeField] GameObject Item_PF;
	[SerializeField] Transform Parent;



	public static LeaderboardPanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static LeaderboardPanelUI ShowUI()
	{
		if (Instance == null)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/LeaderboardPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<LeaderboardPanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		Destroy(gameObject);
	}
	//public void SetLeaderboardData(string [] arr)
	public void SetLeaderboardData(JSONNode node)
    {
		//for(int i=0;i<arr.Length;i++)
		for(int i=0;i< node["users"].Count;i++)
        {
			if(i==0)
            {
				First.SetActive(true);
				First.transform.Find("NameBgImage").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["username"];
				First.transform.Find("Score Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["total_obtained_score"];

			}
			else if(i==1)
            {
				Second.SetActive(true);
				Second.transform.Find("NameBgImage").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["username"];
				Second.transform.Find("Score Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["total_obtained_score"];

			}
			else if(i==2)
            {
				Third.SetActive(true);
				Third.transform.Find("NameBgImage").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["username"];
				Third.transform.Find("Score Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["total_obtained_score"];


			}
			else
            {
				GameObject go = Instantiate(Item_PF, Parent);
				go.transform.SetAsLastSibling();

				go.transform.Find("NameBgImage").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["username"];
				go.transform.Find("Score Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["total_obtained_score"];
				go.transform.Find("SrText").GetComponent<TextMeshProUGUI>().text = (i+1).ToString();


			}
			if (GlobalData.Instance.userId == node["users"][i]["_id"].Value.ToString())
            {
				CurrentUser.transform.Find("NameBgImage").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["username"];
				CurrentUser.transform.Find("Score Text (TMP)").GetComponent<TextMeshProUGUI>().text = node["users"][i]["total_obtained_score"];
				CurrentUser.transform.Find("SrText").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();

			}
		}
    }

}