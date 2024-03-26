using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float initialJumpPower;
    [SerializeField] private float extraJumpPower;
    [SerializeField] private float maxExtraJumpTime;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float coyoteTime; // Add coyoteTime for the grace period
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private bool isJumping;
    private float extraJumpStartTime;
    private float lastGroundedTime; // Add a variable to store the last time the player was grounded

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        bool grounded = isGrounded();
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);

        if (grounded)
            lastGroundedTime = Time.time; // Update lastGroundedTime when the player is grounded

        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !grounded)
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastGroundedTime <= coyoteTime)
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;

        if (isJumping && Input.GetKey(KeyCode.Space))
        {
            if (Time.time - extraJumpStartTime < maxExtraJumpTime)
                body.AddForce(new Vector2(0, extraJumpPower), ForceMode2D.Impulse);
            else
                isJumping = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
            isJumping = false;
    }

    private void Jump()
    {
        if (isGrounded() || Time.time - lastGroundedTime <= coyoteTime)
        {
            isJumping = true;
            extraJumpStartTime = Time.time;
            body.velocity = new Vector2(body.velocity.x, initialJumpPower);
            anim.SetTrigger("jump");
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}