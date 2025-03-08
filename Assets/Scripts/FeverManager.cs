using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour
{
    
    public static FeverManager Instance { get; private set; }

    [SerializeField] private float feverCount;
    [SerializeField] private float level1_feverRequire = 30f;
    [SerializeField] private float level2_feverRequire = 60f;
    [SerializeField] private float feverMax = 100f;

    [SerializeField] private int feverLevel;

    [SerializeField] private Slider feverBar;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        InstantiateFeverBar();
    }
    private void InstantiateFeverBar() {
        feverLevel = 1;
        feverBar.value = 0f;
        feverBar.maxValue = feverMax;
    }

    private void Update() {

        switch (feverLevel) {
            case 1:
                CoolDown(2f);
                if(feverCount > level1_feverRequire) {
                    feverLevel++;
                }
                break;
            case 2:
                CoolDown(5f);
                if (feverCount < level1_feverRequire) {
                    feverLevel--;
                }
                if(feverCount > level2_feverRequire) {
                    feverLevel++;
                }
                break;
            case 3:
                CoolDown(7f);
                if (feverCount < level1_feverRequire) {
                    feverLevel--;
                }
                break;
        }
        feverBar.value = feverCount;
    }

    private void CoolDown(float multiplier) {
        if (feverCount > 0) {
            feverCount -= Time.deltaTime * multiplier;
        }
        if(feverCount < 0) {
            feverCount = 0;
        }
    }

    public void IncrementFeverCount() {
        feverCount++;
    }

    public int GetFeverLevel() {
        return feverLevel;
    }
}
