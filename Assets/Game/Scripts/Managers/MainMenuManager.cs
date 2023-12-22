using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject ModePanel;
    [SerializeField] Button PlayButton;
    public static MainMenuManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {


    }
    public void OnClick_PlayButton()
    {
        ModePanel.SetActive(true);
        PlayButton.interactable = false;

    }

    public void Loadlevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void CallToSetGamemode(int gm)
    {
        GlobalData.Instance.SetGamemode(gm);
    }
    }
