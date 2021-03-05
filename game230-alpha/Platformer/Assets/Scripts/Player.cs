using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D playerCharacter;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    CircleCollider2D playerHeadCollider;

    [SerializeField] float runSpeed = 5.0f;
    [SerializeField] float jumpSpeed = 5.0f;
    [SerializeField] float climbSpeed = 5.0f;
    [SerializeField] Vector2 deathSeq = new Vector2(25f, 25f);
    float gravityScaleAtStart;

    AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip jumpSound;
    private bool jump;

    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        playerHeadCollider = GetComponent<CircleCollider2D>();

        audioSource = GetComponent<AudioSource>();

        gravityScaleAtStart = playerCharacter.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive)
        {
            // Return halts the method
            return;
        }

        Run();
        FlipSprite();
        Jump();
        Climb();
        DetectCeiling();
        Die();
    }
    private void Run()
    {
        // Horiz movement value between -1 and 1
        float hMovement = Input.GetAxis("Horizontal");
        Vector2 runVelocity = new Vector2(hMovement * runSpeed, playerCharacter.velocity.y);
        playerCharacter.velocity = runVelocity;
        // Turn on the Animator's run Parameter
        playerAnimator.SetBool("run", true);
        // Turn off the Animator's run Parameter
        bool hSpeed = Mathf.Abs(playerCharacter.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("run", hSpeed);

        // print(runVelocity);
    }
    private void FlipSprite()
    {
        // If player is moving
        bool hMovement = Mathf.Abs(playerCharacter.velocity.x) > Mathf.Epsilon;
        if (hMovement)
        {
            // Reverse the current scaling of the X axis
            transform.localScale = new Vector2(Mathf.Sign(playerCharacter.velocity.x), 1f);
        }
    }

    private void Jump()
    {
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            // Will stop this function if false
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            // Get new Y velocity based on a controllable variable
            Vector2 jumpVelocity = new Vector2(0.0f, jumpSpeed);
            playerCharacter.velocity += jumpVelocity;

            if (jump)
            {
                audioSource.PlayOneShot(jumpSound, 0.7F);
            }
        }
    }

    private void Climb()
    {
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerAnimator.SetBool("climb", false);
            playerCharacter.gravityScale = gravityScaleAtStart;
            return;
        }

        // "Vertical from Input axes
        float vMovement = Input.GetAxis("Vertical");
        // X needs to remain the same as we change Y
        Vector2 climbVelocity = new Vector2(playerCharacter.velocity.x, vMovement * climbSpeed);
        playerCharacter.velocity = climbVelocity;

        playerCharacter.gravityScale = 0.0f;

        bool vSpeed = Mathf.Abs(playerCharacter.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("climb", vSpeed);
    }

    private void DetectCeiling()
    {
        if (!playerHeadCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
    }

    private void Die()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            audioSource.PlayOneShot(deathSound, 0.7F);
            isAlive = false;
            playerAnimator.SetTrigger("die");
            GetComponent<Rigidbody2D>().velocity = deathSeq;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

}
