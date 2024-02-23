using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UpdateUserNamePanelUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI newUserName;
	[SerializeField] TMP_InputField InputFieldText;
	[SerializeField] TextMeshProUGUI lengthCheck;
	[SerializeField] private Animator PanelAnimator;
	[SerializeField] private Animator BackButtonAnimator;
	[SerializeField] private Animator PanelFadingAnimator;
	private List<string> panelExitAnimationCondition = new List<string>();

	public static UpdateUserNamePanelUI Instance;

	private void Awake()
	{
		Instance = this;
	}
    private void Start()
    {

		panelExitAnimationCondition.Add("IsLeftOut");
		panelExitAnimationCondition.Add("IsRightOut");
		panelExitAnimationCondition.Add("IsBottomOut");

		lengthCheck.gameObject.SetActive(false);
		InputFieldText.text = GlobalData.Instance.UserName;

    }
    
	public static UpdateUserNamePanelUI ShowUI()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		if (Instance == null)
		{
			SoundManager.instance.Play_PANEL_INSTANTIATE_Sound();
			GameObject obj = Instantiate(Resources.Load("Prefabs/UI/UpdateUserNamePanelUI")) as GameObject;
			Canvas[] cans = GameObject.FindObjectsOfType<Canvas>() as Canvas[];
			for (int i = 0; i < cans.Length; i++)
			{
				if (cans[i].gameObject.activeInHierarchy && cans[i].gameObject.tag.Equals("Canvas"))
				{
					obj.transform.SetParent(cans[i].transform, false);
					break;
				}
			}
			Instance = obj.GetComponent<UpdateUserNamePanelUI>();
		}
		return Instance;
	}
	public void OnBackPressed()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		SoundManager.instance.Play_PANEL_DESTROY_Sound();
		StartCoroutine(waitAndDestroy());
	}
	IEnumerator waitAndDestroy()
	{
		PanelAnimator.SetBool(panelExitAnimationCondition[UnityEngine.Random.Range(0, 3)], true);
		BackButtonAnimator.SetBool("IsOut", true);
		PanelFadingAnimator.SetBool("IsOut", true);
		yield return new WaitForSeconds(0.8f);
		Destroy(gameObject);

	}
	public void CheckUserNameLength()
	{
		string name = newUserName.text.TrimEnd();
		//print(name.Length);
		//for(int i=0;i< name.Length;i++)
  //      {
		//	print(name[i]);

  //      }
		if (name.Length >= 3)
		{
			lengthCheck.gameObject.SetActive(false) ;
		}
		//else
		//{
		//	lengthCheck.gameObject.SetActive(true);
		//}
	}
	public void UpdateUserName()
	{
		SoundManager.instance.Play_BUTTON_CLICK_Sound();
		string name = newUserName.text.Trim();
		//int ind = 0;
		//start trim
		bool isStartCopying = false;
		for (int i = 0; i < name.Length; i++)
		{
			if (name[i] != ' ')
			{
				isStartCopying = true;
				name = name.Substring(i, name.Length - 1);
				print("1 length : " + name.Length);
				break;

			}
		}
        for (int i = name.Length - 1; i >= 0; i--)
        {
            if (name[i] != ' ')
            {
				isStartCopying = true;
				name = name.Substring(0, i + 1);
				print("2 length : " + name.Length);

				break;

			}
			if (isStartCopying)
            {
				if(i!= name.Length-1)
                {
					//newUserName.text.Remove(i+1, newUserName.text.Length - (i+1));
		
				}

			}
        }
        print("Length : " + name.Length);
		if (name.Length >= 3 && name.Length <= 12)
		{
			lengthCheck.gameObject.SetActive(false);
			APIManager.Instance.UpdateUserName(name.ToString());
			OnBackPressed();

		}	
		else 
		{
			lengthCheck.gameObject.SetActive(true);
		
		}

	}

}
