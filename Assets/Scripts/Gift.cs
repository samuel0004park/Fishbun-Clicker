using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    private const int GOLD_GIFT = 0;
    private const int JELLY_GIFT = 1;
    private int giftAmount;
    private int giftId;

    private bool isUsed;

    [SerializeField] private Animator giftAnim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void InstantiateGift(int giftId, int giftAmount) {
        spriteRenderer.sprite = GiftManager.Instance.GetSpriteFromList(giftId);
        this.giftAmount = giftAmount * GameManager.Instance.ClickLevel;
        this.giftId = giftId;
        StartCoroutine(DeleteGiftCoroutine1());
    }

    IEnumerator DeleteGiftCoroutine1() {
        yield return new WaitForSeconds(10f);
        giftAnim.SetTrigger("doTouch");
        Destroy(gameObject);
    }

    public void OnMouseDown() {
        if (isUsed)
            return;

        isUsed = true;

        if (giftId == GOLD_GIFT)
            GetGold();
        else if (giftId == JELLY_GIFT)
            GetJelatine();

        SoundManager.Instance.ChangeSfxClip("Unlock");
        giftAnim.SetTrigger("doTouch");
        StartCoroutine(MouseDownCoroutine());
    }
    
    IEnumerator MouseDownCoroutine() {
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.Save();
        Destroy(gameObject);
    }


    private void GetGold(){
        int offset = giftAmount + GameManager.Instance.Gold;
        offset = Mathf.Min(offset, 99999999);
        GameManager.Instance.SetGold(offset);
    }

    private void GetJelatine() {
        int offset = giftAmount + GameManager.Instance.Jelatine;
        offset = Mathf.Min(offset, 99999999);
        GameManager.Instance.SetJelatine(offset);
    }
}
