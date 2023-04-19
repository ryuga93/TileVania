using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 4f;
    [SerializeField] Vector2 deathForce = new Vector2(10f, 10f);
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D playerRb;
    Animator playerAnimator;
    CapsuleCollider2D playerCollider;
    BoxCollider2D feetCollider;
    float playerGravityScale;
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerGravityScale = playerRb.gravityScale;
        feetCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }

        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        int groundLayerMask = LayerMask.GetMask("Ground");
        if (!feetCollider.IsTouchingLayers(groundLayerMask) || !isAlive)
        {
            return;
        }

        if (value.isPressed)
        {
            playerRb.velocity = new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }

        float sign = Mathf.Sign(transform.localScale.x);
        Vector3 bulletRotation = new Vector3(0f, 0f, sign * -90f);
        GameObject bullet = Instantiate(bulletPrefab, gun.position, Quaternion.Euler(bulletRotation));
    }

    void ClimbLadder()
    {
        int climbingLayerMask = LayerMask.GetMask("Climbing");
        if (!feetCollider.IsTouchingLayers(climbingLayerMask))
        {
            playerRb.gravityScale = playerGravityScale;
            playerAnimator.SetBool("isClimbing", false);
            return;
        }
        
        playerRb.gravityScale = 0f;
        playerRb.velocity = new Vector2(playerRb.velocity.x, moveInput.y * climbSpeed);

        bool playerHasVerticalSpeed = Mathf.Abs(playerRb.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, playerRb.velocity.y);
        playerRb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRb.velocity.x), 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.GetComponent<EnemyMovement>() || LayerMask.LayerToName(other.gameObject.layer) == "Hazards") && isAlive)
        {
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
        playerAnimator.SetTrigger("Dying");
        playerRb.AddForce(deathForce, ForceMode2D.Impulse);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
