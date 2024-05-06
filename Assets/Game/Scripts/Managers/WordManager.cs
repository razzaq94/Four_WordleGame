using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Networking;

//using UnityEngine.UI.Extensions;
using SimpleJSON;
using Unity.VisualScripting;
using System;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance;
    public int CurrentRowNumber = 0;
    public int CurrentColNumber = 0;
    public bool CanWrite = true;
    public Color revealedColor;
    public Color concealedColor;
    public Color originalBgColor;
    public Color counterBgColor;
    public List<Color> colors;
    public string WordToGuess = "ABROAD";
    public string WordDefinition = null;
    public List<Tuple<string, int>> guesses = new List<Tuple<string, int>>();
    public List<Reveal> revealList = new List<Reveal>();
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //  GetWord();
        //  originalBgColor = Color.white;
    //    MyReadString();
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
        string newWord = "";
        if (WrdDefinition != null)
        {
            List<string> alreadyCheckedLetters = new List<string>();
            // set background Yellow or Red
            GameObject rowPanel = UIManager.Instance.TopRow.transform.GetChild(0).gameObject;
            for (int i = 0; i < (int)GlobalData.Instance.gameMode; i++)
            {
                string letter = rowPanel.transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.ToString();
                newWord += letter;
                if (WordToGuess.Contains(letter))// && !(alreadyCheckedLetters.Contains(letter)))
                {                 
                    alreadyCheckedLetters.Add(letter);
                 //   print("1");
                    for (int j = 0; j < WordToGuess.Length; j++)
                    {
                   //     print("2");
                        if(WordToGuess[j].ToString() == letter)
                        {
                            letterCount++;
                          //  print("letter count : " + letterCount);
                        }
                    }
                }
                

            }

            rowPanel.transform.GetChild((int)GlobalData.Instance.gameMode).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = letterCount.ToString();
            Tuple<string, int> guess = new Tuple<string, int>(newWord, letterCount);

            guesses.Add(guess);
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
                    rowPanel.transform.GetChild(i).GetComponent<Image>().color = concealedColor;
                    KeyboardManager.Instance.ChangeKeyColor(letter, concealedColor);
                    alreadyCheckedLetters.Add(letter);
                }
                else
                {
                    rowPanel.transform.GetChild(i).GetComponent<Image>().color = originalBgColor;
                    KeyboardManager.Instance.ChangeKeyColor(letter, originalBgColor);

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
               // TimerUp.Instance.StopTimer(true);
                Timer.Instance.StopTimer(true);
                APIManager.Instance.SaveGameData(true);
              //  APIManager.Instance.UpdateUserData();

                //    GameOverPanelUI.ShowUI();
                //    GameOverPanelUI.Instance.SetText("Congratulations !!!", WordToGuess, WordDefinition);
            }
            else
            {
                if (CurrentRowNumber < 5)
                {
                    SetNextRowNumber();
                }
                else
                {
                    APIManager.Instance.SaveGameData(false);
                 //   APIManager.Instance.UpdateUserData();

                    //                    GameOverPanelUI.ShowUI();
                    //                  GameOverPanelUI.Instance.SetText("BetterLuck next time !!!", WordToGuess, WordDefinition);

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
        CanWrite = false;
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
                           // TimerUp.Instance.StopTimer(true);
                            Timer.Instance.StopTimer(true);
                            APIManager.Instance.SaveGameData(true);
                         //   APIManager.Instance.UpdateUserData();


                            // GameOverPanelUI.ShowUI();
                            // GameOverPanelUI.Instance.SetText("Congratulations !!!", WordToGuess, WordDefinition);
                        }
                        else
                        {
                            UIManager.Instance.CreateEmptyRow();
                            // BoosterManager.Instance.AutoColorLatestRow();
                            if (BoosterManager.Instance.isAutoColor)
                            {
                                BoosterManager.Instance.AutoColor();
                            }
                            CanWrite = true;

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
    
    public string CheckWordOffline()
    {
        KeyboardManager.Instance.UpdateEnterButton(false);
        string wordToCheck = UIManager.Instance.GetToCheckWord();
        string definition = null;
        CanWrite = false;

        if (wordToCheck.Length > 0)
        {
            if ((WholeDictionary.Exists(x => x.word.Equals(wordToCheck))))
            {

                //   definition = "Definition : " + data[0]["meanings"][0]["definitions"][0]["definition"].Value;
                //CheckWord(definition);
                countCorrectLetterAndShow("exists");

                if (wordToCheck == WordToGuess)
                {
                    print("Definition : " + WordDefinition);
                   // ScoreManager.Instance.CalculateScore(UIManager.Instance.ContentHolder.transform.childCount, (int)TimerUp.Instance.CurrentTime);
                    ScoreManager.Instance.CalculateScore(UIManager.Instance.ContentHolder.transform.childCount+1, ((int)Timer.Instance.MaxTime - (int)Timer.Instance.CurrentTime),true);
                                      
                    //TimerUp.Instance.StopTimer(true);
                    Timer.Instance.StopTimer(true);
                    APIManager.Instance.SaveGameData(true);
                  //  APIManager.Instance.UpdateUserData();
                    ///  GameOverPanelUI.ShowUI();
                    // GameOverPanelUI.Instance.SetText("Congratulations !!!", WordToGuess, WordDefinition);
                }
                else
                {
                    SoundManager.instance.Play_VALID_WORD_Sound();
                    UIManager.Instance.CreateEmptyRow();
                    UIManager.Instance.CopyLetters();
                    UIManager.Instance.ClearCurrentRow();
                    // BoosterManager.Instance.AutoColorLatestRow();
                    if (BoosterManager.Instance.isAutoColor)
                    {
                        // BoosterManager.Instance.AutoColor();
                        AutoColorBooster.Instance.AutoColor();
                    }
                    CanWrite = true;

                }
            }
            else
            {
                SoundManager.instance.Play_INVALID_WORD_Sound();
                SoundManager.instance.Play_BUTTON_Vibrate();

                WriteMiscellaneousWord(wordToCheck);
                UIManager.Instance.ClearCurrentRow();
                WarningPanelUI.ShowUI();
                WarningPanelUI.Instance.CallToQuitPanelAutomatically();
                print("This word does not exist....");


            }
        }
           
        KeyboardManager.Instance.UpdateEnterButton(true);
        return definition;

    }

    private bool isUniqueLettersString(string str)
    {
        str = str.TrimEnd();
        int limit = str.Length;
     //   print("Limit : " + limit);
        for (int i=0; i<limit-1 && str.Length>1; i++)
        {
            string s = str[0].ToString();
            str =  str.Substring(1);
       //     print("s : " + s + "str : " + str);
            if((str != null) && str.Contains(s))
            {
                return false;
            }

        }



        return true;
    }
    public void GetWord()
    {    
        ReadWholeDictionary();
        WordToGuess =   ReadString();
        print("WordToGuess : " + WordToGuess);
    }
     public static string MyReadString()
      {
          char fileName = 'A';
          for (int count = 0; count < 1; count++)
          {
                  string str = Resources.Load("MyWordList").ToString();     

             // string str = Resources.Load(fileName.ToString()).ToString();
              string[] rowOfIndex = str.Split('\n');
              GlobalData.Instance.TempWordList.Clear();
              for (int i = 0; i < rowOfIndex.Length; i++)
              {
                print(rowOfIndex[i].Split('|')[0] + "Length : " + rowOfIndex[i].Split('|')[0].Length);
                   if (rowOfIndex[i].Split('|')[0].Length == (int)GlobalData.Instance.gameMode )
                  {
                    //   print(rowOfIndex[i].TrimEnd() + " count : " + rowOfIndex[i].Length);
                    
                    if (WordManager.Instance.isUniqueLettersString(rowOfIndex[i].Split('|')[0]))
                    {
                        // if (rowOfIndex[i].Length != 1)
                        // {
                        // print(rowOfIndex[i]);
                        //   GlobalData.Instance.WordList.Add(rowOfIndex[i].Replace('"', ' ').TrimStart());
                        GlobalData.Instance.TempWordList.Add((rowOfIndex[i].ToUpper()).TrimEnd());
                        //                  }
                    }
                }
              }

           //   int index = Random.Range(0, GlobalData.Instance.WordList.Count);
             // print("word list : " + GlobalData.Instance.WordList.Count);
              WriteString(fileName.ToString());
          //    fileName++;
          }
          //return (GlobalData.Instance.WordList[index].ToUpper()).TrimEnd();
          return "NONE";


      }
    public static void WriteMiscellaneousWord(string miscellaneousWord)
    {

        string path = Application.persistentDataPath + "/MiscellaneousWords.txt";
       // string path = System.IO.Directory.GetCurrentDirectory() + "/MiscellaneousWord.txt";
        print("Path : " + path);

        if (!File.Exists(path))
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write((miscellaneousWord));
                sw.Write('\n');
                sw.Close();

            }
        }
        else
        {
            StreamWriter sw = File.AppendText(path);
            sw.Write((miscellaneousWord));
            sw.Write('\n');
            sw.Close();
        }


        /*   string path = Application.dataPath + "/MiscellaneousWord.txt";
           print("Path : " + path);
          // StreamWriter sw = new StreamWriter("D:\\MySourceWordList.txt", true, Encoding.ASCII);
           StreamWriter sw = new StreamWriter(path, true, Encoding.ASCII);
           if (!File.Exists(path))
           {
               File.WriteAllText(path, miscellaneousWord);
               sw.Write('\n');
           }
           else
           {
               sw.Write((miscellaneousWord));
               sw.Write('\n');
           }
           sw.Close(); */
    }
    public static void WriteString(string fName)
      {
          //StreamWriter sw = new StreamWriter("D:\\Dictionary.txt", true, Encoding.ASCII);
          StreamWriter sw = new StreamWriter("D:\\MySourceWordList.txt", true, Encoding.ASCII);

          string prevStr = "";
          //Write out the numbers 1 to 10 on the same line.
          for (int i = 0; i < GlobalData.Instance.TempWordList.Count; i++)
          {
              //       string str = (GlobalData.Instance.WordList[i]).Split(' ')[0] + "," + (GlobalData.Instance.WordList[i]).Split(')')[1];
              string myStr = (GlobalData.Instance.TempWordList[i]).Split('|')[0];
             // if ( myStr.Length >= 4 && myStr.Length <=6 && (!(myStr.Contains('-'))))//  && (GlobalData.Instance.WordList[i]).Split(' ')[0].ToString().Length <=6)
           //   if ( myStr.Length == (int)GlobalData.Instance.gameMode )//  && (GlobalData.Instance.WordList[i]).Split(' ')[0].ToString().Length <=6)
              {
                 // if (prevStr != myStr)
                  {
                      sw.Write((GlobalData.Instance.TempWordList[i]));
                    sw.Write('\n');
                    //sw.Write((GlobalData.Instance.WordList[i]).Split(' ')[0]);
                    //sw.Write(",");
                    //sw.Write(((GlobalData.Instance.WordList[i]).Split(')')[0]).Split('(')[1]);
                    //sw.Write(",");
                    //sw.Write(((GlobalData.Instance.WordList[i]).Split(')')[1]));
                    //sw.Write('\n');
                    //Word wrd = new Word();
                    //wrd.word = (GlobalData.Instance.WordList[i]).Split(' ')[0];
                    //wrd.type = ((GlobalData.Instance.WordList[i]).Split(')')[0]).Split('(')[1];
                    //wrd.definition = ((GlobalData.Instance.WordList[i]).Split(')')[1]);
                    // mydictionary.Add(wrd);

                    prevStr = myStr;
                  }
              }
          }

          //close the file
          sw.Close();

          //for(int i =0; i< mydictionary.Count; i++)
          //{
          //    print(mydictionary[i].word + "   " + mydictionary[i].type + "   " + mydictionary[i].definition);
          //}


      }
    
    public static List<Word> myDictionary = new List<Word>();
    public static List<Word> WholeDictionary = new List<Word>();
    public static string GetWordToGuess()
    {
        string str = Resources.Load("SourceWordList").ToString();
        // string str = Resources.Load("Z").ToString();     
        string[] rowOfIndex = str.Split('\n');
        GlobalData.Instance.WordList.Clear();
        for (int i = 0; i < rowOfIndex.Length; i++)
        {
            if (rowOfIndex[i].Length == (int)GlobalData.Instance.gameMode )
            {
                if (WordManager.Instance.isUniqueLettersString(rowOfIndex[i]))
                {
                    GlobalData.Instance.WordList.Add(rowOfIndex[i]);
                }
            }
        }
       
        int index = UnityEngine.Random.Range(0, GlobalData.Instance.WordList.Count);
        //print("word list : " + GlobalData.Instance.WordList.Count);
        return (GlobalData.Instance.WordList[index].ToUpper());
    }
    public static void ReadWholeDictionary()
    {
        string str = Resources.Load("words_all").ToString();
        string[] rowOfIndex = str.Split('\n');
        GlobalData.Instance.TempWordList.Clear();
        for (int i = 0; i < rowOfIndex.Length - 1; i++)
        {
            Word myWord = new Word();
            myWord.word = rowOfIndex[i].TrimEnd().ToUpper();
           WholeDictionary.Add(myWord);
        }

    }
    public static string ReadString()
    {
        string dictionarName = "Dictionary" + GlobalData.Instance.gameMode;
      //  string str = Resources.Load("WordList").ToString();     
        string str = Resources.Load(dictionarName).ToString();
   //     print("dictionary : " + str);
       // string str = Resources.Load("Z").ToString();     
        string[] rowOfIndex = str.Split('\n');
     //   print("length : " + rowOfIndex.Length);
        GlobalData.Instance.WordList.Clear();
        for (int i = 0; i < rowOfIndex.Length-1; i++)
        {
            Word myWord = new Word();
             //      print(rowOfIndex[i]);
            string[] tempStr = rowOfIndex[i].Split('|');
            //if (tempStr.Length >= 3)
           // {
                myWord.word = rowOfIndex[i].Split('|')[0].ToUpper();
                myWord.definition = rowOfIndex[i].Split('|')[1];
             ////   myWord.definition = rowOfIndex[i].Split(',')[2];
             //   for(int j= 2; j< tempStr.Length;j++)
             //   {
             //       myWord.definition = myWord.definition + tempStr[j]; 
             //   }
             //   //   if (rowOfIndex[i].Length == (int)GlobalData.Instance.gameMode+1 )
                // { 

                // if (WordManager.Instance.isUniqueLettersString(rowOfIndex[i]))
        //        if (WordManager.Instance.isUniqueLettersString(myWord.word))
          //      {
                    GlobalData.Instance.WordList.Add(myWord.word);
            //    }
                //}
                myDictionary.Add(myWord);
            //}
        }

        int index = UnityEngine.Random.Range(0, GlobalData.Instance.WordList.Count);
        print("word list : " + GlobalData.Instance.WordList.Count);
       // return GetWordToGuess();
        return (GlobalData.Instance.WordList[index].ToUpper());
      //  return (GlobalData.Instance.WordList[index].ToUpper()).TrimEnd();
   //     return (GlobalData.Instance.WordList[0].ToUpper());

        
    }
   // public static string ReadString()
   // {
   //     string dictionarName = "Dictionary" + GlobalData.Instance.gameMode;
   //   //  string str = Resources.Load("WordList").ToString();     
   //     string str = Resources.Load(dictionarName).ToString();     
   //    // string str = Resources.Load("Z").ToString();     
   //     string[] rowOfIndex = str.Split('\n');
   //     GlobalData.Instance.WordList.Clear();
   //     for (int i = 0; i < rowOfIndex.Length; i++)
   //     {
   //         Word myWord = new Word();
   //          //      print(rowOfIndex[i]);
   //         string[] tempStr = rowOfIndex[i].Split(',');
   //         if (tempStr.Length >= 3)
   //         {
   //             myWord.word = rowOfIndex[i].Split(',')[0].ToUpper();
   //             myWord.type = rowOfIndex[i].Split(',')[1];
   //          //   myWord.definition = rowOfIndex[i].Split(',')[2];
   //             for(int j= 2; j< tempStr.Length;j++)
   //             {
   //                 myWord.definition = myWord.definition + tempStr[j]; 
   //             }
   //             //   if (rowOfIndex[i].Length == (int)GlobalData.Instance.gameMode+1 )
   //             // { 

   //             // if (WordManager.Instance.isUniqueLettersString(rowOfIndex[i]))
   //             if (WordManager.Instance.isUniqueLettersString(myWord.word))
   //             {
   //                 GlobalData.Instance.WordList.Add(myWord.word);
   //             }
   //             //}
   //             myDictionary.Add(myWord);
   //         }
   //     }

   //     int index = UnityEngine.Random.Range(0, GlobalData.Instance.WordList.Count);
   //     print("word list : " + GlobalData.Instance.WordList.Count);
   //     return GetWordToGuess();
   //    // return (GlobalData.Instance.WordList[index].ToUpper());
   //   //  return (GlobalData.Instance.WordList[index].ToUpper()).TrimEnd();
   ////     return (GlobalData.Instance.WordList[0].ToUpper());

        
   // }
    public void GetWordDefinitionOffline()
    {
        for(int  i=0;i < myDictionary.Count;i++)
        {
            if(myDictionary[i].word ==  WordToGuess)
            {
                WordDefinition = myDictionary[i].definition;
            }
        }
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
            //  string letterOnCurrentCell = UIManager.Instance.ContentHolder.transform.GetChild(row).transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            string letterOnCurrentCell = UIManager.Instance.TopRow.transform.GetChild(row).transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            if (letterOnCurrentCell != "" )
            {
                length++;
            }
            else
            {
                break;
            }
        }
      //  print("Length from fun : " + length);
        return length;
    }
    
    public List<string> getLetterList()
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

public class Word 
{
    public string word;
    public string type;
    public string definition;
}