using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    // Set up the door's Animator
    public GameObject door; // This will become an open slot in Unity to drag the door object into
    Animator doorAnimator;

    // Set up the door timer
    private bool startDoorTimer = false;
    private float doorTimer = 0.0f;
    [SerializeField] private float doorOpenTime = 3.0f;

    // Door sounds
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip doorShutSound;
    private bool doorOpen = false;
    private new AudioSource audio;

    // Battery sound
    [SerializeField] private AudioClip batteryCollectSound;

    // Oars
    private bool hasOars = false;
    [SerializeField] private AudioClip oarCollectSound;

    // Boat
    [SerializeField] private AudioClip boatSound;

    // Escape
    [SerializeField] private AudioClip escapeSound;

    // Spawn Point
    [SerializeField] private Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = door.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Run the timer
        if(startDoorTimer)
        {
            doorTimer += Time.deltaTime;
        }
        // Automatically shut door after timer
        if (doorTimer > doorOpenTime)
        {
            ShutDoor();
            doorTimer = 0.0f;
        }
    }

    // Collision detection
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Checking if we're hitting the shackDoor
        if(hit.gameObject.tag == "shackDoor" && BatteryCollect.charge >= 4)
        {
            // Debug.Log("Hit the door!");

            OpenDoor();
            BatteryCollect.chargeUI.enabled = false;
        }
        else if (hit.gameObject.tag == "shackDoor" && BatteryCollect.charge < 4)
        {
            BatteryCollect.chargeUI.enabled = true;
            TextHints.message = "The door seems to need more power...";
            TextHints.textOn = true;
        }


        if (hit.gameObject.tag == "waterBlocker")
        {
            Debug.Log("Watch out! There might be sharks in there!");

            TextHints.message = "Watch out! There might be sharks in there!";
            TextHints.textOn = true;

            transform.position = spawnPoint.position;
            // transform.rotation = spawnPoint.rotation;
            // transform.rotation = Quaternion.Euler(0, -180, 0);
            // transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(spawnPoint.localRotation.eulerAngles.x, spawnPoint.localRotation.eulerAngles.y, spawnPoint.localRotation.eulerAngles.z));
            // transform.localRotation = Quaternion.Euler(spawnPoint.localRotation.eulerAngles.x, spawnPoint.localRotation.eulerAngles.y, spawnPoint.localRotation.eulerAngles.z);
            // transform.localRotation = Quaternion.Euler(new Vector3(spawnPoint.localRotation.eulerAngles.x, spawnPoint.localRotation.eulerAngles.y, spawnPoint.localRotation.eulerAngles.z));
        }
    }

    // Open door function
    void OpenDoor()
    {
        // Set Animator parameter
        doorAnimator.SetBool("doorIsOpen", true);
        // Start the door timer!
        startDoorTimer = true;
        // Play audio
        if (!doorOpen)
        {
            audio.PlayOneShot(doorOpenSound);
            doorOpen = true;
        }
    }

    // Shut door function
    void ShutDoor()
    {
        // Debug.Log("Shut door.");
        // Set Animator parameter
        doorAnimator.SetBool("doorIsOpen", false);
        // Stop the door timer!
        startDoorTimer = false;
        // Play audio
        audio.PlayOneShot(doorShutSound);
        doorOpen = false;
    }

    // Trigger collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "battery")
        {
            BatteryCollect.charge++;
            audio.PlayOneShot(batteryCollectSound);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "oars")
        {
            hasOars = true;
            audio.PlayOneShot(oarCollectSound);
            Destroy(other.gameObject);

            TextHints.message = "You got the oars. Time to find a boat!";
            TextHints.textOn = true;
        }
        if (other.gameObject.tag == "boat")
        {
            audio.PlayOneShot(boatSound);
            if (!hasOars)
            {
                Debug.Log("You won't get far without some oars!");

                TextHints.message = "You won't get far without some oars!";
                TextHints.textOn = true;
            }
            else
            {
                audio.PlayOneShot(escapeSound);
                Application.Quit();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "boat")
        {
            audio.Stop();
        }
    }
}
