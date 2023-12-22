using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreUI;
    [SerializeField] TextMeshProUGUI LevelUI;
    [SerializeField] TextMeshProUGUI UserNameUI;
    

    public static MainMenuUIManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreUI();
        UpdateLevelUI();
        UpdateNameUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScoreUI()
    {
        ScoreUI.text = (InGameStorage.Instance.GetScore()).ToString();
    } 
    public void UpdateLevelUI()
    {
        LevelUI.text ="LV " +(InGameStorage.Instance.GetLevel()).ToString();
    }
    public void UpdateNameUI()
    {
        UserNameUI.text = (InGameStorage.Instance.GetName()).ToString();
    }
}
