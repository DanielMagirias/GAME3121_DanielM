using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    private NavMeshAgent agent;

    AudioSource audioSource;
    [SerializeField] AudioClip explosion;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("playerChar"))
        {
            audioSource.PlayOneShot(explosion, 0.2f);

        }
    }
}
