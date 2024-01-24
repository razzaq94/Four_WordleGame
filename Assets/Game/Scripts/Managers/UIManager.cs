using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject GamePanel;
    [SerializeField] public GameObject TopRow;
    [SerializeField] public GameObject ContentHolder;
    [SerializeField] public GameObject Row_PF;
    [SerializeField] public GameObject Cell_PF;

    //Boasters
    [SerializeField] GameObject Reveal;
    [SerializeField] GameObject Eliminate;
    [SerializeField] GameObject AutoColor;

    public Sprite revealedBgSprite;
    public Sprite concealedBgSprite;
    public Sprite originalBgSprite;


    // porivate 
    private int leftPadding =20;
    private Vector2 cellSize = new Vector2(150, 150);
    
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //CreateEmptyRow();
        KeyboardManager.Instance.ChangeKeySize("Backspace", new Vector2(200, 99));
    }
    public void CreateTopRow()
    {
        adjustDimenssions();
        GameObject row = Instantiate(Row_PF, TopRow.transform);
        row.transform.GetComponent<HorizontalLayoutGroup>().padding.left = leftPadding;
        row.transform.localPosition = new Vector3(row.transform.localPosition.x, 65, row.transform.localPosition.z);
        row.transform.SetAsFirstSibling();
        for (int i = 0; i <= (int)GlobalData.Instance.gameMode; i++)
        {
            GameObject cell = Instantiate(Cell_PF, row.transform);
            cell.GetComponent<RectTransform>().sizeDelta = cellSize;
            cell.transform.SetAsLastSibling();
            if (i == (int)GlobalData.Instance.gameMode)
            {
                Destroy(cell.GetComponent<GridCell>());
                cell.transform.Find("Bg_Color").GetComponent<Image>().color = WordManager.Instance.counterBgColor;
            }
        }
        WordManager.Instance.CurrentColNumber = 0;
        WordManager.Instance.CurrentRowNumber = 0;
        WordManager.Instance.CanWrite = true;
        KeyboardManager.Instance.UpdateEnterButton(false);

    }
    public void CreateEmptyRow()
    {
        adjustDimenssions();
        GameObject row = Instantiate(Row_PF, ContentHolder.transform);
        row.transform.GetComponent<HorizontalLayoutGroup>().padding.left = leftPadding;
        row.transform.SetAsFirstSibling();
        for(int i = 0;i <= (int) GlobalData.Instance.gameMode; i++)
        {
            GameObject cell = Instantiate(Cell_PF, row.transform);
            cell.GetComponent<RectTransform>().sizeDelta = cellSize;
            cell.transform.SetAsLastSibling();
            if(i == (int)GlobalData.Instance.gameMode)
            {
                Destroy(cell.GetComponent<GridCell>());
                cell.transform.Find("Bg_Color").GetComponent<Image>().color = WordManager.Instance.counterBgColor;

            }
        }
        WordManager.Instance.CurrentColNumber = 0;
        WordManager.Instance.CurrentRowNumber = 0;
        WordManager.Instance.CanWrite = true;
        KeyboardManager.Instance.UpdateEnterButton(false);



    }
    public void CopyLetters()
    {
        for (int i = 0; i <= (int)GlobalData.Instance.gameMode; i++)
        {
            string letter = TopRow.transform.GetChild(0).transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            ContentHolder.transform.GetChild(0).transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = letter;
            if (i != (int)GlobalData.Instance.gameMode)
            {
                ContentHolder.transform.GetChild(0).transform.GetChild(i).GetComponent<GridCell>().SetCellBg_Color(KeyboardManager.Instance.GetKeyColor(letter));
            }

        }
    }
    public void ClearCurrentRow()
    {
        for (int i = 0; i <= (int)GlobalData.Instance.gameMode; i++)
        {
            //       ContentHolder.transform.GetChild(0).transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            //      ContentHolder.transform.GetChild(0).transform.GetChild(i).transform.Find("Bg_Color").GetComponent<Image>().color = WordManager.Instance.originalBgColor;
            TopRow.transform.GetChild(0).transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            if (i == (int)GlobalData.Instance.gameMode)
            {
                TopRow.transform.GetChild(0).transform.GetChild(i).transform.Find("Bg_Color").GetComponent<Image>().color = WordManager.Instance.counterBgColor;
            }
            else
            {
                TopRow.transform.GetChild(0).transform.GetChild(i).transform.Find("Bg_Color").GetComponent<Image>().color = WordManager.Instance.originalBgColor;
            }
        }
        WordManager.Instance.CurrentColNumber = 0;
        WordManager.Instance.CurrentRowNumber = 0;
        WordManager.Instance.CanWrite = true;
        KeyboardManager.Instance.UpdateEnterButton(false);


    }
    private void adjustDimenssions()
    {
        switch(GlobalData.Instance.gameMode)
        {
            case GlobalData.GameMode.Easy:
                cellSize = new Vector2(150, 150);
                leftPadding = 20;
                break;
            case GlobalData.GameMode.Medium:
                cellSize = new Vector2(120, 120);
                leftPadding = 25;
                break;
            case GlobalData.GameMode.Hard:
                cellSize = new Vector2(100, 100);
                leftPadding = 30;
                break;
        }
    }
    public void ChangeAllKeyColorToDefault()
    {
        for (int i = 0; i < ContentHolder.transform.childCount; i++)
        {
            GameObject row = ContentHolder.transform.GetChild(i).gameObject;
            for (int j = 0; j < row.transform.childCount - 1; j++)
            {
                GridCell cell = row.transform.GetChild(j).GetComponent<GridCell>();

                cell.SetCellBg_Color(WordManager.Instance.originalBgColor);

            }
        }


    } public void ChangeKeyColor(string myKey, Color color)
    {
        for(int i = 0; i <ContentHolder.transform.childCount;i++)
        {
            GameObject row = ContentHolder.transform.GetChild(i).gameObject;
            for(int j =0; j < row.transform.childCount-1;j++ )
            {
                GridCell cell = row.transform.GetChild(j).GetComponent<GridCell>();
                if (cell.GetCellText() == myKey)
                {
                    cell.SetCellBg_Color(color);
                }
            }
        }


    }
    public void SetGameGridUI()
    {
        GameObject GridPanel = GamePanel.transform.Find("GridPanel").gameObject;
        for (int i = 0; i < GridPanel.transform.childCount; i++)
        {
            GameObject RowPanel = GridPanel.transform.GetChild(i).gameObject;
            if (GlobalData.Instance.gameMode == GlobalData.GameMode.Medium)
            {
                RowPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 40;
            }
            else if (GlobalData.Instance.gameMode == GlobalData.GameMode.Easy)
            {
                RowPanel.GetComponent<HorizontalLayoutGroup>().padding.left = 70;
            }
            for (int j = (int)GlobalData.Instance.gameMode; j < RowPanel.transform.childCount; j++)
            {
                RowPanel.transform.GetChild(j).gameObject.SetActive(false);
            }

        }
    }
    public void ResetGamePanelUI()
    {
        //GameObject GridPanel = GamePanel.transform.Find("GridPanel").gameObject;
        //for(int i =0; i < GridPanel.transform.childCount;i++)
        //{
        //    GameObject RowPanel = GridPanel.transform.GetChild(i).gameObject;
        //    for(int j =0; j  < RowPanel.transform.childCount ;  j++)
        //    {
        //        RowPanel.transform.GetChild(j).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
        //    }

        //}
        GameObject GridPanel = GamePanel.transform.Find("GridPanel").gameObject;
        for(int i = ContentHolder.transform.childCount-1; i >= 0;i--)
        {
            Destroy( GridPanel.transform.GetChild(i).gameObject);
        }
    }

    public void WriteLetter(string letter)
    {
        //  int row = WordManager.Instance.CurrentRowNumber;
        if (WordManager.Instance.CanWrite)
        {
            int row = 0;
            int col = WordManager.Instance.CurrentColNumber;
            //   string letterOnCurrentCell = GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            string letterOnCurrentCell = TopRow.transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            if (letter != "" && WordManager.Instance.CanWrite && letterOnCurrentCell == "")
            {
                //   GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = letter;
                TopRow.transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = letter;
                TopRow.transform.GetChild(row).transform.GetChild(col).GetComponent<GridCell>().SetCellBg_Color(KeyboardManager.Instance.GetKeyColor(letter));
                WordManager.Instance.SetNextColNumber();
            }
            else if (letter != "" && WordManager.Instance.CanWrite && letterOnCurrentCell != "" && col < ((int)GlobalData.Instance.gameMode - 1))
            {
                //  GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col+1).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = letter;
                TopRow.transform.GetChild(row).transform.GetChild(col + 1).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = letter;
                TopRow.transform.GetChild(row).transform.GetChild(col).GetComponent<GridCell>().SetCellBg_Color(KeyboardManager.Instance.GetKeyColor(letter));
                WordManager.Instance.SetNextColNumber();
            }
            // print("wordlength : " + WordManager.Instance.GetCurrentWordLength() + "game mode count : " + (((int)GlobalData.Instance.gameMode)));
            if (WordManager.Instance.GetCurrentWordLength() == (((int)GlobalData.Instance.gameMode)))
            {

                //WordManager.Instance.CheckWordOnline();
                WordManager.Instance.CheckWordOffline();
       
                //KeyboardManager.Instance.UpdateEnterButton(true);
            }
        }
    }
    public void RemoveLetter()
    {
       // int row = WordManager.Instance.CurrentRowNumber;
        int row = 0;
        int col = WordManager.Instance.CurrentColNumber;
        //string letterOnCurrentCell = GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
        string letterOnCurrentCell = TopRow.transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
        if(letterOnCurrentCell == "" && col-1 >=0)
        {
            //GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col-1).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            TopRow.transform.GetChild(row).transform.GetChild(col-1).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
            TopRow.transform.GetChild(row).transform.GetChild(col - 1).transform.Find("Bg_Color").GetComponent<Image>().color = WordManager.Instance.originalBgColor;
            WordManager.Instance.SetPreviousColNumber();

        }
        else 
        {
         //   GamePanel.transform.Find("GridPanel").transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
           
            TopRow.transform.GetChild(row).transform.GetChild(col).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "";
           TopRow.transform.GetChild(row).transform.GetChild(col ).transform.Find("Bg_Color").GetComponent<Image>().color = WordManager.Instance.originalBgColor;
           WordManager.Instance.SetPreviousColNumber();

        }
        WordManager.Instance.CanWrite = true;
        KeyboardManager.Instance.UpdateEnterButton(false);

    }


    public string GetToCheckWord()
    {
        string tempWordToCheck = "";
        for (int i = 0; i < (int)GlobalData.Instance.gameMode; i++)
        {
            string temp = TopRow.transform.GetChild(0).transform.GetChild(i).transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            tempWordToCheck += temp;
        }

        return tempWordToCheck;
    }
    public void UpdateRevealUI(int remaining)
    {
        if (remaining > 0)
        {
            Reveal.transform.Find("CountImage").gameObject.SetActive(true);
            Reveal.transform.Find("AdImage").gameObject.SetActive(false);
            Reveal.transform.Find("CountImage").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = remaining.ToString();

        }
        else
        {
            Reveal.transform.Find("CountImage").gameObject.SetActive(false); ;
            Reveal.transform.Find("AdImage").gameObject.SetActive(true);

        }

    } 
    public void UpdateEliminateUI(int remaining)
    {
        if (remaining > 0)
        {
            Eliminate.transform.Find("CountImage").gameObject.SetActive(true);
            Eliminate.transform.Find("AdImage").gameObject.SetActive(false);
            Eliminate.transform.Find("CountImage").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = remaining.ToString();

        }
        else
        {
            Eliminate.transform.Find("CountImage").gameObject.SetActive(false) ;
            Eliminate.transform.Find("AdImage").gameObject.SetActive(true);
        }

    }
    public void UpdateAutoColorUI(int remaining)
    {
        if (remaining > 0)
        {
            AutoColor.transform.Find("CountImage").gameObject.SetActive(true);
            AutoColor.transform.Find("AdImage").gameObject.SetActive(false);
            AutoColor.transform.Find("CountImage").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = remaining.ToString();
        }
        else
        {
            AutoColor.transform.Find("CountImage").gameObject.SetActive(false);
            AutoColor.transform.Find("AdImage").gameObject.SetActive(true);
        }

    }





    public void OpenQuitPanel()
    {
        QuitPanelUI.ShowUI();
    }





}
