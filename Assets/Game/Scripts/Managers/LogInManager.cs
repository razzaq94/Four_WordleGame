using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInManager : MonoBehaviour
{
    public static LogInManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //LogInPanelUI.ShowUI();
        if(InGameStorage.Instance.GetName() == "")
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
