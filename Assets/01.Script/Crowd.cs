using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    private RectTransform rect = null;
    private Rigidbody2D rigid = null;
    private float speed = 4;
    private float enabledTime;
    private bool isRunning = false;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        //등장
        speed = 4;
        enabledTime = Time.time;
        isRunning = false;
        rect.transform.position = new Vector2(Camera.main.transform.position.x - Camera.main.orthographicSize * 2, rect.transform.position.y);
    }

    void Update()
    {
        //이동
        rigid.velocity = new Vector2(speed, rigid.velocity.y);

        if (Time.time - enabledTime >= 5)
        {
            speed = 15;
            isRunning = true;
        }

        if(rect.transform.position.x >= Camera.main.transform.position.x + Camera.main.orthographicSize * 2)
        {
            gameObject.SetActive(false);
            isRunning = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (isRunning)
                GameManager.Instance.PlayerBase.isCrowdExit = true;
        }
    }
}
