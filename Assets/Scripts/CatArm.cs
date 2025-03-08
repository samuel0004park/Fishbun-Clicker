using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatArm : MonoBehaviour
{
    private enum Position { TOP,BOTTOM,LEFT,RIGHT}
    private Position startingPosition;
    [SerializeField] private Vector3[] spawnPoints;
    [SerializeField] private Animator animator;

    private bool isCatched;
    private LayerMask entityLayerMask;
    private void Awake() {
        entityLayerMask = LayerMask.GetMask("Entity");
    }

    public void SetDirectionOfAttack() {
        int randomDirection = Random.Range(0, 4);
        float randomOffset;

        switch (randomDirection) {
            case 0:
                startingPosition = Position.TOP;
                randomOffset = Random.Range(-3,3);
                transform.position = new Vector3(spawnPoints[randomDirection].x + randomOffset, spawnPoints[randomDirection].y,1);
                break;
            case 1:
                startingPosition = Position.BOTTOM;
                randomOffset = Random.Range(-3, 3);
                transform.position = new Vector3(spawnPoints[randomDirection].x + randomOffset, spawnPoints[randomDirection].y, 1);
                break;
            case 2:
                startingPosition = Position.LEFT;
                randomOffset = Random.Range(-1, 1);
                transform.position = new Vector3(spawnPoints[randomDirection].x, spawnPoints[randomDirection].y + randomOffset, 1);
                break;
            case 3:
                startingPosition = Position.RIGHT;
                randomOffset = Random.Range(-1, 1);
                transform.position = new Vector3(spawnPoints[randomDirection].x, spawnPoints[randomDirection].y + +randomOffset, 1);
                break;
        }
    }

    private void OnMouseDown() {
        isCatched = true;
        SoundManager.Instance.ChangeSfxClip("Touch");
        animator.SetTrigger("doTouch");
        Destroy(gameObject,0.5f);
    }
    private void Update() {
        if (isCatched)
            return;

        switch (startingPosition) {
            case Position.TOP:
                transform.Translate(Vector3.down * Time.deltaTime *2);
                if (transform.position.y < 0) {
                    ReachedDestination();
                }
                break;
            case Position.BOTTOM:
                transform.Translate(Vector3.up * Time.deltaTime * 2);
                if (transform.position.y > 0) {
                    ReachedDestination();
                }
                break;
            case Position.LEFT:
                transform.Translate(Vector3.right * Time.deltaTime * 2);
                if (transform.position.x > 0) {
                    ReachedDestination();
                }
                break;
            case Position.RIGHT:
                transform.Translate(Vector3.left * Time.deltaTime * 2);
                if (transform.position.x < 0) {
                    ReachedDestination();
                }
                break;
        }
        TryCatchSlime();
    }

    private void ReachedDestination() {
        Destroy(gameObject);
    }

    private void TryCatchSlime() {
        RaycastHit2D[] targets = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.zero, 0, entityLayerMask);

        if (targets.Length == 0) {
            return;
        }

        BeanFishMove jelly = targets[0].transform.GetComponent<BeanFishMove>();
        if (jelly != null) {
            GameManager.Instance.RemoveJelly(jelly);
            ReachedDestination();
        }
 
    }

}
