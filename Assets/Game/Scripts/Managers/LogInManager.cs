using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class LogInManager : MonoBehaviour
{
	[SerializeField] Image ProgressBar;
	[SerializeField] TextMeshProUGUI ProgressText;
	AsyncOperation asyncOperation;


	public static LogInManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

		//if (InGameStorage.Instance.GetUserId() == "")
  //      {
  //          LogInPanelUI.ShowUI();
  //      }
  //      else
  //      {
            APIManager.Instance.LogIn();
			LogInManager.Instance.OpenGameScene();
			UpdateSceneActivationStatus(false);


		//}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OpenGameScene()
	{
		//  SceneManager.LoadScene("GameScene");
		//	SoundManager.instance.Play_BUTTON_CLICK_Sound();
		StartCoroutine(LoadScene());
	}
	public void UpdateSceneActivationStatus(bool status)
	{
		asyncOperation.allowSceneActivation = status;

	}
	IEnumerator LoadScene()
	{
		float progress = 0f;
		//LoadingPanel.SetActive(true);
		string sceneName = "MainMenu";
		asyncOperation = SceneManager.LoadSceneAsync(sceneName);
		asyncOperation.allowSceneActivation = true;



		while (progress <= 0.90f)
		{

			ProgressBar.fillAmount = progress;

			ProgressText.text = Mathf.Round(progress * 100f) + "%";

			progress += .01f;

			yield return new WaitForSeconds(.01f);
		}





		while (!asyncOperation.isDone)
		{
			//Output the current progress
			ProgressText.text = Math.Round(asyncOperation.progress * 100) + "%";
			ProgressBar.fillAmount = (asyncOperation.progress );
			

			// Check if the load has finished
			if (asyncOperation.progress >= 0.90f)
			{
				ProgressText.text = Math.Round(asyncOperation.progress * 100) + "%";
//				ProgressText.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
				ProgressBar.fillAmount = (asyncOperation.progress );


				asyncOperation.allowSceneActivation = false;
			}

			yield return null;
		}
	}
}
