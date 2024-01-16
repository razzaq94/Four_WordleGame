using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuUIManager : MonoBehaviour
{
	public static MainMenuUIManager Instance;

    private void Awake()
    {
		Instance = this;
    }
	
}
