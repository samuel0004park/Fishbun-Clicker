using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellyPannelUI : MonoBehaviour {

    [SerializeField] private Text beanFishText;
    [SerializeField] private Text goldText;
    [SerializeField] private Image unlockImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private GameObject lockGroup;
    [SerializeField] private Text beanText_Unlock;
    [SerializeField] private Text beanDescriptionText;

    [SerializeField] private Button pageLeftButton;
    [SerializeField] private Button pageRightButton;
    [SerializeField] private Button unlockButton;
    [SerializeField] private Button buyButton;

    private void Awake() {
        pageLeftButton.onClick.AddListener(()=> {
            PageDown();
        });
        pageRightButton.onClick.AddListener(() => {
            PageUp();
        });
        unlockButton.onClick.AddListener(() => {
            Unlock();
        });
        buyButton.onClick.AddListener(() => {
            Buy();
        });

    }

    private void Start() {
        UpdateBeanFishPannel();   
    }

    private void UpdateBeanFishPannel() {
        LockGroupChange();
        UnlockGroupChange();
    }

    private void LockGroupChange() {
        int page = UIController.Instance.GetBeanPage();
        //change sprite and set native size
        unlockImage.sprite = GameManager.Instance.GetSpriteFromArray(page);
        unlockImage.SetNativeSize();

        //change beanfish
        beanFishText.text = GameManager.Instance.GetNameFromArray(page);
        beanDescriptionText.text = GameManager.Instance.GetDescriptionFromArray(page);

        //change bean gold and page text
        goldText.text = string.Format("{0:n0}", GameManager.Instance.GetGoldFromArray(page)); //format string: comma for 1000, with no decimal
    }

    private void UnlockGroupChange() {
        int page = UIController.Instance.GetBeanPage();

        //set active if not unlocked
        bool unlocked = GameManager.Instance.GetUnlockFromArray(page);
        lockGroup.SetActive(!unlocked);
        unlockImage.gameObject.SetActive(unlocked);

        //change sprite and set native size
        lockImage.sprite = GameManager.Instance.GetSpriteFromArray(page);
        lockImage.SetNativeSize();

        //change beanfish beanpaste and page text
        beanText_Unlock.text = string.Format("{0:n0}", GameManager.Instance.GetBeanPasteFromArray(page)); //format string: comma for 1000, with no decimal
    }

    private void Unlock() {
        int page = UIController.Instance.GetBeanPage();

        //if current jelatine is higher than req jelatin to unlock
        if (GameManager.Instance.Jelatine >= GameManager.Instance.GetBeanPasteFromArray(page)) {
            //subtract req cost from bean fish
            GameManager.Instance.SetJelatine(GameManager.Instance.Jelatine - GameManager.Instance.GetBeanPasteFromArray(page));
            GameManager.Instance.SetUnlockInArray(page);

            //check if all items are unlocked BeanUnlockedList 
            foreach (bool x in GameManager.Instance.GetUnlockArray()) {
                //if there is an item, which is not unlocked
                if (x == false) {
                    //play unlocked clip
                    SoundManager.Instance.ChangeSfxClip("Unlock");
                    UpdateBeanFishPannel();
                    return;
                }
            }
            //else play clear clip and show clear message
            ClipAndMessage("Clear");
            UpdateBeanFishPannel();
        }
        else {
            ClipAndMessage("Fail");
        }
        GameManager.Instance.Save();
    }

    private void Buy() {
        int page = UIController.Instance.GetBeanPage();

        //if current gold is higher than req gold to unlock
        if (GameManager.Instance.Gold >= GameManager.Instance.GetGoldFromArray(page) && GameManager.Instance.GetBeanList().Count < GameManager.Instance.NumLevel * 2) {
            //subtract req cost from beanpaste
            GameManager.Instance.SetGold(GameManager.Instance.Gold - GameManager.Instance.GetGoldFromArray(page));
            Spawner.Instance.SpawnBeanFish(page, 1, 0f);
            UpdateBeanFishPannel();
        }
        else {
            //not enough gold
            if (GameManager.Instance.Gold < GameManager.Instance.GetGoldFromArray(page)) {
                ClipAndMessage("Fail");
            }
            //not enough num 
            else {
                ClipAndMessage("Fail");
            }
        }
    }
    public void PageUp() {
        if (UIController.Instance.GetBeanPage() != 11) {
            UIController.Instance.IncrementBeanPage();
            //play clip
            SoundManager.Instance.ChangeSfxClip("Button");
            UpdateBeanFishPannel();
        }
    }
    public void PageDown() {
        if (UIController.Instance.GetBeanPage() != 0) {
            UIController.Instance.DecrementBeanPage();
            //play clip
            SoundManager.Instance.ChangeSfxClip("Button");
            UpdateBeanFishPannel();
        }
    }
    private void ClipAndMessage(string clip) {
        //function that takes two string as arguement to change both played lip and message
        SoundManager.Instance.ChangeSfxClip(clip);
    }
}