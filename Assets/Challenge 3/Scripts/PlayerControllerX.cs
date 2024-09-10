using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip boundTouch;

    //define the top and bottom boundaries (havent done anything with these ones yet)
    [SerializeField] private Transform topBorder; //reference to the empty GameObject defining the top boundary
    [SerializeField] private Transform bottomBorder;

    private bool isLowEnough;
    private bool hasTouchedBottom = false;

        


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        playerRb = GetComponent<Rigidbody>();
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

        
    }

    // Update is called once per frame
    void Update()
    {
        borderRestriction();

            // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && isLowEnough)
        {
            Debug.Log("Space Key Was Pressed!");
            playerRb.AddForce(Vector3.up * floatForce);
            Debug.Log("the space bar works");
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            fireworksParticle.Stop();
            explosionParticle.Play();
            
            Debug.Log("The fireworks particle effect doesn't work");
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");  
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);


        }


    }


    private void borderRestriction()
    {
        // Code for top and bottom border
        if (transform.position.y > topBorder.position.y)
        {

            transform.position = new Vector3(transform.position.x, topBorder.position.y, transform.position.z);
            isLowEnough = false;
            Debug.Log("Player is touching top bound");
        }
        else if (transform.position.y < bottomBorder.position.y)
        {
            hasTouchedBottom = true;
            playerAudio.PlayOneShot(boundTouch, 1.0f);
            transform.position = new Vector3(transform.position.x, bottomBorder.position.y, transform.position.z);
            isLowEnough = true;
            
            Debug.Log("Player is touching bottom bound");
        }




    }

}
