using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class GameOverPanelUI : MonoBehaviour
{
	//[SerializeField] TextMeshProUGUI resultMessage;
	[SerializeField] TextMeshProUGUI word;
	[SerializeField] TextMeshProUGUI definition;
	public static GameOverPanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}

	public static GameOverPanelUI ShowUI()
	{
		if (Instance == null)
		{
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/GameOverPanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<GameOverPanelUI>();
		}
		return Instance;
	}
	public void SetText(string rMessage, string wrd, string def )
    {
		//resultMessage.text = rMessage;
		word.text = wrd;
		definition.text = def;
    }
	public void OnBackPressed()
	{
		Destroy(gameObject);
	}
	public void CallToQuitGame()
	{
		GameManager.Instance.BackToMainMenu();
	}
	public void CallToPlayAgainGame()
	{
		GameManager.Instance.ReloadLevel();
	}
}
