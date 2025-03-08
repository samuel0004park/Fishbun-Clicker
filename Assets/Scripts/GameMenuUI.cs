using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : MonoBehaviour {

    private void Awake() {
        Show();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            //GameManager.Instance.SetLive();
            Hide();
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

}
