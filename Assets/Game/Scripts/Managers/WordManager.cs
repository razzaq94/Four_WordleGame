using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
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
    public Color revealedColor;
    public Color partiallyRevealedColor;
    public Color concealedColor;
    public Color originalBgColor;
    public List<Color> colors;
    public string WordToGuess = "ABROAD";
    public string WordDefinition = null;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
      //  GetWord();
        originalBgColor = Color.white;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextColNumber()
    {
        if ((CurrentColNumber ) != ((int)GlobalData.Instance.gameMode) -1)
        {
            CurrentColNumber++;
            CanWrite = true;
            KeyboardManager.Instance.UpdateEnterButton(false);

        }
        else if (CurrentColNumber == (((int)GlobalData.Instance.gameMode) - 1))
        {
            CanWrite = false;
            KeyboardManager.Instance.UpdateEnterButton(true);
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
        if (CurrentRowNumber < 5)
        {
            CurrentRowNumber++;
            CurrentColNumber = 0;
            CanWrite = true;
            KeyboardManager.Instance.UpdateEnterButton(false);
        }

    }
    private void countCorrectLetterAndShow(string WrdDefinition)
    {
        int letterCount=0;
        if (WrdDefinition != null)
        {
            List<string> alreadyCheckedLetters = new List<string>();
            // set background Yellow or Red
            GameObject rowPanel = UIManager.Instance.ContentHolder.transform.GetChild(0).gameObject;
            for (int i = 0; i < (int)GlobalData.Instance.gameMode; i++)
            {
                string letter = rowPanel.transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.ToString();
                
                if (WordToGuess.Contains(letter) && !(alreadyCheckedLetters.Contains(letter)))
                {                 
                    alreadyCheckedLetters.Add(letter);
                    print("1");
                    for (int j = 0; j < WordToGuess.Length; j++)
                    {
                        print("2");
                        if(WordToGuess[j].ToString() == letter)
                        {
                            letterCount++;
                            print("letter count : " + letterCount);
                        }
                    }
                }
                

            }

            rowPanel.transform.GetChild((int)GlobalData.Instance.gameMode).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = letterCount.ToString();
        }
    }

    public void CheckWord(string WrdDefinition)
    {
        if (WrdDefinition != null)
        {
            List<string> alreadyCheckedLetters = new List<string>();
            // set background Yellow or Red
            GameObject rowPanel = UIManager.Instance.GamePanel.transform.Find("GridPanel").transform.GetChild(CurrentRowNumber).gameObject;
            for (int i = 0; i < (int)GlobalData.Instance.gameMode; i++)
            {
                string letter = rowPanel.transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.ToString();
                if (WordToGuess.Contains(letter) && !(alreadyCheckedLetters.Contains(letter)))
                {
                    rowPanel.transform.GetChild(i).GetComponent<Image>().color = partiallyRevealedColor;
                    KeyboardManager.Instance.ChangeKeyColor(letter, partiallyRevealedColor);
                    alreadyCheckedLetters.Add(letter);
                }
                else
                {
                    rowPanel.transform.GetChild(i).GetComponent<Image>().color = concealedColor;
                    KeyboardManager.Instance.ChangeKeyColor(letter, concealedColor);

                }

            }
            int gCount = 0;

            //  Green
            for (int i = 0; i < (int)GlobalData.Instance.gameMode; i++)
            {
                string letter = rowPanel.transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.ToString();         
                if (letter == WordToGuess[i].ToString())
                {
                    rowPanel.transform.GetChild(i).GetComponent<Image>().color = revealedColor;
                    KeyboardManager.Instance.ChangeKeyColor(letter, revealedColor);
                    gCount++;

                }

            }
            if (gCount == (int)GlobalData.Instance.gameMode)
            {
                print("Definition : " + WordDefinition);
                TimerUp.Instance.StopTimer(true);
                GameOverPanelUI.ShowUI();
                GameOverPanelUI.Instance.SetText("Congratulations !!!", WordToGuess, WordDefinition);
            }
            else
            {
                if (CurrentRowNumber < 5)
                {
                    SetNextRowNumber();
                }
                else
                {
                    GameOverPanelUI.ShowUI();
                    GameOverPanelUI.Instance.SetText("BetterLuck next time !!!", WordToGuess, WordDefinition);

                }
            }
        }
        else
        {

        }

    }

    public string CheckWordOnline()
    {
        KeyboardManager.Instance.UpdateEnterButton(false);
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
                        //CheckWord(definition);
                        countCorrectLetterAndShow(definition);

                        if (wordToCheck == WordToGuess)
                        {
                            print("Definition : " + WordDefinition);
                            TimerUp.Instance.StopTimer(true);
                            GameOverPanelUI.ShowUI();
                            GameOverPanelUI.Instance.SetText("Congratulations !!!", WordToGuess, WordDefinition);
                        }
                        else
                        {
                            UIManager.Instance.CreateEmptyRow();
                        }
                    }
                    else
                    {
                        UIManager.Instance.ClearCurrentRow();
                        WarningPanelUI.ShowUI();
                        WarningPanelUI.Instance.CallToQuitPanelAutomatically();
                        print("This word does not exist....");
                    }
                }
            },
            (error, type) =>
            {
                print(error);
                UIManager.Instance.ClearCurrentRow();
                WarningPanelUI.ShowUI();
                    WarningPanelUI.Instance.CallToQuitPanelAutomatically();
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
        KeyboardManager.Instance.UpdateEnterButton(true);
        return definition;

    }


    public void GetWord()
    {    
        WordToGuess =   ReadString();
    }
    public static string ReadString()
    {
             string str = Resources.Load("WordList").ToString();
      

        string[] rowOfIndex = str.Split('\n');
        GlobalData.Instance.WordList.Clear();
        for (int i = 0; i < rowOfIndex.Length; i++)
        {
            if (rowOfIndex[i].Length == (int)GlobalData.Instance.gameMode+1 )
            {
                //   print(rowOfIndex[i]);
                GlobalData.Instance.WordList.Add(rowOfIndex[i]);
            }
        }

        int index = Random.Range(0, GlobalData.Instance.WordList.Count);


        return (GlobalData.Instance.WordList[index].ToUpper()).TrimEnd();

        
    }

    public void getWordDefinition()
    {
        print("Word to guess : " + WordToGuess);
        NetworkAPIManager.Instance.CheckWordOnline(WordToGuess,
            (resposne) =>
            {
                JSONArray data = JSON.Parse(resposne).AsArray;

                if (data.Count > 0)
                {
                    if (data[0]["word"].Value.ToLower() == WordToGuess.ToLower())
                    {
                        WordDefinition = "Definition : " + data[0]["meanings"][0]["definitions"][0]["definition"].Value;


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
    public int GetCurrentWordLength()
    {
        int row = 0;
        int length = 0;
        for (int i = 0; i < (int)GlobalData.Instance.gameMode; i++)
        {
            string letterOnCurrentCell = UIManager.Instance.ContentHolder.transform.GetChild(row).transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            if (letterOnCurrentCell != "" )
            {
                length++;
            }
            else
            {
                break;
            }
        }
        return length;
    }
    public void Reveal()
    {
        List<string> letterList = getLetterList();
        for(int i =0;i<letterList.Count;i++)
        {

        }
    }
    private List<string> getLetterList()
    {
        List<string> letterList = new List<string>();
        for(int i=0;i< WordToGuess.Length;i++)
        {
            letterList.Add(WordToGuess[i].ToString());
        }
        return letterList;
    }

    public IEnumerator test()
    {
        yield return new WaitForSeconds(5f);
        SetNextRowNumber();
    }



}