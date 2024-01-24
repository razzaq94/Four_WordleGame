using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LogInManager : MonoBehaviour
{
	[SerializeField] Image ProgressBar;
	[SerializeField] TextMeshProUGUI ProgressText;


	public static LogInManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (InGameStorage.Instance.GetUserId() == "")
        {
            LogInPanelUI.ShowUI();
        }
        else
        {
            APIManager.Instance.LogIn();
			OpenGameScene();

        }
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
	IEnumerator LoadScene()
	{
		float progress = 0f;
		//LoadingPanel.SetActive(true);
		string sceneName = "MainMenu";
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
		asyncOperation.allowSceneActivation = false;



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
			ProgressText.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
			ProgressBar.fillAmount = (asyncOperation.progress );

			// Check if the load has finished
			if (asyncOperation.progress >= 0.90f)
			{
				ProgressText.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";
				ProgressBar.fillAmount = (asyncOperation.progress );


				asyncOperation.allowSceneActivation = true;
			}

			yield return null;
		}
	}
}
