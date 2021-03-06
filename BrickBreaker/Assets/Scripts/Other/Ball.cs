﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector2 Velocity { private set; get; }
    public float radius { private set; get; }
    [SerializeField]
    float speed;
    public Vector2 prevPos { private set; get; }
    Vector2 leftBottomCorner, rightUpCorner;
    public void SetVelocity(Vector2 vel)
    {
        Velocity = vel;
    }
    void Start()
    {
        leftBottomCorner = Camera.main.ScreenToWorldPoint(Vector3.zero);
        rightUpCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        prevPos = transform.position;
        Velocity = new Vector2 (Random.Range(-0.8f, 0.8f), Random.Range(0, 1f));
        Velocity = Velocity.normalized * speed;
        radius = 0.15f;
    }
    void Update()
    {
        if (transform.position.x - radius <= leftBottomCorner.x)
        {
            Velocity = new Vector2(-Velocity.x, Velocity.y);
            transform.position = new Vector3(leftBottomCorner.x + radius, transform.position.y, transform.position.z);
        }
        else if (transform.position.x + radius>= rightUpCorner.x)
        {
            Velocity = new Vector2(-Velocity.x, Velocity.y);
            transform.position = new Vector3(rightUpCorner.x - radius, transform.position.y, transform.position.z);
        }
        if (transform.position.y - radius <= leftBottomCorner.y)
        {
            Velocity = new Vector2(Velocity.x, -Velocity.y);
            transform.position = new Vector3(transform.position.x, leftBottomCorner.y + radius, transform.position.z);
        }
        else if (transform.position.y + radius >= rightUpCorner.y)
        {
            Velocity = new Vector2(Velocity.x, -Velocity.y);
            transform.position = new Vector3(transform.position.x, rightUpCorner.y - radius, transform.position.z);
        }
    }    
    
    private void FixedUpdate()
    {
        prevPos = transform.position;
        transform.position = new Vector3(
            transform.position.x + Velocity.x * Time.deltaTime,
            transform.position.y + Velocity.y * Time.deltaTime,
            transform.position.z);
    }
   
}
