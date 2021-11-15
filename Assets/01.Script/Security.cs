using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Security : MonoBehaviour
{
    private float speed = 0;

    private Rigidbody2D rb = null;
    private RectTransform rect = null;
    private IEnumerator move = null;

    private bool rayDir = false;
    private bool isTrace = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rect = GetComponent<RectTransform>();
        move = Move();
        StartCoroutine(move);
    }

    void Update()
    {
        GameOver();
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(1);
        while(true)
        {
            if (isTrace) break;
            speed = -3;
            transform.localScale = new Vector2(1, 1);
            yield return new WaitForSeconds(3);
            speed = 0;
            yield return new WaitForSeconds(2);

            speed = 3;
            transform.localScale = new Vector2(-1, 1);
            yield return new WaitForSeconds(3);
            speed = 0;
            yield return new WaitForSeconds(2);
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
        if (collision != null && collision.CompareTag("Player"))
        {
            Trace(collision);
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            speed = 0;
            StartCoroutine(move);
        }
    }

    private void Trace(Collider2D collision)
    {
        StopCoroutine(move);
        
        if (collision.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
            speed = -3;

        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
            speed = 3;
        }
    }
}
