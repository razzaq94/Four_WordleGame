using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameStorage : MonoBehaviour
{
    public static InGameStorage Instance;
    // Start is called before the first frame update
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int scr)
    {
        PlayerPrefs.SetInt("SCORE", GetScore()+scr);
    }
    public int GetScore()
    {
        return PlayerPrefs.GetInt("SCORE", 0);
    }
    public void SetLevel (int lvl)
    {
        PlayerPrefs.SetInt("LEVEL", lvl);
    }
    public int GetLevel()
    {
        return PlayerPrefs.GetInt("LEVEL", 0);
    } 
    public void SetName(string nme)
    {
        PlayerPrefs.SetString("NAME", nme);
    }
    public string GetName()
    {
        return PlayerPrefs.GetString("NAME", "Anonymous");
    }
}
