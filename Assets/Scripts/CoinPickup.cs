using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] int score = 100;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            AudioSource.PlayClipAtPoint(pickupSFX, new Vector3(0f, 0f, Camera.main.transform.position.z));
            FindObjectOfType<GameSession>().IncreaseScore(score);
            Destroy(gameObject);
        }    
    }
}
