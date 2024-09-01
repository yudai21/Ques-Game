using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f; 
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public DialogueLoader dialogueLoader;
    public TextMeshProUGUI enemyDialogueText;
    
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (isMoving && isGrounded)
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
            StartCoroutine(StartDialogue());
        }
    }

    IEnumerator StartDialogue()
    {
        List<string[]> dialogues = dialogueLoader.GetDialogues();

        foreach (string[] dialogue in dialogues)
        {
            if (dialogue[0] == "Enemy")
            {
                enemyDialogueText.text = dialogue[1];
                yield return new WaitForSeconds(2f);
            }
        }

        enemyDialogueText.text = "";
    }
}
