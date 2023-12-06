using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public GameMode gameMode = GameMode.Hard;
    public static GlobalData Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum GameMode
    {
        Easy = 4,
        Medium = 5,
        Hard = 6
    }
}
