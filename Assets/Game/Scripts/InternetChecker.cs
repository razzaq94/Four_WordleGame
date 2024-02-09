using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InternetChecker : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CheckInternetConnection());
    }

    IEnumerator CheckInternetConnection()
    {
        using (UnityWebRequest www = new UnityWebRequest("http://www.google.com"))
        {
            www.timeout = 5; // Set a timeout for the request
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("No internet connection");
            }
            else
            {
                Debug.Log("Internet connection available");
            }
        }
    }
}
