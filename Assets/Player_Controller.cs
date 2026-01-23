using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float movementX;
    float movementY;
    [SerializeField] float speed = 5.0f;
    [SerializeField] float dash = 15.0f;
    Rigidbody2D rb;
    bool isGrounded = true;
    int score = 0;
    bool canDash = true;

    Animator animator;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //float movementDistanceX = movementX * speed * Time.deltaTime;
        //float movementDistanceY = movementY * speed * Time.deltaTime;
        //transform.position = new Vector2(transform.position.x + movementDistanceX, transform.position.y + movementDistanceY);
        rb.linearVelocity = new Vector2(movementX * speed, rb.linearVelocity.y);
        if (!Mathf.Approximately(movementX, 0f))
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = movementX < 0;
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (isGrounded && movementY > 0)
        {
            rb.AddForce(new Vector2(0, 100));
        }

    }

    void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        movementX = v.x;
        movementY = v.y;
        //Debug.Log("Movement X = " + movementX);
        //Debug.Log("Movement Y = " + movementY);
    }

    void OnDash(InputValue value)
    {
        if (canDash)
        {
            int way;
            if (movementX < 0)
            {
                way = -800;
            }
            else
            {
                way = 800;
            }
                rb.AddForce(new Vector2(way, 0));
            if (!isGrounded)
            {
                canDash = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDash = true;
            animator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            score++;
            collision.gameObject.SetActive(false);
            Debug.Log("Score: " + score);
        }
    }
}
