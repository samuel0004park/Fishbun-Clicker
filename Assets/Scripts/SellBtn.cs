using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SellBtn : MonoBehaviour
{
    public void OnClick()
    {
        SoundManager.Instance.ChangeSfxClip("Button");
    }
    public void DoSell()
    {
        GameManager.Instance.IsSell = true;
    }
    public void DoNotSell()
    {
        GameManager.Instance.IsSell = false;
    }
}
