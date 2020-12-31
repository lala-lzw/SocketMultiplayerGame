using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Bullet : MonoBehaviour
{
    public bool isLocal = false;
    public DamageRequest damageRequest;
    public Vector2 start, end;

    private void Start()
    {
        Destroy(gameObject,3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"&& isLocal)
        {
            //击中
            end = other.transform.position;
            damageRequest.SendRequest(transform.position.x,transform.position.y,other.gameObject.GetComponent<CharacterRistic>().username,start.x,start.y,end.x,end.y);
            Debug.Log("击中");
        }
        Destroy(gameObject);
    }
}
