using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    private NavMeshAgent agent;

    AudioSource audioSource;
    [SerializeField] AudioClip explosion;

    [SerializeField] GameObject explosionEffect;

    private UIManager ui;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        ui = FindAnyObjectByType<UIManager>();
    }

    void Update()
    {
        if (ui.bIsPlaying && agent.speed == 0.0f)
        {
            agent.speed = 2.0f;
        }

        if (player != null)
        {
            agent.SetDestination(player.position);
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("playerChar"))
        {
            Vector3 explosionPos = collision.gameObject.transform.position;
            GameObject explosionParticleSpawn = Instantiate(explosionEffect, explosionPos, Quaternion.identity);
            Destroy(explosionParticleSpawn, 2.0f);
            audioSource.PlayOneShot(explosion, 0.2f);

        }
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }
}
