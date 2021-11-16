using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
    private const int PLAYER_LAYER = 10;
    private const int DEFAULT_LAYER = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.layer = DEFAULT_LAYER;
            Debug.Log($"{collision.name} = {collision.gameObject.layer}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.layer = PLAYER_LAYER;
            Debug.Log($"{collision.name} = {collision.gameObject.layer}");
        }
    }
}
