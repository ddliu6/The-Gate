using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Animator animator;
    Transform leftPoint, rightPoint;
    Vector3 localScale;
    bool moveR = true;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        leftPoint = GameObject.Find("LeftPoint").GetComponent<Transform>();
        rightPoint = GameObject.Find("RightPoint").GetComponent<Transform>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveSpeed));

        if (transform.position.x > rightPoint.position.x)
            moveR = false;
        if (transform.position.x < leftPoint.position.x)
            moveR = true;
        if (moveR)
            moveRight();
        else
            moveLeft();
    }
    void moveRight()
    {
        moveR = true;
        localScale.x = 1;
        transform.localScale = localScale;
        rb.velocity = new Vector2(localScale.x * moveSpeed, rb.velocity.y);
    }
    void moveLeft()
    {
        moveR = false;
        localScale.x = -1;
        transform.localScale = localScale;
        rb.velocity = new Vector2(localScale.x * moveSpeed, rb.velocity.y);
    }
}
