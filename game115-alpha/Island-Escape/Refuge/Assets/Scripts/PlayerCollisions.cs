using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    // Logic for Shack Door, yo!
    private bool doorIsOpen = false;
    private float doorTimer = 0.0f;
    public float doorOpenTime = 3.0f;

    // Door sounds
    public AudioClip doorOpenSound;
    public AudioClip doorShutSound;
    private new AudioSource audio;

    // Battery sound
    public AudioClip batteryCollectSound;

    // Satellite Dish sound
    public AudioClip satelliteDishSound;

    // Radio sound
    public AudioClip radioSound;

    // Helicopter
    public GameObject helicopter;
    public AudioSource helicopterAudio;

    // Fader
    public Animator fader;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        helicopter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Timer that automatically shuts door once it's open
        if(doorIsOpen)
        {
            doorTimer += Time.deltaTime;
        }
        if(doorTimer > doorOpenTime)
        {
            ShutDoor();
            doorTimer = 0.0f;
        }
    }

    // Collision detection
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "shackDoor" && !doorIsOpen && BatteryCollect.charge >= 4)
        {
            OpenDoor();
            BatteryCollect.chargeUI.enabled = false;
        }
        else if (hit.gameObject.tag == "shackDoor" && !doorIsOpen && BatteryCollect.charge < 4)
        {
            BatteryCollect.chargeUI.enabled = true;
            TextHints.message = "The door seems to need more power...";
            TextHints.textOn = true;
        }
    }

    // Battery/Dish collision
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "battery")
        {
            BatteryCollect.charge++;
            audio.PlayOneShot(batteryCollectSound);
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "dish" && !SatelliteDish.radio)
        {
            audio.PlayOneShot(satelliteDishSound);
            TextHints.message = "Maybe there's a radio somewhere...";
            TextHints.textOn = true;
        }

        if (coll.gameObject.tag == "radio")
        {
            SatelliteDish.radio = true;
            audio.PlayOneShot(radioSound);
            TextHints.message = "You were able to contact help! Go check outside!";
            TextHints.textOn = true;
            helicopter.SetActive(true);
        }

        if (coll.gameObject.tag == "helicopter" && SatelliteDish.radio)
        {
            fader.SetTrigger("FadeOut");
        }
    }

    void OpenDoor()
    {
        // Play audio
        audio.PlayOneShot(doorOpenSound);
        // Set doorIsOpen to true
        doorIsOpen = true;
        // Find the GameObject that has animation
        GameObject myShack = GameObject.Find("Shack");
        // Play animation
        myShack.GetComponent<Animation>().Play("doorOpen");
    }

    void ShutDoor()
    {
        audio.PlayOneShot(doorShutSound);
        doorIsOpen = false;
        GameObject myShack = GameObject.Find("Shack");
        myShack.GetComponent<Animation>().Play("doorShut");
    }
}
