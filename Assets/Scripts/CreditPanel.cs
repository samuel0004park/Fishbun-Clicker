using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;

    public void ShowVisuals() {
        CanvasGroup.alpha = 1f;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;

    }
    public void HideVisuals() {
        CanvasGroup.alpha = 0f;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
    }
}
