using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


//Code for Option Pannel (When pressed ESC) 
public class ButtonOption : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private CreditPanel CreditPanel;

    private Image image;
    private bool isClick;

    private void Awake()
    {
        image = GetComponent<Image>();
        isClick = false;
    }
    private void Update()
    {
        //call function if esc is pressed 
        if (Input.GetKeyDown(KeyCode.Escape))
            EscPress();
    }
    private void EscPress()
    {
        //when pressed esc
        //close esc pannel if not live 
        if (!UIController.Instance.GetLive() && isClick)
        {
            GameManager.Instance.Save();
            CloseOptionPanel();
        }
        //open esc pannel if live
        else if (UIController.Instance.GetLive()) 
        {
            //if one of the other pannels are up, then close them and do nothing to ecs pannel
            if (UIController.Instance.GetPannelDown())
            {
                UIController.Instance.SetPannelDown(false);
                SoundManager.Instance.ChangeSfxClip("Button");
                return;
            }
            //if other pannels are all down
            OpenOptionPanel();
        }

    }
    private void OpenOptionPanel()
    {
        Panel.SetActive(true);
        Time.timeScale = 0; //stop scene time
        isClick = true;
        UIController.Instance.SetLiveFalse(); //set live to false
                                              //play clip
        SoundManager.Instance.ChangeSfxClip("Button");
    }
    public void CloseOptionPanel()
    {
        Panel.SetActive(false);
        Time.timeScale = 1f; //set scene time to normal
        isClick = false;
        UIController.Instance.SetLiveTrue(); //set back to live
                                             //play clip
        SoundManager.Instance.ChangeSfxClip("Button");
        CreditPanel.HideVisuals();
    }

    public void EndGame()
    {
        GameManager.Instance.Save();
        Application.Quit();
    }

}

