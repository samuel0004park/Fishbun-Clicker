using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAlert : MonoBehaviour
{

    [SerializeField] private GameObject catArmPrefab;
    
    private float catTimer;
    private float catTimerMax = 60f;
    private float catTimerMin = 30f;

    private void Awake() {
        catTimer = Random.Range(catTimerMin, catTimerMax);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            SpawnCatArm();
        }
        catTimer -= Time.deltaTime;
        if(catTimer <= 0f) {
            catTimer = Random.Range(catTimerMin, catTimerMax);
            SpawnCatArm();
        }

    }

    private void SpawnCatArm() {
        GameObject catArmInstance = Instantiate(catArmPrefab, transform);
        catArmInstance.GetComponent<CatArm>().SetDirectionOfAttack();
    }
}
