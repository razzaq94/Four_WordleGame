using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;
using System;

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

        //StartCoroutine(waitAndShowUI());

        //if(InGameStorage.Instance.GetUserId() ==  "")
        //{
        //    LogInPanelUI.ShowUI();
        //}
        //else
        //{
        //    LogIn();
        //    MainPanelUI.ShowUI();

        //}





    }
    IEnumerator waitAndShowUI()
    {
        yield return new WaitForSeconds(0.1f);
        MainPanelUI.ShowUI();

    }


}

   
    
