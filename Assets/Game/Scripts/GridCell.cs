using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GridCell : MonoBehaviour
{
    [SerializeField] Image bg_image;
    [SerializeField] TextMeshProUGUI CellText;
    int currentColorIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeColor()
    {
        if (CellText.text.ToString() != "")
        {
            int ind = currentColorIndex % WordManager.Instance.colors.Count;
            switch (ind)
            {
                case 0:
                    bg_image.color = WordManager.Instance.revealedColor;
                    KeyboardManager.Instance.ChangeKeyColor(CellText.text.ToString(), WordManager.Instance.revealedColor);
                    UIManager.Instance.ChangeKeyColor(CellText.text.ToString(), WordManager.Instance.revealedColor);
                    break;
                case 1:
                    bg_image.color = WordManager.Instance.partiallyRevealedColor;
                    KeyboardManager.Instance.ChangeKeyColor(CellText.text.ToString(), WordManager.Instance.partiallyRevealedColor);
                    UIManager.Instance.ChangeKeyColor(CellText.text.ToString(), WordManager.Instance.partiallyRevealedColor);
                    break;
                case 2:
                    bg_image.color = WordManager.Instance.concealedColor;
                    KeyboardManager.Instance.ChangeKeyColor(CellText.text.ToString(), WordManager.Instance.concealedColor);
                    UIManager.Instance.ChangeKeyColor(CellText.text.ToString(), WordManager.Instance.concealedColor);
                    break;

            }

            currentColorIndex++;
        }
    }
    public string GetCellText()
    {
        return CellText.text;
    }
    public void SetCellText(string txt)
    {
        CellText.text = txt;
    }
    public Color GetCellBg_Color()
    {
        return bg_image.color;
    }
    public void SetCellBg_Color(Color clr)
    {
        bg_image.color = clr;
    }
}
