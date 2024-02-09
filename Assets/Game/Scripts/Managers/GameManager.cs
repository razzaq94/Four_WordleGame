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

     //   APIManager.Instance.SaveGameData("TAKE", 321, 157, 17, true, "Easy");
    }

    private void StartGame()
    {
        WordManager.Instance.GetWord();
        UIManager.Instance.ResetGamePanelUI();
        //   UIManager.Instance.CreateEmptyRow();
        UIManager.Instance.CreateTopRow();
        KeyboardManager.Instance.IsInterectable = true;
        UIManager.Instance.UpdateInterectabilityBossterButtons(true);
        UIManager.Instance.UpdateInterectabilityBackButton(true);
        WordManager.Instance.revealList.Clear();
        StartCoroutine(waitAndCallGetWordDefinition());
    }
    IEnumerator waitAndCallGetWordDefinition()
    {
        yield return new WaitForSeconds(2f);
        WordManager.Instance.GetWordDefinitionOffline();
       // WordManager.Instance.getWordDefinition();
    }
    public void BackToMainMenu()
    {
        Destroy(NetworkAPIManager.Instance.gameObject);
        SceneManager.LoadScene("SplashScene");
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene("Game");
    }



}

