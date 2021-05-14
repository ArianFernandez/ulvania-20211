using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform hero;
    public float heroSpeed;
    public float heroJumpSpeed;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    Rigidbody2D rbHero;
    Animator animmatorHero;
    SpriteRenderer srHero;
    CapsuleCollider2D colliderHero;
    private bool jump = false;
    private float movement;

    void Start()
    {
        rbHero = hero.GetComponent<Rigidbody2D>();
        animmatorHero = hero.GetComponent<Animator>();
        srHero = hero.GetComponent<SpriteRenderer>();
        colliderHero = hero.GetComponent<CapsuleCollider2D>();
    }


    private void FixedUpdate()
    {
        rbHero.velocity = new Vector2(movement * heroSpeed, rbHero.velocity.y);

        if (rbHero.velocity.y < 0)
        {
            // Esta cayendo
            rbHero.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }else if (rbHero.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rbHero.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1  ) * Time.deltaTime;
        }

        if (jump)
        {
            rbHero.velocity = new Vector2(rbHero.velocity.x, heroJumpSpeed);
            jump = !jump;
        }
    }

    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");

        if (movement < 0)
        {
            srHero.flipX = true;
            animmatorHero.SetBool("isRunning", true);
        }else if (movement > 0)
        {
            srHero.flipX = false;
            animmatorHero.SetBool("isRunning", true);
        }
        else
        {
            animmatorHero.SetBool("isRunning", false);
        }


        if (!IsJumping()) // puede ser un or (if (!isJumping() && !isJumping <--- no esta tan bien
        {
            animmatorHero.SetBool("isJumping", false);
            jump = false;
        }

        if (!IsJumping() && Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        
    }

    private void Jump()
    {
        jump = true;
        animmatorHero.SetBool("isJumping", true);
        animmatorHero.SetTrigger("jump");
        
    }

    private bool IsJumping()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            colliderHero.bounds.center,
            Vector2.down,
            colliderHero.bounds.extents.y + 0.2f
        );

        DebugJumpRay(hit);

        return !hit;
    }

    private void DebugJumpRay(RaycastHit2D hit)
    {
        Color color;
        if (hit)
        {
            color = Color.red;
        }
        else
        {
            color = Color.white;
        }

        Debug.DrawRay(colliderHero.bounds.center,
            Vector2.down * (colliderHero.bounds.extents.y + 0.2f),
            color);
    }
}
