using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeanFishMove : MonoBehaviour
{
    private float pickTime=0f;

    private Camera cam;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;

    //Border Group For PreventFall Function
    private GameObject BorderGroup;
    private Transform TopLeft;
    private Transform BottomRight;

    private int id;
    private int level;
    private float exp;

    private float SpeedX;
    private float SpeedY;
    private bool delay; 
    private bool doMove;
    private bool doDrag;

    public Vector3 mousePos;

    public Coroutine a;
    public enum State { Move, Idle, Turn, Click, Pick}
    public State state { get; private set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        //To find child, Find Parent wil tag and then find child by name
        BorderGroup = GameObject.FindGameObjectWithTag("Border");
        TopLeft = BorderGroup.transform.Find("TopLeft");
        BottomRight = BorderGroup.transform.Find("BottomRight");
    }

    public void InstantiateBeanFish(int page, int lv,float experience, Vector3 point)
    {
        id = page;
        level = lv;
        exp = experience;
        sprite.sprite = GameManager.Instance.GetSpriteFromArray(id);
        transform.position = point;
        state = State.Idle;
        GameManager.Instance.ChangeAc(anim, level);
    }

    private void Update()
    {
        Act();
        if (doMove)
        {
            Translation();
            CheckBorder();
        }
        if (doDrag)
            PickTimer();
        GetMousePoint();
        SetExp();
    }

    //Act decides which functions to call when in a certain state
    private void Act()
    {
        if (state == State.Idle&&delay==false) 
        { 
            IdleState();
        }
        if (state == State.Move && delay == false)
        {
            MoveState();
        }
        if (state == State.Turn && delay == false)
        {
            TurnState();
        }
        if (state == State.Click&& delay == false)
        {
            ClickState();
        }
        if(state == State.Pick && delay == false)
        {
            PickState();
        }
    }

    private void IdleState()
    {
        delay = true;
        
        //set sprite to idle
        anim.SetBool("isWalk", false);

        //wait for 5 seconds 
        a = StartCoroutine(countdown(5f, State.Move));

        return;
    }

    private void MoveState()
    {
        delay = true;
        doMove = true;

        SpeedX = Random.Range(-0.8f, 0.8f);   
        SpeedY = Random.Range(-0.8f, 0.8f);

        //set sprite to walk
        anim.SetBool("isWalk", true);  // set anim to walking

        //remain in move state from 3 to 5 seconds
        a = StartCoroutine(countdown(Random.Range(3f, 5f), State.Idle));

        return;
    }
    private void TurnState()
    {
        delay = true;
        doMove = true;

        //Pick random point from save points
        Vector3 Point = GameManager.Instance.GetPoint();

        //Calculate new speed x and y
        Point = Vector3.Normalize(Point - transform.position) * 0.3f;
        SpeedX = Point.x;
        SpeedY = Point.y;

        //remain in turn state from 3 to 5 seconds
        a = StartCoroutine(countdown(Random.Range(3f,5f),State.Idle));

        return;
    }

    private void ClickState()
    {
        delay = true;
        pickTime = 0f;
        doDrag = true;

        FeverManager.Instance.IncrementFeverCount();

        GetBeanPaste();
        exp = exp + 1;

        SoundManager.Instance.ChangeSfxClip("Touch");

        anim.SetTrigger("doTouch");
        anim.SetBool("isWalk", false);

        //remain in click state from 2 to 4 seconds
        a = StartCoroutine(countdown(Random.Range(2f, 4f), State.Move));

        return;
    }

    private void PickTimer()
    {
        pickTime += Time.deltaTime;

        if (pickTime > 0.5f) 
        {
            ForceStop(State.Pick);
        }
    }
  
    private void PickState()
    {
        //Debug.Log("Pick");
        transform.position = mousePos;
    }

   
    private void Translation()
    {
        //move jelly with given speed
        Vector3 offset = new Vector3(SpeedX * Time.deltaTime,SpeedY * Time.deltaTime, SpeedY * Time.deltaTime);
        transform.position = offset + transform.position;

        //flip sprite if -xspeed
        if (SpeedX < 0)                        
            sprite.flipX = true;
        else
            sprite.flipX = false;

    }
    private void CheckBorder()
    {
        float x_axis=transform.position.x;
        float y_axis=transform.position.y;
        //if meet border, force stop
        if ((x_axis> BottomRight.position.x)||(x_axis<TopLeft.position.x)||
            (y_axis< BottomRight.position.y)||(y_axis>TopLeft.position.y))
        {
            ForceStop(State.Turn);   
        }
    }

    private void ForceStop(State nextState)
    {
        StopCoroutine(a);
        delay = false;
        doMove = false;
        state = nextState;
    } 
  
    IEnumerator countdown(float waitTime, State nextState)
    {
        yield return new WaitForSeconds(waitTime);
        state = nextState;
        delay = false;
        doMove = false;
    } 

    public void GetBeanPaste()
    {
        
        int offset = ((id + 1) * level * GameManager.Instance.ClickLevel * FeverManager.Instance.GetFeverLevel()) + GameManager.Instance.Jelatine;
        //save incremented jelatin value (max is 99999999)
        GameManager.Instance.SetJelatine(Mathf.Min(offset, 99999999));
        
    }

    private void SetExp()
    {
        //add exp if lower than level 3
        if (level == 3)
            return;
        else
        {
            exp = exp + Time.deltaTime;
            if (exp < level * 50)
                return;
            else //level up if max exp
            {
                level++;
                //change the size with animation controller
                GameManager.Instance.ChangeAc(anim,level);
                //play clip
                SoundManager.Instance.ChangeSfxClip("Grow");
                GameManager.Instance.Save();
            }
        }
    } 

    private void GetMousePoint()
    {
        Vector3 temp = Input.mousePosition;

        Vector2 xy_position = cam.ScreenToWorldPoint(new Vector2(temp.x, temp.y));

        mousePos = new Vector3(xy_position.x, xy_position.y, xy_position.y);
    }  
    
    
    public void PreventFall()
    {
        //function that transforms positon of jelly if at cliff 
        float x_axis = transform.position.x;
        float y_axis = transform.position.y;

        if ((x_axis > BottomRight.transform.position.x) || (x_axis < TopLeft.transform.position.x) ||
            (y_axis < BottomRight.transform.position.y) || (y_axis > TopLeft.transform.position.y))
        {

            Vector3 Point = GameManager.Instance.GetPoint();
            transform.position = Point;   //meet border, force stop
            SoundManager.Instance.ChangeSfxClip("Fail");
        }
    }

    public void OnMouseDown()
    {
        //if game is not live, return 
        if (UIController.Instance.GetLive() == false)
            return;
        StopCoroutine(a); //end whatever state
        state = State.Click; //enter click state
        doMove = false; 
        delay = false;
    }

    public void OnMouseUp()
    {
        doDrag = false; //no longer drag bean fish
        state = State.Idle; //bean fish should return back to idle

        if (GameManager.Instance.IsSell) //check if at sellBtn
        {
            GetGold();
            SoundManager.Instance.ChangeSfxClip("Sell");
            GameManager.Instance.RemoveJelly(this);
        }
        else
            PreventFall(); //check if at border
        
    }

    private void GetGold() //incrememt gold after selling 
    {
        int gold=GameManager.Instance.GetGoldFromArray(id);

        int offset = (gold * level)+ GameManager.Instance.Gold;
        offset= Mathf.Min(offset, 99999999);

        GameManager.Instance.SetGold (offset);
    }

    public int GetBeanFishId() {
        return id;
    }

    public int GetBeanFishLevel() {
        return level;
    }

    public float GetBeanFishExp() {
        return exp;
    }
    
}
