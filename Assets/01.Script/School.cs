using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class School : MonoBehaviour
{
    [SerializeField]
    private Collider2D playerCol = null;

    private Security security = null;

    private bool isCoroutineRunning = false;

    void Start()
    {
        security = FindObjectOfType<Security>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isCoroutineRunning) return;
            if (security.isSlipped) return;
            StartCoroutine(Slipped());
        }
    }

    private IEnumerator Slipped()
    {
        isCoroutineRunning = true;
        // nÃÊ °£°ÝÀ¸·Î ÆÇº°
        yield return new WaitForSeconds(1);
        int randomDelay = Random.Range(1, 101);

        if (randomDelay > 80)
        {
            // »ß²ýÇÒ ¶§
            Debug.Log($"{randomDelay}, »ß²ý");
            StartCoroutine(security.SlippedReaction(playerCol));
        }
        isCoroutineRunning = false;
    }
}
