using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject GamePanel;

    
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ResetGamePanelUI();
    }

    public void ResetGamePanelUI()
    {
        GameObject GridPanel = GamePanel.transform.Find("GridPanel").gameObject;
        for(int i =0; i < GridPanel.transform.childCount;i++)
        {
            GameObject RowPanel = GridPanel.transform.GetChild(i).gameObject;
            for(int j =0; j  < RowPanel.transform.childCount ;  j++)
            {
                RowPanel.transform.GetChild(j).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            }

        }
    }

    public void WriteLetter(string letter)
    {
        int row = WordManager.Instance.CurrentRowNumber;
        int col = WordManager.Instance.CurrentColNumber;
        string letterOnCurrentCell = GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
        if (letter != "" && WordManager.Instance.CanWrite && letterOnCurrentCell =="")
        {
               GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = letter;
            WordManager.Instance.SetNextColNumber();
        }
        else if (letter != "" && WordManager.Instance.CanWrite && letterOnCurrentCell != "" && col < ((int) GlobalData.Instance.gameMode -1) )
        {
            GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col+1).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = letter;
            WordManager.Instance.SetNextColNumber();

        }

    }
    public void RemoveLetter()
    {
        int row = WordManager.Instance.CurrentRowNumber;
        int col = WordManager.Instance.CurrentColNumber;
        string letterOnCurrentCell = GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
        if(letterOnCurrentCell == "" && col-1 >=0)
        {
            GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col-1).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            WordManager.Instance.SetPreviousColNumber();

        }
        else
        {
            GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            WordManager.Instance.SetPreviousColNumber();

        }
        WordManager.Instance.CanWrite = true;

    }


    public string GetToCheckWord()
    {
        string tempWordToCheck = "";
        for (int i = 0; i < (int)GlobalData.Instance.gameMode; i++)
        {
            string temp = GamePanel.transform.Find("GridPanel").transform.GetChild(WordManager.Instance.CurrentRowNumber).transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            tempWordToCheck += temp;
        }

        return tempWordToCheck;
    }


}
