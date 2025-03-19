using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 30f;
    public float dashTime = 0.4f;
    private bool isDashing = false;
    private SpriteRenderer sr;

    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isCharging = false;
    private float jumpCharge = 0f;
    private float colorCharge = 0f;
    private bool isGrounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f; // 중력 적용
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocityX = movement.x * speed;
        }

        if (isCharging)
        {
            colorCharge += 0.01f;
            sr.color = new Color(colorCharge,1f,1f);
            jumpCharge += 2f;
            if(!isGrounded)
            {
                jumpCharge += 3f;
                rb.AddTorque(10f, ForceMode2D.Force);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.constraints = RigidbodyConstraints2D.None;
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void OnDash()
    {
        StartCoroutine(Dash());
        StartCoroutine(StopFalling());
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        // movement가 계속 바뀌니까 while문을 사용할 필요 없음.
        //rb.linearVelocityX = movement.normalized.x * dashSpeed;
        rb.AddForce(movement * dashSpeed * 15, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashTime);

        isDashing = false;
    }

    IEnumerator StopFalling()
    {
        // 0.5초 동안 y축 움직임 멈춤
        float elapsed = 0f;

        while(elapsed < 0.3f)
        {
            rb.linearVelocityY = 0f;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void OnCharge()
    {
        isCharging = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public void OnJump()
    {
        isCharging = false;
        if(!isGrounded)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        rb.angularVelocity = 0f;
        rb.transform.rotation = Quaternion.identity;
        sr.color = Color.white;
        if (!isGrounded)
        {
            rb.AddForce(Vector2.down * jumpCharge, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.up * jumpCharge, ForceMode2D.Impulse);
        }
        jumpCharge = 0f;
        colorCharge = 0f;
    }
}
