using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    private Rigidbody2D playerRigid;
    [SerializeField] private EventTrigger ET = null;
    [SerializeField] private Vector2 windPosition;
    private int direction = 0;
    private int jumpPower = 3;
    private IEnumerator addPower;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();

        addPower = AddPower();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => { StartCoroutine(addPower); });
        ET.triggers.Add(pointerDown);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(-1);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(1);
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            Move(0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(addPower);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jump();
        }
        Debug.Log(Vector2.Distance(transform.position, windPosition));
        playerRigid.velocity = new Vector2(direction * speed - Mathf.Sqrt((110 - Vector2.Distance(transform.position, windPosition))) * 0.4f, playerRigid.velocity.y);
    }

    public void Move(int dir)
    {
        direction = dir;
    }
    
    private IEnumerator AddPower()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (jumpPower < 13)
                jumpPower++;
        }
    }
    public void Jump()
    {
        StopCoroutine(addPower);
        playerRigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        jumpPower = 3;
    }
}
