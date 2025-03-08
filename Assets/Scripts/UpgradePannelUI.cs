using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePannelUI : MonoBehaviour {

    [SerializeField] private Text text;
    [SerializeField] private Text headerText;
    [SerializeField] private Text goldReqText;
    [SerializeField] private Image upgradeImage;
    [SerializeField] private Button upgradeBtn;

    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;
    

    private void Awake() {
        upgradeBtn.onClick.AddListener(() => {
            switch (UIController.Instance.GetUpgradePage()) {
                case 0:
                    Num();
                    break;
                case 1:
                    Click();
                    break;
                default:
                    break;
            }
        });

        leftBtn.onClick.AddListener(()=> { PageDown(); });
        rightBtn.onClick.AddListener(() => { PageUp(); });
       
    }

    private void Start() {
        UpdateVisuals();
    }

    public void PageUp() {
        if (UIController.Instance.GetUpgradePage() != 1) {
            UIController.Instance.IncrementUpgradePage();
            //play clip
            SoundManager.Instance.ChangeSfxClip("Button");
            UpdateVisuals();
        }
    }
    public void PageDown() {
        if (UIController.Instance.GetUpgradePage() != 0) {
            UIController.Instance.DecrementUpgradePage();
            //play clip
            SoundManager.Instance.ChangeSfxClip("Button");
            UpdateVisuals();
        }
    }

    private void UpdateVisuals() {
        int level=0;

        switch (UIController.Instance.GetUpgradePage()) {
            case 0:
                NumGroupChange();
                level = GameManager.Instance.NumLevel;
                break;
            case 1:
                ClickGroupChange();
                level = GameManager.Instance.ClickLevel;
                break;
            default:
                break;
        }

        if(level >=5)
            upgradeBtn.gameObject.SetActive(false);
    }

    private void NumGroupChange() {
        //change the sub text in accordance to numlevel
        headerText.text = "어항 크기";
        upgradeImage.sprite = GameManager.Instance.GetUpgradeFromArray(0);
        goldReqText.text = string.Format("{0:n0}", GameManager.Instance.GetNumGoldFromArray(GameManager.Instance.NumLevel));
        text.text = string.Format("수용량 {0}", GameManager.Instance.NumLevel * 2);

    }

    private void ClickGroupChange() {
        //change CLICK GROUP 
        headerText.text = "클릭 컨트롤";
        upgradeImage.sprite = GameManager.Instance.GetUpgradeFromArray(1);
        goldReqText.text = string.Format("{0:n0}", GameManager.Instance.GetClickGoldFromArray(GameManager.Instance.ClickLevel));
        text.text = string.Format("팥 생산량 {0}", GameManager.Instance.ClickLevel);
    }

    private void Num() {
        //if current gold is higher than req gold to unlock
        if (GameManager.Instance.Gold >= GameManager.Instance.GetNumGoldFromArray(GameManager.Instance.NumLevel)) {
            //subtract req cost from bean
            GameManager.Instance.SetGold(GameManager.Instance.Gold - GameManager.Instance.GetNumGoldFromArray(GameManager.Instance.NumLevel));
            GameManager.Instance.SetNumLevel(GameManager.Instance.NumLevel + 1);
            //play clip
            SoundManager.Instance.ChangeSfxClip("Unlock");
            UpdateVisuals();
            GameManager.Instance.Save();
        }
        else {
            ClipAndMessage("Fail");
        }
    }

    private void Click() {
        //if current gold is higher than req gold to unlock
        if (GameManager.Instance.Gold >= GameManager.Instance.GetClickGoldFromArray(GameManager.Instance.ClickLevel)) {
            //subtract req cost from bean
            GameManager.Instance.SetGold(GameManager.Instance.Gold - GameManager.Instance.GetClickGoldFromArray(GameManager.Instance.ClickLevel));
            GameManager.Instance.SetClickLevel(GameManager.Instance.ClickLevel + 1);
            //play clip
            SoundManager.Instance.ChangeSfxClip("Unlock");
            UpdateVisuals();
            GameManager.Instance.Save();
        }
        else {
            ClipAndMessage("Fail");
        }
    }

    private void ClipAndMessage(string clip) {
        //function that takes two string as arguement to change both played lip and message
        SoundManager.Instance.ChangeSfxClip(clip);
    }
}