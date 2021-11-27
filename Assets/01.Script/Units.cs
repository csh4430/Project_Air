using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public int num;
    public bool isPicked;
    public bool isFloating;

    private Animator unitAni = null;
    private Rigidbody2D unitRigid = null;

    private void Awake()
    {
        unitAni = GetComponent<Animator>();
        unitRigid = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (isPicked)
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_MainTex", Color.yellow);
        else
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_MainTex", Color.clear);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        unitAni.SetBool("isFloat", false);
        if (isPicked)
        {
            isFloating = false;
            GameManager.Instance.FallUnit();
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (isFloating)
        {
            GameManager.Instance.SetGame();
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    public void SetPick(bool picked)
    {
        isPicked = picked;
    }
    public void SetFloat(bool floated)
    {
        isFloating = floated;
        if (isFloating)
        {
            unitAni.SetBool("isFloat", true);
            unitRigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            transform.position = new Vector3(transform.position.x,transform.position.y, -2);
        }
    }
}
