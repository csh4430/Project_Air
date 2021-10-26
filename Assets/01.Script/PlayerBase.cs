using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBase : MonoBehaviour
{
    private const float MAX_WIND_RES = 5f;
    private const float MIN_WIND_RES = 1F;

    [SerializeField] private float speed = 1;
    [SerializeField] private EventTrigger ET = null;
    [SerializeField] private Vector2 windPosition;
    private Rigidbody2D playerRigid;
    private IEnumerator addPower;
    private int direction = 0;
    private int jumpPower = 3;
    private int powerRecord = 3;
    private float windRes = MAX_WIND_RES;

    private bool isJumping = false;
    private bool isCharging = false;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();

        addPower = AddPower();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;

        pointerDown.callback.AddListener((e) => {
            StartCoroutine(addPower); });
        ET.triggers.Add(pointerDown);
    }
    
    void Update()
    {
        if (isCharging)
        {
            speed = 1;
        }
        else
        {
            speed = 5;
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
            StartCoroutine(addPower);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jump();
        }

        Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.8f, Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * 1.3f, Color.blue);
        if (playerRigid.velocity.y <= 0)
        {
            RaycastHit2D jumpRay = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Floor"));
            if(jumpRay.collider != null)
            {
                if (jumpRay.distance < 0.8f)
                {
                    isJumping = false;
                }
            }
        }
        RaycastHit2D objectRay = Physics2D.Raycast(transform.position, Vector2.right, 1f, LayerMask.GetMask("Floor"));
        if (objectRay.collider != null)
        {
            if (objectRay.distance < 1.3f)
            {
                windRes = 0f;
            }
        }
        else
        {
            if (isJumping)
            {
                if(powerRecord < 14)
                    windRes = MAX_WIND_RES + 30;
                if (powerRecord < 9)
                    windRes = MAX_WIND_RES + 20;
                if (powerRecord < 6)
                    windRes = MAX_WIND_RES + 10;
            }
            else if (!isCharging)
                windRes = MAX_WIND_RES;
        }
        
        Debug.Log(Mathf.Pow((110 - Vector2.Distance(transform.position, windPosition)) * 0.07f, 2) * windRes * 0.01f);

        playerRigid.velocity = new Vector2(direction * speed - (Mathf.Pow((110 - Vector2.Distance(transform.position, windPosition)) * 0.08f, 2) * windRes * 0.01f), playerRigid.velocity.y);
    }

    public void Move(int dir)
    {
        direction = dir;
    }
    
    private IEnumerator AddPower()
    {
        while (true)
        {
            if (!isJumping)
            {
                isCharging = true;
                windRes = MIN_WIND_RES;
                if (jumpPower < 13)
                    jumpPower++;
            }
            yield return new WaitForSeconds(0.1f);
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
        jumpPower = 3;
    }
}
