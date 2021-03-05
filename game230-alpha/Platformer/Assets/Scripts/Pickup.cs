using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickSFX;

    [SerializeField] int coinValue = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<GameSession>().ProcessPlayerScore(coinValue);
        
        // PlayClipAtPoint uses Vector3 info for where the sound plays from
        AudioSource.PlayClipAtPoint(coinPickSFX, Camera.main.transform.position);

        FindObjectOfType<GameSession>().AddLife();

        Destroy(gameObject);
    }
}
