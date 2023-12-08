using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        WordManager.Instance.GetWord();
        UIManager.Instance.ResetGamePanelUI();
        UIManager.Instance.SetGameGridUI();
        StartCoroutine(waitAndCallGetWordDefinition());
    }
    IEnumerator waitAndCallGetWordDefinition()
    {
        yield return new WaitForSeconds(2f);
        WordManager.Instance.getWordDefinition();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene("Game");
    }



}
