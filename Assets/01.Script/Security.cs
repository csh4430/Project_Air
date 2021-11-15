using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Security : MonoBehaviour
{
    private const float speed = 5;

    private Rigidbody2D rb = null;
    private RectTransform rect = null;
    private Transform playerTr = null;

    private bool rayDir = false;
    private bool isTrace = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rect = GetComponent<RectTransform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(Move());
    }

    void Update()
    {
        GameOver();
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(1);
        while(true)
        {
            if (isTrace) break;
            rect.DOScaleX(-1, 0);
            rayDir = false;
            rb.velocity = new Vector2(speed, rb.velocity.y);
            yield return new WaitForSeconds(2);
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(2);
            rect.DOScaleX(1, 0);
            rayDir = true;
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            yield return new WaitForSeconds(2);
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(2);
            InfiniteLoopDetector.Run();
        }
    }

    private void GameOver()
    {
        if (rayDir)
        {
            RaycastHit2D Ray = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, LayerMask.GetMask("Player"));
            Debug.DrawLine(transform.position, transform.position + Vector3.left, Color.blue);
            if (Ray.collider != null && Ray.collider.CompareTag("Player"))
            {
                if (Ray.distance < .8f)
                {
                    GameManager.Instance.PlayerBase.isDead = true;
                }
            }
        }
        else if (!rayDir)
        {
            RaycastHit2D Ray = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, LayerMask.GetMask("Player"));
            Debug.DrawLine(transform.position, transform.position + Vector3.right, Color.blue);
            if (Ray.collider != null && Ray.collider.CompareTag("Player"))
            {
                if (Ray.distance < .8f)
                {
                    GameManager.Instance.PlayerBase.isDead = true;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isTrace = true;
            StartCoroutine(Trace());
        }
    }

    private IEnumerator Trace()
    {
        if(isTrace)
        {
            // 이거 문제 있는데

            // 플리에어 약간 추격하기
            if((transform.position.x - playerTr.position.x) < 0)
            {
                transform.DOKill();
                Debug.Log("1");
                transform.DOLocalMoveX(40, 1).SetEase(Ease.Linear);
                //rb.velocity = new Vector2(speed, rb.velocity.y);
                isTrace = false;

                yield return new WaitForSeconds(1);
                StartCoroutine(Move());
            }
            else if((transform.position.x - playerTr.position.x) > 0)
            {
                transform.DOKill();
                Debug.Log("1");
                transform.DOLocalMoveX(-40, 1);
                //rb.velocity = new Vector2(-speed, rb.velocity.y);
                isTrace = false;
                yield return new WaitForSeconds(1);
                StartCoroutine(Move());
            }
        }
    }
}
