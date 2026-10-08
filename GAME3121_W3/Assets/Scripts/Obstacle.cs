using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] float minSize = 0.5f;
    [SerializeField] float maxSize = 2.0f;
    [SerializeField] float minSpeed = 50.0f;
    [SerializeField] float maxSpeed = 150.0f;
    [SerializeField] float maxSpinSpeed = 10.0f;
    [SerializeField] GameObject ps;

    Rigidbody2D rb;

    [SerializeField] AudioClip obstacleCollision;
    [SerializeField] AudioClip playerCollision;
    AudioSource audioSource;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

        //size
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        //speed
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;
        rb.AddForce(randomDirection * randomSpeed);

        //rotation
        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            audioSource.PlayOneShot(obstacleCollision, 0.75f);
            GameObject particleSystemPrefab;
            particleSystemPrefab = Instantiate(ps, this.gameObject.transform.position, Quaternion.identity);
            Destroy(particleSystemPrefab, 2.0f);
            
        }

        if (collision.gameObject.CompareTag("player"))
        {

            audioSource.PlayOneShot(playerCollision, 0.25f);
        }

    }

    void Update()
    {
        
    }
}
