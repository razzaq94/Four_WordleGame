using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("----- AUDIO SOURCES -----")]
    [Space(12)]
    public AudioSource asrc;
    public AudioSource music_audioSource;

    [Header("----- AUDIO CLIPS -----")]
    [Space(12)]
    [SerializeField] private AudioClip buttonClick_Clip;
    [SerializeField] private AudioClip panelInstantiate_Clip;
    [SerializeField] private AudioClip panelDestroy_Clip;
    [SerializeField] private AudioClip ValidWord_Clip;
    [SerializeField] private AudioClip InvalidWord_Clip;
    [SerializeField] private AudioClip levelFailed_Clip;
    [SerializeField] private AudioClip victory_Clip;
    [SerializeField] private AudioClip CountDown_Clip;

    [SerializeField] private AudioClip smallEnemy_Clip;
    [SerializeField] private AudioClip playerHurt_Clip;
    [SerializeField] private AudioClip monsterHurt_Clip;
    [SerializeField] private AudioClip petDestroy_Clip;

    //[Header("----- AUDIO BUTTONS -----")]
    //[Space(4)]
    //[SerializeField] private GameObject sfxOff;
    //[SerializeField] private GameObject sfxOn;
    //[SerializeField] private GameObject musicOn;
    //[SerializeField] private GameObject musicOff;


    //[SerializeField] private GameObject vibrationOn;
    //[SerializeField] private GameObject vibrationOff;
    //[SerializeField] private GameObject keyboardOn;
    //[SerializeField] private GameObject keyboardOff;

    //[SerializeField] private GameObject ChallengekeyboardOn;
    //[SerializeField] private GameObject ChallengekeyboardOff;
    //[SerializeField] private GameObject PuzzlekeyboardOn;
    //[SerializeField] private GameObject PuzzlekeyboardOff;


    private int vibrationIntensity = 0;


    void Awake()
    {
        //  instance = this;
        if (instance == null)
        {
            instance = this; // In first scene, make us the singleton.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject); // On reload, singleton already set, so destroy duplicate.
    }


    void Start()
    {
        Set_SOUNDS_STATUS();
    }


    public void Set_SOUNDS_STATUS()
    {
        asrc.volume = ((float)PlayerPrefs.GetInt("SFX", 50)) / 100f;
        music_audioSource.volume = ((float)PlayerPrefs.GetInt("MUSIC", 50)) / 100f;
        vibrationIntensity = PlayerPrefs.GetInt("VIBRATE", 500) ;
    }
    public void Change_Music_Volume(float music)
    {
        music_audioSource.volume = music;
        int sfx_music = (int)(music * 100);
        PlayerPrefs.SetInt("MUSIC", sfx_music);

    }
    public void Change_SFX_Volume(float sfx)
    {
        asrc.volume = sfx;
        int sfx_volume = (int)(sfx * 100);
        PlayerPrefs.SetInt("SFX", sfx_volume);
    }
    public void Change_VIBRATE_VOLUME(float vbr)
    {
        vibrationIntensity =(int)(vbr*1000);
      //  int sfx_volume = (int)(sfx * 100);
        PlayerPrefs.SetInt("VIBRATE", vibrationIntensity);
    }
    public void Play_BUTTON_Vibrate()
    {


        Vibration.Vibrate((int)vibrationIntensity);

    }



    public void Play_BUTTON_CLICK_Sound()
    {
        asrc.PlayOneShot(buttonClick_Clip);

    }
    public void Play_PANEL_INSTANTIATE_Sound()
    {
        asrc.PlayOneShot(panelInstantiate_Clip);

    } 
    public void Play_PANEL_DESTROY_Sound()
    {
        asrc.PlayOneShot(panelDestroy_Clip);
    }
    
    

    public void Play_VALID_WORD_Sound()
    {
        asrc.PlayOneShot(ValidWord_Clip);

    }
    public void Play_INVALID_WORD_Sound()
    {
        asrc.PlayOneShot(InvalidWord_Clip);

    }
    public void Play_SMALL_ENEMY_Sound()
    {
        asrc.PlayOneShot(smallEnemy_Clip);

    }
    public void Play_VICTORY_COMPLETE_Sound()
    {
        asrc.PlayOneShot(victory_Clip);

    }
    public void Play_LEVEL_FAILED_Sound()
    {
        asrc.PlayOneShot(levelFailed_Clip);

    }
    public void Play_COUNT_DOWN_Sound()
    {
        asrc.PlayOneShot(CountDown_Clip);

    }
    bool canMonsterPLayHurtSound = true;
    public void Play_MONSTER_HURT_Sound()
    {
        if (canMonsterPLayHurtSound == true)
        {
            canMonsterPLayHurtSound = false;
            asrc.PlayOneShot(monsterHurt_Clip);
            StartCoroutine(WaitAndPlayHurtSoundAgain());
        }
    }

    IEnumerator WaitAndPlayHurtSoundAgain()
    {
        yield return new WaitForSeconds(1f);
        canMonsterPLayHurtSound = true;
    }



    public void Play_PET_DESRTOY_Sound()
    {
        asrc.PlayOneShot(petDestroy_Clip);
    }


    public bool isMusicOn()
    {
        if (PlayerPrefs.GetInt("MUSIC", 0) == 0)
        {
            // MUSIC OFF
            return false;
        }
        else
        {
            // MUSIC ON
            return true;
        }
    }


    public bool isSfxOn()
    {
        if (PlayerPrefs.GetInt("SFX", 0) == 0)
        {
            // SFX OFF
            return false;
        }
        else
        {
            // SFX ON
            return true;
        }
    }

    public bool isVibrateOn()
    {
        if (PlayerPrefs.GetInt("VIBRATE", 0) == 0)
        {
            // Vibrate OFF
            return false;
        }
        else
        {
            // Vibrate ON
            return true;
        }
    }
   
   



   

   
  




    public void OnClick_SETTINGS()
    {
        Time.timeScale = 0f;
    }



    public void OnClick_SETTINGS_BACK()
    {
        Time.timeScale = 1f;

    }


    public void InitializeButtons()
    {
    }
    public void ExitSettingPanel()
    {
       // MainMenuManager.Instance.ExitSettingPanel();
    }


}