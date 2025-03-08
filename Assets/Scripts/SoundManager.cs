using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource sfxPlayer;
    [SerializeField] private AudioSource bgmPlayer;

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    [SerializeField] private AudioClip[] bgmClips;

    private int currentBGMClipIndex;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        SetSavedBGM();
        sfxPlayer.volume = sfxSlider.value = PlayerPrefs.GetFloat("SFX");
        bgmPlayer.volume = bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        //add listener to both sliders
        sfxSlider.onValueChanged.AddListener(delegate { SFXValueChangeCheck(); });
        bgmSlider.onValueChanged.AddListener(delegate { BGMValueChangeCheck(); });
    }

    private void SetSavedBGM() {
        if (PlayerPrefs.HasKey("BGM_Clip")) {
            currentBGMClipIndex = PlayerPrefs.GetInt("BGM_Clip");
        }

        bgmPlayer.clip = bgmClips[currentBGMClipIndex];
        bgmPlayer.Play();
    }

    public string GetCurrentBGMName() {
        return bgmClips[PlayerPrefs.GetInt("BGM_Clip")].name;
    }

    public void ChangeBGMClip_UP() {
        currentBGMClipIndex = (currentBGMClipIndex + 1) % bgmClips.Length;
        PlayerPrefs.SetInt("BGM_Clip", currentBGMClipIndex);
        bgmPlayer.clip = bgmClips[currentBGMClipIndex];
        bgmPlayer.Play();
    }

    public void ChangeBGMClip_DOWN() {
        currentBGMClipIndex = (currentBGMClipIndex - 1 + bgmClips.Length) % bgmClips.Length;
        PlayerPrefs.SetInt("BGM_Clip", currentBGMClipIndex);
        bgmPlayer.clip = bgmClips[currentBGMClipIndex];
        bgmPlayer.Play();
    }

    public void SetSFXVolume(float volume) {
        sfxPlayer.volume = volume;
        SFXValueChangeCheck();
    }

    public void SetBGMVolume(float volume) {
        bgmPlayer.volume = volume;
        BGMValueChangeCheck();
    }

    public void ChangeSfxClip(string sfx)
    {
        //get string as arguement and load correct audio 
        switch (sfx)
        {
            case "Touch":
                sfxPlayer.clip = Resources.Load("Audio/"+sfx) as AudioClip;
                break;
            case "Grow":
                sfxPlayer.clip = Resources.Load("Audio/" + sfx) as AudioClip;
                break;
            case "Sell":
                sfxPlayer.clip = Resources.Load("Audio/" + sfx) as AudioClip;
                break;
            case "Buy":
                sfxPlayer.clip = Resources.Load("Audio/" + sfx) as AudioClip;
                break;
            case "Unlock":
                sfxPlayer.clip = Resources.Load("Audio/" + sfx) as AudioClip;
                break;
            case "Fail":
                sfxPlayer.clip = Resources.Load("Audio/" + sfx) as AudioClip;
                break;
            case "Button":
                sfxPlayer.clip = Resources.Load("Audio/" + sfx) as AudioClip;
                break;
            case "Pause In":
                sfxPlayer.clip = Resources.Load("Audio/" + sfx) as AudioClip;
                break;
            case "Pause Out":
                sfxPlayer.clip = Resources.Load("Audio/" + sfx) as AudioClip;
                break;
            default:
                break;
        }sfxPlayer.Play();
    }

 

    private void SFXValueChangeCheck()
    {
        sfxPlayer.volume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        //Debug.Log(sfxSlider.value);
    }
    private void BGMValueChangeCheck()
    {
        bgmPlayer.volume = bgmSlider.value;
        PlayerPrefs.SetFloat("BGM", bgmSlider.value);
    }
    private void Exit()
    {
        //save everything 
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("BGM", bgmSlider.value);
        GameManager.Instance.Save();
        //play clip
        ChangeSfxClip("Button");
        //do upscaled timer for 0.5 seconds
        StartCoroutine(UnscaledTimer());
        //Exit game
        Application.Quit();
    }

    IEnumerator UnscaledTimer()
    {
        yield return new WaitForSecondsRealtime(0.5f);
    }
 
    
}
