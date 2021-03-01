using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickSFX;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // PlayClipAtPoint uses Vector3 info for where the sound plays from
        AudioSource.PlayClipAtPoint(coinPickSFX, Camera.main.transform.position);
        
        Destroy(gameObject);
    }
}
