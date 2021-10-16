using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    [SerializeField] private int arr;
    private bool doEnter = false;

    private void Awake()
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        doEnter = !doEnter;
        if (doEnter)
        {
            Camera.main.transform.position = new Vector3(GameManager.Instance.Xs[arr], 0, -10);
        }
        else
        {
            Camera.main.transform.position = new Vector3(GameManager.Instance.Xs[arr - 1], 0, -10);

        }

    }
}
