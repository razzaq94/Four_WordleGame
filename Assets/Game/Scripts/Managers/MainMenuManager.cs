using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        MainPanelUI.ShowUI();

    }
}

   
    
