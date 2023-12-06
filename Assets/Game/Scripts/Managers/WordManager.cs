using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.Networking;

//using UnityEngine.UI.Extensions;
using SimpleJSON;


public class WordManager : MonoBehaviour
{
    public static WordManager Instance;
    public int CurrentRowNumber = 0;
    public int CurrentColNumber = 0;
    public bool CanWrite = true;
    public string WordToGuess = "ABROAD";

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextColNumber()
    {
        if ((CurrentColNumber + 1) != (int)GlobalData.Instance.gameMode)
        {
            CurrentColNumber++;
        }
        else
        {
            CanWrite = false;
            //CheckWord();
//            StartCoroutine(test());
        }

    }
    public void SetPreviousColNumber()
    {
        if(CurrentColNumber > 0)
        {
            CurrentColNumber--;
        }
    }
        public void SetNextRowNumber()
    {
        if(CurrentRowNumber < 5)
        {
            CurrentRowNumber++;
            CurrentColNumber = 0;
            CanWrite = true;
        }

    }

    public void CheckWord(string WordDefinition)
    {
        if (WordDefinition!= null)
        {
            int gCount = 0;
            List<string> alreadyCheckedLetters = new List<string>();
            // set background Yellow or Red
            GameObject rowPanel = UIManager.Instance.GamePanel.transform.Find("GridPanel").transform.GetChild(CurrentRowNumber).gameObject;
            for (int i = 0; i < (int)GlobalData.Instance.gameMode; i++)
            {
                string letter = rowPanel.transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.ToString();
                if (WordToGuess.Contains(letter) && !(alreadyCheckedLetters.Contains(letter)))
                {
                    rowPanel.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
                    alreadyCheckedLetters.Add(letter);
                }
                else
                {
                    rowPanel.transform.GetChild(i).GetComponent<Image>().color = Color.grey;
                }

            }

            //  Green
            for (int i = 0; i < (int)GlobalData.Instance.gameMode; i++)
            {
                if (rowPanel.transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.ToString() == WordToGuess[i].ToString())
                {
                    rowPanel.transform.GetChild(i).GetComponent<Image>().color = Color.green;
                }

            }
            if (gCount == (int)GlobalData.Instance.gameMode)
            {
                print("Definition : " + WordDefinition);
                // Game over
            }
            else
            {
                SetNextRowNumber();
            }
        }
        else
        {

        }

    }

    public string CheckWordOnline()
    {
        string wordToCheck = UIManager.Instance.GetToCheckWord();
        string definition = null;
        NetworkAPIManager.Instance.CheckWordOnline(wordToCheck,
            (resposne) =>
            {
                JSONArray data = JSON.Parse(resposne).AsArray;

                if (data.Count > 0)
                {
                    if (data[0]["word"].Value.ToLower() == wordToCheck.ToLower())
                    {

                        definition = "Definition : " + data[0]["meanings"][0]["definitions"][0]["definition"].Value;
                        CheckWord(definition);

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

        return definition;

    }



    public IEnumerator test()
    {
        yield return new WaitForSeconds(5f);
        SetNextRowNumber();
    }



}