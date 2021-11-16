using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Security : MonoBehaviour
{
    private float speed = 0;

    private Rigidbody2D rb = null;
    private IEnumerator move = null;

    private RaycastHit2D ray;

    private bool rayDir = false;
    private bool isCoroutineRunning = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move = Move();
        StartCoroutine(move);
    }

    void Update()
    {
        if (rayDir)
        {
            ray = Physics2D.Raycast(transform.position, Vector2.left * 8, Mathf.Infinity, LayerMask.GetMask("Player"));
            Debug.DrawLine(transform.position, transform.position + Vector3.left * 8, Color.red);
        }
        else if (!rayDir)
        {
            ray = Physics2D.Raycast(transform.position, Vector2.right * 8, Mathf.Infinity, LayerMask.GetMask("Player"));
            Debug.DrawLine(transform.position, transform.position + Vector3.right * 8, Color.red);
        }

        GameOver();
        LightEnter();
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private IEnumerator Move()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(1);
        while(true)
        {
            speed = -3;
            rayDir = true;
            transform.localScale = new Vector2(1, 1);
            yield return new WaitForSeconds(3);
            speed = 0;
            yield return new WaitForSeconds(2);

            speed = 3;
            rayDir = false;
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
            if (Ray.collider != null && Ray.collider.CompareTag("Player") && Ray.collider.gameObject.layer == 10)
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
            if (Ray.collider != null && Ray.collider.CompareTag("Player") && Ray.collider.gameObject.layer == 10)
            {
                if (Ray.distance < .8f)
                {
                    GameManager.Instance.PlayerBase.isDead = true;
                }
            }
        }
    }

    private bool BoolLightEnter()
    {
        if (ray.collider != null && ray.collider.CompareTag("Player"))
        {
            if (ray.distance < 7.8f)
            {
                return true;
            }
        }
        else if (ray.collider == null)
        {
            return false;
        }

        return false;
    }

    private void LightEnter()
    {
        if(BoolLightEnter())
        {
            Trace(ray.collider);
        }
        else if(!BoolLightEnter())
        {
            if (isCoroutineRunning) return;
            speed = 0;
            StartCoroutine(move);
            isCoroutineRunning = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && collision.gameObject.layer == 10)
            GameManager.Instance.PlayerBase.isDead = true;
    }

    private void Trace(Collider2D collision)
    {
        StopCoroutine(move);
        isCoroutineRunning = false;

        Debug.Log("Hit");
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
