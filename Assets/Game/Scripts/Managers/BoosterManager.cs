using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BoosterManager : MonoBehaviour
{
   // [SerializeField] int revealBoosterCount = 2;
  //  [SerializeField] int eliminateBoosterCount = 3;
  //  [SerializeField]public int autocolorBoosterCount = 1;
    public bool isAutoColor = false;
    public static BoosterManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateBoosterCountAndUI();
    }
    public void UpdateAllBoosterCount(int count)
    {
        if (GlobalData.Instance.RevealBoosterCount <= 0)
        {
            GlobalData.Instance.RevealBoosterCount += count;
        }
        if (GlobalData.Instance.EliminateBoosterCount <= 0)
        {
            GlobalData.Instance.EliminateBoosterCount += count;
        }
    }
    public void UpdateBoosterCountAndUI()
    {
        UpdateAllBoosterCount(1);
        UIManager.Instance.UpdateRevealUI(GlobalData.Instance.RevealBoosterCount);
        UIManager.Instance.UpdateEliminateUI(GlobalData.Instance.EliminateBoosterCount);
        //   UIManager.Instance.UpdateAutoColorUI(GlobalData.Instance.AutoColorBoosterCount);


        Boosters mybooster = new Boosters();

        mybooster.id = GlobalData.Instance.RevealBoosterId;
        mybooster.count = GlobalData.Instance.RevealBoosterCount;
        GlobalData.Instance.boosters[0] = mybooster;

        mybooster.id = GlobalData.Instance.AutoColorBoosterId;
        mybooster.count = GlobalData.Instance.AutoColorBoosterCount;
        GlobalData.Instance.boosters[1] = mybooster;

        mybooster.id = GlobalData.Instance.EliminateBoosterId;
        mybooster.count = GlobalData.Instance.EliminateBoosterCount;
        GlobalData.Instance.boosters[2] = mybooster;


        SaveBoosters mySavebooster = new SaveBoosters();

        mySavebooster.name = "Booster1";
        mySavebooster.count = GlobalData.Instance.RevealBoosterCount;
        GlobalData.Instance.saveBoosters[0] = mySavebooster;

        mySavebooster.name = "Booster2";
        mySavebooster.count = GlobalData.Instance.AutoColorBoosterCount;
        GlobalData.Instance.saveBoosters[1] = mySavebooster;

        mySavebooster.name = "Booster3";
        mySavebooster.count = GlobalData.Instance.EliminateBoosterCount;
        GlobalData.Instance.saveBoosters[2] = mySavebooster;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Reveal()
    {
        SoundManager.instance.Play_BUTTON_CLICK_Sound();
        if (GlobalData.Instance.RevealBoosterCount > 0)
        {

            List<string> letterList = WordManager.Instance.getLetterList();
            List<string> OriginalLetterList = WordManager.Instance.getLetterList();
            List<string> TempRevealLetterList = new List<string>();
            for(int i =0; i < WordManager.Instance.revealList.Count; i++)
            {
                TempRevealLetterList.Add(WordManager.Instance.revealList[i].word);
            }
            int length = letterList.Count;


            for (int i = 0; i < length; i++)
            {
                int idx = UnityEngine.Random.Range(0, letterList.Count);
                string tempLetter = letterList[idx].ToString();
                if (TempRevealLetterList.IndexOf(tempLetter) < 0)
                {
                    KeyboardManager.Instance.ChangeKeyColor(letterList[idx].ToString(), WordManager.Instance.revealedColor);
                    UIManager.Instance.ChangeKeyColor(letterList[idx].ToString(), WordManager.Instance.revealedColor);
                    GlobalData.Instance.UpdateBoosterCount("Reveal", -1);
                    UIManager.Instance.UpdateRevealUI(GlobalData.Instance.RevealBoosterCount);
                    Reveal r = new Reveal();
                    r.word = letterList[idx].ToString();
                    r.index = OriginalLetterList.IndexOf(letterList[idx]);
                    WordManager.Instance.revealList.Add(r);
                    UIManager.Instance.ShowRevealLetterOnTopRow();
                    break;
                }
                else
                {
                    letterList.Remove(letterList[idx]);
                }
            }
        }
        else
        {
            print("Ads...");
            // Run Ad
            AdsManager.instance.GetBooster = true;
            AdsManager.instance.ShowRewardedVideoAd();

            GlobalData.Instance.isRevealBoosterAdShown = true;
            //Reward

        }
    }
    //public void Reveal()
    //{
    //    SoundManager.instance.Play_BUTTON_CLICK_Sound();
    //    if (GlobalData.Instance.RevealBoosterCount > 0)
    //    {

    //        List<string> letterList = WordManager.Instance.getLetterList();
    //        List<string> OriginalLetterList = WordManager.Instance.getLetterList();
    //        int length = letterList.Count;
    //        for (int i = 0; i < length; i++)
    //        {
    //            int idx = UnityEngine.Random.Range(0, letterList.Count);
    //            if (KeyboardManager.Instance.GetKeyColor(letterList[idx].ToString()) != WordManager.Instance.revealedColor)
    //            {
    //                KeyboardManager.Instance.ChangeKeyColor(letterList[idx].ToString(), WordManager.Instance.revealedColor);
    //                UIManager.Instance.ChangeKeyColor(letterList[idx].ToString(), WordManager.Instance.revealedColor);
    //               // GlobalData.Instance.RevealBoosterCount--;
    //                GlobalData.Instance.UpdateBoosterCount("Reveal",-1);
    //                UIManager.Instance.UpdateRevealUI(GlobalData.Instance.RevealBoosterCount);
    //                Reveal r = new Reveal();
    //                r.word = letterList[idx].ToString();
    //                //r.index = idx;
    //                r.index = OriginalLetterList.IndexOf(letterList[idx]);
    //                WordManager.Instance.revealList.Add(r);
    //                UIManager.Instance.ShowRevealLetterOnTopRow();
    //                break;
    //            }

    //            letterList.Remove(letterList[idx]);

    //        }
    //    }
    //    else
    //    {
    //        print("Ads...");
    //        // Run Ad
    //        AdsManager.instance.ShowRewardedVideoAd();
    //        GlobalData.Instance.isRevealBoosterAdShown = true;
    //        //Reward

    //    }
    //}
    public List<string> getAlphabets()
    {
        char alp = 'A';
        List<string> alphList = new List<string>();
        for (int i = 0; i < 26; i++)
        {

            alphList.Add(alp.ToString());
            alp++;
        }
        return alphList;
    }
    public void Eliminate()
    {
        SoundManager.instance.Play_BUTTON_CLICK_Sound();
        if (GlobalData.Instance.EliminateBoosterCount > 0)
        {
            List<string> alphabetList = getAlphabets();

            List<string> letterList = WordManager.Instance.getLetterList();
            int length = alphabetList.Count;
            int count = 0;
            for (int i = 0; i < length; i++)
            {
                int idx = UnityEngine.Random.Range(0, alphabetList.Count);
                string alphabet = alphabetList[idx];
                if (!(letterList.Contains(alphabet)) && KeyboardManager.Instance.GetKeyColor(alphabet) != WordManager.Instance.concealedColor)
                {
                    KeyboardManager.Instance.ChangeKeyColor(alphabet, WordManager.Instance.concealedColor);
                    UIManager.Instance.ChangeKeyColor(alphabet, WordManager.Instance.concealedColor);
                    count++;
                }
                alphabetList.Remove(alphabet);
                if (count == 5)
                {
                    break;
                }
                else if (count == 0 && alphabetList.Count == 0)
                {
                    // don't use booster
                }

            }
            if (count > 0)
            {
  //              GlobalData.Instance.EliminateBoosterCount--;
                GlobalData.Instance.UpdateBoosterCount("Eliminate", -1);
                UIManager.Instance.UpdateEliminateUI(GlobalData.Instance.EliminateBoosterCount);

            }
        }
        else
        {
            print("Ads...");
            AdsManager.instance.GetBooster = true;
            AdsManager.instance.ShowRewardedVideoAd();
            //Reward
            GlobalData.Instance.isEliminateBoosterAdShown = true;
           

        }
    }
    public void AutoColor()
    {
        isAutoColor = true;
//        GlobalData.Instance.AutoColorBoosterCount--;
        GlobalData.Instance.UpdateBoosterCount("AutoColor", -1);
        UIManager.Instance.UpdateAutoColorUI(GlobalData.Instance.AutoColorBoosterCount);
        GameObject rows = UIManager.Instance.ContentHolder;
        int counterIndex = (int)GlobalData.Instance.gameMode;
        Color color = WordManager.Instance.concealedColor;


        for (int i = rows.transform.childCount - 1; i > 0; i--)
        {
            color = WordManager.Instance.revealedColor;
            int matchedLetterCount = System.Convert.ToInt32(rows.transform.GetChild(i).transform.GetChild((int)GlobalData.Instance.gameMode).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.ToString());
            if (matchedLetterCount != 0)
            {
                GameObject row = rows.transform.GetChild(i).gameObject;
                color = WordManager.Instance.revealedColor;
                List<GameObject> orignalColorCells = new List<GameObject>();

                int cellCountTobChanged = matchedLetterCount;
                for (int k = 0; k < counterIndex; k++)
                {
                    Color currentCellBgColor = row.transform.GetChild(k).GetComponent<GridCell>().GetCellBg_Color();
                    if (currentCellBgColor == WordManager.Instance.revealedColor)
                    {
                        cellCountTobChanged--;
                    }
                    else if (currentCellBgColor != WordManager.Instance.concealedColor)
                    {
                        orignalColorCells.Add(row.transform.GetChild(k).gameObject);
                    }
                }
                for (int j = 0; j < cellCountTobChanged; j++)
                {
                    int ind = UnityEngine.Random.Range(0, orignalColorCells.Count);

                    string key = row.transform.GetChild(ind).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
                    UIManager.Instance.ChangeKeyColor(key, color);
                    KeyboardManager.Instance.ChangeKeyColor(key, color);

                    orignalColorCells.Remove(row.transform.GetChild(ind).gameObject);
                }
            }
        }
       // for (int i = 1; i < rows.transform.childCount; i++)       
        for (int i = rows.transform.childCount-1; i > 0 ; i--)
        {
            color = WordManager.Instance.concealedColor;
            int matchedLetterCount = System.Convert.ToInt32(rows.transform.GetChild(i).transform.GetChild((int)GlobalData.Instance.gameMode).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.ToString());
           // if (rows.transform.GetChild(i).transform.GetChild(4).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text == "0")
            if(matchedLetterCount == 0)
            {
                print("innn");
                GameObject row = rows.transform.GetChild(i).gameObject;
                for (int j = 0; j < row.transform.childCount; j++)
                {
                    print("mazeedinnn");
                    string key = row.transform.GetChild(j).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
                    UIManager.Instance.ChangeKeyColor(key, color);
                    KeyboardManager.Instance.ChangeKeyColor(key, color);
                }
            }
            //else
            //{
            //    GameObject row = rows.transform.GetChild(i).gameObject;
            //    color = WordManager.Instance.revealedColor;
            //    List<GameObject> orignalColorCells = new List<GameObject>();

            //    int cellCountTobChanged = matchedLetterCount;
            //    for (int k = 0; k < counterIndex; k++)
            //    {
            //        Color currentCellBgColor = row.transform.GetChild(k).GetComponent<GridCell>().GetCellBg_Color();
            //        if (currentCellBgColor == WordManager.Instance.revealedColor)
            //        {
            //            cellCountTobChanged--;
            //        }
            //        else if (currentCellBgColor != WordManager.Instance.concealedColor)
            //        {
            //            orignalColorCells.Add(row.transform.GetChild(k).gameObject);
            //        }
            //    }
            //    for (int j = 0; j < cellCountTobChanged; j++)
            //    {
            //        int ind = UnityEngine.Random.Range(0, orignalColorCells.Count);

            //        string key = row.transform.GetChild(ind).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            //        UIManager.Instance.ChangeKeyColor(key, color);
            //        KeyboardManager.Instance.ChangeKeyColor(key, color);

            //        orignalColorCells.Remove(row.transform.GetChild(ind).gameObject);

            //    }


            //}

        }
    }
    public void AutoColorLatestRow()
    {
        GameObject rows = UIManager.Instance.ContentHolder;
        int counterIndex = (int)GlobalData.Instance.gameMode;
        Color color = WordManager.Instance.concealedColor;
        int matchedLetterCount = System.Convert.ToInt32(rows.transform.GetChild(1).transform.GetChild((int)GlobalData.Instance.gameMode).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text.ToString());
        // if (rows.transform.GetChild(1).transform.GetChild((int)GlobalData.Instance.gameMode).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text == "0")
        if (matchedLetterCount == 0)
        {
            color = WordManager.Instance.concealedColor;
            print("00000000000");
            GameObject row = rows.transform.GetChild(1).gameObject;
            for (int j = 0; j < counterIndex; j++)
            {

                string key = row.transform.GetChild(j).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
                UIManager.Instance.ChangeKeyColor(key, color);
                KeyboardManager.Instance.ChangeKeyColor(key, color);
            }
        }






    }

   

   
}


