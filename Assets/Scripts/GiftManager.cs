using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftManager : MonoBehaviour
{
    public static GiftManager Instance { get; private set; }

    [SerializeField] private GameObject giftPrefab;
    [SerializeField] private Sprite[] giftSpriteList;

    private float giftDropTimer;
    private const float giftDropTimerMax = 60f;
    private const float giftDropTimerMin = 30f;

    private const int giftAmountMax = 1000;
    private const int giftAmountMin = 300;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        giftDropTimer = Random.Range(giftDropTimerMin,giftDropTimerMax);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            //DroGift();
        }
        giftDropTimer -= Time.deltaTime;
        if(giftDropTimer <= 0f) {
            giftDropTimer = Random.Range(giftDropTimerMin, giftDropTimerMax);
            DroGift();
        }
    }

    private void DroGift() {
        Vector3 dropPoint = GameManager.Instance.GetPoint();

        GameObject giftGameObject = Instantiate(giftPrefab, transform);

        int giftIndex = Random.Range(0, 2);
        int giftAmount = Random.Range(giftAmountMin, giftAmountMax);

        giftGameObject.GetComponent<Gift>().InstantiateGift(giftIndex, giftAmount) ;

        giftGameObject.transform.position = dropPoint;

    }

    public Sprite GetSpriteFromList(int index) {
        return giftSpriteList[index];
    }
}
