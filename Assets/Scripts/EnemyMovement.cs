using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    Rigidbody2D enemyRb;
    BoxCollider2D rotateCollider;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        rotateCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyRb.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ground")
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(Mathf.Sign(moveSpeed), 1f);
    }
}
