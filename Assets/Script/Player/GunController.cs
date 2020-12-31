using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameObject bull;
    private Transform firePos;
    public FireRequest fireRequest;
    public DamageRequest damageRequest;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bull = Resources.Load("Prefab/bullet") as GameObject;
        firePos = transform.Find("Fire");
    }

    private void Update()
    {
        LookAt();
        Fire();
    }

    private void LookAt()
    {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation=Quaternion.AngleAxis(angle,Vector3.forward);
        if (transform.eulerAngles.z >= 90 && transform.eulerAngles.z <= 270)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 m_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_mousePos.z = 0;
            float m_fireAngle = Vector3.Angle(m_mousePos - transform.position, Vector3.up);
            if (m_mousePos.x > transform.position.x)
            {
                m_fireAngle = -m_fireAngle;
            }
            GameObject g = Instantiate(bull,firePos.position,Quaternion.identity);
            g.transform.eulerAngles=new Vector3(0,0,m_fireAngle+90);
            Vector2 velocity = (m_mousePos - firePos.position).normalized * 20;
            g.GetComponent<Rigidbody2D>().velocity=velocity;
            Bullet  b= g.GetComponent<Bullet>();
            b.isLocal = true;
            b.damageRequest = damageRequest;
            b.start = m_mousePos;
            fireRequest.SendRequest(firePos.position,m_fireAngle+90,m_mousePos);
        }
    }
}
