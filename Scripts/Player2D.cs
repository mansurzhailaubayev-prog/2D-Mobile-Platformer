using UnityEngine;
using UnityEngine.InputSystem;

public class Player2D : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;

            AudioManager.Instance?.PlayJump();

            if (anim != null) anim.SetBool("IsJumping", true);
        }
    }

    public void OnFire()
    {
        Shooter shooter = GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.Shoot();
        }
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        if (moveInput.x != 0 && sr != null)
        {
            sr.flipX = moveInput.x < 0;
        }
        if (anim != null) anim.SetFloat("Speed", Mathf.Abs(moveInput.x));

        if (transform.position.y < -15f)
        {
            Health health = GetComponent<Health>();
            if (health != null) health.TakeDamage(99);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (anim != null) anim.SetBool("IsJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}