using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMChooseUI : MonoBehaviour
{
    [SerializeField] private Text bgmNameText;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private void Start() {
        SetBGMText(SoundManager.Instance.GetCurrentBGMName());
        leftButton.onClick.AddListener(()=> {
            SoundManager.Instance.ChangeBGMClip_DOWN();
            SetBGMText(SoundManager.Instance.GetCurrentBGMName());
        });
        rightButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeBGMClip_UP();
            SetBGMText(SoundManager.Instance.GetCurrentBGMName());
        });
    }

    private void SetBGMText(string text) {
        bgmNameText.text = text;
    }

}
