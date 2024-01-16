using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalData : MonoBehaviour
{
    public GameMode gameMode = GameMode.Hard;
    public List<string> WordList;
    public static GlobalData Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
        }

    }
    public void SetGamemode(int gm)
    {
        if(gm == 1)
        {
            gameMode = GameMode.Easy;
        }
        else if(gm == 2)
        {
            gameMode = GameMode.Medium;

        }
        else if(gm == 3)
        {
            gameMode = GameMode.Hard;

        }
    }
    public void Loadlevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public enum GameMode
    {
        Easy = 4,
        Medium = 5,
        Hard = 6
    }
}
