using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoSingleton<CameraMove>
{
    public bool isOver = true;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(!isOver && Vector2.Distance(this.transform.position, GameManager.Instance.PlayerBase.transform.position) <= 6)
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(GameManager.Instance.PlayerBase.transform.position.x +6, this.transform.position.y, -10), Time.deltaTime * 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {
            isOver = true;
        }
    }
}
