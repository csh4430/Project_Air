using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Traffic_Lights : MonoBehaviour
{
    private Transform car = null;
    [SerializeField] private new Image light = null;

    private bool isGreen = false;

    private float time = 0;
    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Car").transform;
        car.gameObject.SetActive(false);
        StartCoroutine(ChangeLights());
    }

    private IEnumerator ChangeLights()
    {
        while (true)
        {
            isGreen = false;
            light.color = Color.red;
            yield return new WaitForSeconds(10f);
            isGreen = true;
            light.color = Color.green;
            yield return new WaitForSeconds(5);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGreen)
        {
            time += Time.deltaTime;
            if (time >= 2)
            {
                CarMove(collision);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!isGreen)
        {
            time = 0;
        }
    }

    private void CarMove(Collider2D coll)
    {
        if(car.transform.position != coll.transform.position)
            car.position = coll.transform.position;

        GameManager.Instance.PlayerBase.isDead = true;

        car.gameObject.SetActive(true);
        car.DOScale(Vector3.one * 3, 1).SetEase(Ease.Linear);

        Debug.Log("Game Over!");
        time = 0;
    }
}
