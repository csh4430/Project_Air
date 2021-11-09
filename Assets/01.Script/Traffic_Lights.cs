using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Traffic_Lights : MonoBehaviour
{
    private Transform car = null;

    private bool isGreen = false;

    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Car").transform;
        car.gameObject.SetActive(false);
    }

    private IEnumerator ChangeLights()
    {
        while (true)
        {
            isGreen = false;
            yield return new WaitForSeconds(10f);
            isGreen = true;
            yield return new WaitForSeconds(5);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGreen)
        {
            car.position = collision.transform.position;

            StartCoroutine(CarMove());
        }
    }

    private IEnumerator CarMove()
    {
        yield return new WaitForSeconds(1);
        car.gameObject.SetActive(true);
        car.DOScale(new Vector3(3, 3, 0), 1).SetEase(Ease.Linear);

        Debug.Log("Game Over!");
    }
}
