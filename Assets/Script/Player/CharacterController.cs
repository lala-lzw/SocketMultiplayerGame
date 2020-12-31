using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D r2d;

    private bool island;
    private void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            r2d.velocity=new Vector2(h*10,r2d.velocity.y); 
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W)&& island)
        {
            r2d.velocity=new Vector2(r2d.velocity.x,15);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        island = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        island = false;
    }
}
