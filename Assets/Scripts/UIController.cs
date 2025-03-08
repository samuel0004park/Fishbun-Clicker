using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //create singleton
    public static UIController Instance { get; private set; }

    //variables to control UI
    private bool isLive;
    private int beanPage;
    private int upgradePage;
    private bool pannelDown;


    private Coroutine a;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        isLive = true;
        beanPage = 0;
    }


    public bool GetLive(){
        return isLive;
    }
    public void SetLiveTrue( ) {
        isLive = true;
    }
    public void SetLiveFalse() {
        isLive = false;
    }
    public int GetBeanPage() {
        return beanPage;
    }
    public int GetUpgradePage() {
        return upgradePage;
    }
    public void IncrementUpgradePage() {
        upgradePage++;
    }
    public void DecrementUpgradePage() {
        upgradePage--;
    }
    public void IncrementBeanPage() {
        beanPage++;
    }
    public void DecrementBeanPage() {
        beanPage--;
    }
    public bool GetPannelDown() {
        return pannelDown;
    }

    public void SetPannelDown(bool offset) {
        pannelDown = offset;
    }
 
}
