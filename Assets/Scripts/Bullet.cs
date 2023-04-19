using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 5f;
    Rigidbody2D bulletRb;

    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        float sign = -Mathf.Sign(transform.rotation.z);
        Vector2 bulletMovement = new Vector2(sign * bulletSpeed, 0f);
        bulletRb.AddForce(bulletMovement, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);

        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemies")
        {
            Destroy(other.gameObject);
        }
    }
}
