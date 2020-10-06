using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Vector2 mousePrevPos;
    [SerializeField]
    GameObject visualBlock;
    [SerializeField]
    float speed;
    public NormalBlock Block { private set; get; }
    private void Awake()
    {
        Block = new NormalBlock(transform.localScale.x, transform.localScale.y, 0f, transform.position, null, Color.cyan);
        GameObject visualPart = Instantiate(visualBlock, transform);
        visualPart.GetComponent<SpriteRenderer>().color = new Color(0.749f, 0.2f, 0.188f, 1f);
    }
    private void FixedUpdate()
    {
        Vector3 deltaPos = Vector3.zero;
        deltaPos.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - mousePrevPos.x;
        transform.position += deltaPos;
        mousePrevPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Block.RecalculateCollider(transform.position, 0f);
    }

  
}
