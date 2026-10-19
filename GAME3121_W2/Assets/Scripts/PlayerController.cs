using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    private float movementX;
    private float movementZ;
    [SerializeField] float movementSpeed = 5.0f;
    private int score;
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text winText;
    [SerializeField] NavMeshAgent Enemy;

    [SerializeField] AudioClip coinPickup;
    [SerializeField] AudioClip wallHit;
    AudioSource audioSource;

    [SerializeField] AudioClip bgMusic;
    [SerializeField] AudioClip winMusic;
    AudioSource musicSource;

    [SerializeField] GameObject pickupEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        winText.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.25f;
        musicSource.clip = bgMusic;
        musicSource.Play();
    }


    void FixedUpdate()
    {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementZ);
        rb.AddForce(movement * movementSpeed);
    }

    void OnMove (InputValue movementValue)
    {

        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementZ = movementVector.y;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            Vector3 pickupPos = other.gameObject.transform.position;
            GameObject pickupParticleSpawn = Instantiate(pickupEffect, pickupPos, Quaternion.identity);
            Destroy(pickupParticleSpawn, 2.0f);
            other.gameObject.SetActive(false);
            audioSource.PlayOneShot(coinPickup, 2.0f);
            score++;
            text.text = "Count: " + score.ToString();

            if (score == 12)
            {
                winText.gameObject.SetActive(true);
                Enemy.gameObject.SetActive(false);

                musicSource.Stop();
                musicSource.clip = winMusic;
                musicSource.Play();

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            this.gameObject.SetActive(false);
            winText.text = "You Lose!";
            winText.gameObject.SetActive(true);

        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            audioSource.PlayOneShot(wallHit, 3.0f);
        }
    }

}
