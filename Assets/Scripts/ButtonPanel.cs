using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//Script that manages pannel button controls
public class ButtonPanel : MonoBehaviour
{
    [SerializeField] private Sprite ShowSprite;
    [SerializeField] private Sprite HideSprite;
    [SerializeField] private GameObject Panel;

    public UnityEvent hideAnother;

    private bool isClick; //var that manages if there is an active pannel
    private Animator ani;
    private Image image;

    private void Awake()
    {
        ani = Panel.GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        //if esc is pressed, and a pannel is up, close it 
        if (Input.GetKeyDown(KeyCode.Escape) && isClick)
        {
            Hide();
            UIController.Instance.SetPannelDown(true);
            UIController.Instance.SetLiveTrue();
        }
    }

    public void Hide()
    {
        ani.SetTrigger("doHide");
        image.sprite = HideSprite;
        isClick = false;
    } //function that hides pannel 

    public void Show()
    {
        ani.SetTrigger("doShow");
        image.sprite = ShowSprite;
        isClick = true;
    } //function that shows pannel

    public void OnClick()
    {
        //if pannel is not active
        if (!isClick)
        {
            //and it is clicked in neutral state, show it and set scene to live to false
            if (UIController.Instance.GetLive() == true)
            {
                Show();
                UIController.Instance.SetLiveFalse();
            }
            //if clicked in when another pannel is up, close other pannel and show this one 
            else
            {
                hideAnother.Invoke();
                Show();
                UIController.Instance.SetLiveFalse();
            }
        }
        //if pannel is active, hide it and set scene to live
        else if (isClick)
        {
            Hide();
            UIController.Instance.SetLiveTrue();
        }
        SoundManager.Instance.ChangeSfxClip("Button");
    }

}
