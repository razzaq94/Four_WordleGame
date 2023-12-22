using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;
public class LogInPanelUI : MonoBehaviour
{
	public static LogInPanelUI Instance;
	string userName;
	string password;
    private void Awake()
    {
		Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
		userName = null;
		password = null;

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

	public void SetUserName(TextMeshProUGUI uName)
    {
		userName = uName.text;
		print("uNAME : " + uName.text);
    }
	public void SetPassword(TextMeshProUGUI pswd)
    {
		password = pswd.text;  
    }
    public void OnClick_LogIn()
    {
        if (InGameStorage.Instance.GetName() != null)
        {
            LogIn();
        }
    }
	public void LogIn()
    {
        string WordToGuess = "Care";
            NetworkAPIManager.Instance.LogIn(WordToGuess,
                (resposne) =>
                {
                    JSONArray data = JSON.Parse(resposne).AsArray;

                    if (data.Count > 0)
                    {
                        if (data[0]["word"].Value.ToLower() == WordToGuess.ToLower())
                        {
                           string WordDefinition = "Definition : " + data[0]["meanings"][0]["definitions"][0]["definition"].Value;


                            print(WordDefinition);

                        }
                        else
                        {
                            print("This word does not exist....");

                        }
                    }
                },
                (error, type) =>
                {
                    print(error);

                    if (type == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
                    {

                    //....... For testing only 
                    //  GameManager.Instance.ResetGame();
                    //..........

                    //            string title = "Network Error";
                    //         string definition = "No internet connection. Please try again.";
                    //             ErrorPanelUI.ShowUI();
                    //       ErrorPanelUI.instance.setText(title, definition);
                }
                    else
                    {
                    //     if (LoadingPanelUI.instance != null)
                    //   {
                    //     LoadingPanelUI.instance.OnBackPressed();
                    //}

                    //    UIManager.Instance.UpdatePlayerGhostUI(false, CharList, CharacterColorList);
                }
                });
        
    }

	
}
