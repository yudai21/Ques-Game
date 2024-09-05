using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecondMove : MonoBehaviour
{
    public float moveSpeed = 3f; 
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public SecondTalk secondTalk;
    
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isMoving = false;
    public LayerMask stairsLayer; // 階段のレイヤー
    private bool isOnStairs = false; // プレイヤーが階段にいるかどうかを示すフラグ
    private Vector2 stairDirection = new Vector2(1f, 1f); // 階段を上る際の移動方向


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isOnStairs)
        {
            rb.velocity = stairDirection.normalized * moveSpeed;
            animator.SetBool("Walk", true);
        }
        else if (isMoving && isGrounded)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    public void StartMoving()
    {
        isMoving = true;  
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "stone")
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
            animator.SetBool("Walk", false);
            secondTalk.TriggerDialogue(); // 会話を開始するメソッドを呼び出す
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
            isOnStairs = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
            isOnStairs = false;
        }
    }
}
