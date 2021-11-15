using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerBase : MonoBehaviour
{
    private const float MAX_SPEED = 5;
    private const float MIN_SPEED = 3;
    private const int MAX_POWER = 13;
    private const int MIN_POWER = 3;


    [SerializeField] private float DEFAULT_WIND_RES = 0;
    [SerializeField] private float speed = 1;
    [SerializeField] private EventTrigger ET = null;
    private Rigidbody2D playerRigid;
    private IEnumerator addPower;
    private int direction = 0;
    private int jumpPower = 3;
    private int powerRecord = 3;
    [SerializeField] private float windRes = 0f;
    [SerializeField] private float windPow = 1f;
    private float windTime = 0;

    private bool isJumping = false;
    private bool isCharging = false;
    private bool isMove = false;
    private bool isHold = false;
    internal bool isDead = false;
    internal bool isCrowdExit = false;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();

        addPower = AddPower();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;

        pointerDown.callback.AddListener((e) => {
            StartCoroutine(addPower); });
        ET.triggers.Add(pointerDown);
        StartCoroutine(WindCycle());
    }
    
    void Update()
    {
        windPow = 12;
        if (isCharging)
        {
            speed = MIN_SPEED;
        }
        else
        {
            speed = MAX_SPEED;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(-1);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(1);
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            Move(0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interaction(FindClosestGameObject());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            
        }
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.8f, Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * 2.3f, Color.blue);
        if (playerRigid.velocity.y <= 0)
        {
            // 점프
            RaycastHit2D jumpRay = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Floor"));
            if(jumpRay.collider != null)
            {
                if (jumpRay.distance < 0.8f)
                {
                    isJumping = false;
                }
            }
        }
        // 애가 가로등 뭐시기
        RaycastHit2D objectRay = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, LayerMask.GetMask("Floor"));
        if (objectRay.collider != null)
        {
            if (objectRay.distance < 2.3f)
            {
                if(playerRigid.velocity.x >= 0)
                    windRes = windPow;
            }
        }
        else
        {
            if (isJumping)
            {
                windRes = DEFAULT_WIND_RES/powerRecord + 8;
            }
            else if (!isCharging)
                windRes = DEFAULT_WIND_RES;
        }

        //Debug.Log(Mathf.Pow((110 - Vector2.Distance(transform.position, windPosition)) * 0.07f, 2) * windRes * 0.01f);
        if (GameManager.Instance.gameMode != 1)
        {
            windRes = windPow = 0;
        } 
        if(isDead)
        {
            playerRigid.velocity = Vector2.zero;
            Debug.Log("Game Over");
        }
        else if (isHold)
        {
            playerRigid.velocity = new Vector2(0, direction * speed);
        }
        else if(isCrowdExit)
        {
            StartCoroutine(PlayerBouncesOff());
        }
        else
            playerRigid.velocity = new Vector2(direction * speed - ((windPow - windRes) * Mathf.Sin((2 * Mathf.PI * windTime / 5) - Mathf.PI / 2) + (windPow - windRes)), playerRigid.velocity.y);
    }

    private IEnumerator PlayerBouncesOff()
    {
        playerRigid.gravityScale = 0;
        playerRigid.AddForce(new Vector2(-1, 1) * Time.deltaTime * 30, ForceMode2D.Impulse);
        transform.DORotate(new Vector3(0, 0, 180), .3f, RotateMode.Fast).SetLoops(-1, LoopType.Incremental);
        yield return new WaitForSeconds(1);
        isCrowdExit = false;
        isDead = true;
    }

    public void Move(int dir)
    {
        if (isMove) return;
        direction = dir;
    }
    
    private IEnumerator AddPower()
    {
        while (true)
        {
            if (!isJumping)
            {
                isCharging = true;
                windRes = DEFAULT_WIND_RES;
                if (jumpPower < MAX_POWER)
                    jumpPower++;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator WindCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            for (windTime = 0; windTime <= 5; windTime += 0.1f)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }   
    }

    public void Jump()
    {
        StopCoroutine(addPower);
        powerRecord = jumpPower;
        isCharging = false;
        if (isJumping) return;
        isJumping = true;
        playerRigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        jumpPower = MIN_POWER;
    }

    private GameObject FindClosestGameObject()
    {
        Collider2D[] allColliderInRange = Physics2D.OverlapCircleAll(playerRigid.transform.position, 1.5f, LayerMask.GetMask("InteractionObject"));

        if (allColliderInRange.Length <= 0) return null;

        float distance = 0f;
        float leastDis = 0f;
        int closestCol = 0;
        for(int i = 0; i < allColliderInRange.Length; i++)
        {
            distance = Vector2.Distance(transform.position, allColliderInRange[0].transform.position);
            if (distance < leastDis)
            {
                leastDis = distance;
                closestCol = i;
            }
        }

        Debug.Log(allColliderInRange[closestCol].name);
        return allColliderInRange[closestCol].gameObject;
    }

    private void Interaction(GameObject targetObject)
    {
        if (targetObject == null) return;
        switch (targetObject.tag)
        {
            case "Street_Light":
                isHold = !isHold;
                playerRigid.gravityScale = playerRigid.gravityScale == 0 ? 1.5f : 0;
                transform.position = new Vector2(targetObject.transform.position.x, transform.position.y);
                break;
        }
    }
}
