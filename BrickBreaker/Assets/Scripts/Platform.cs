using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Vector2 mousePrevPos;

    private void Update()
    {
        
    }
    private void FixedUpdate()
    {

        Vector3 deltaPos = Vector3.zero;
        deltaPos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - mousePrevPos.x;
        transform.position += deltaPos;
        mousePrevPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
