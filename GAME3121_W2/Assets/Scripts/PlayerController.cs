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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        winText.gameObject.SetActive(false);
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

            other.gameObject.SetActive(false);
            score++;
            text.text = "Count: " + score.ToString();

            if (score == 12)
            {
                winText.gameObject.SetActive(true);
                Enemy.gameObject.SetActive(false);
                
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
    }

}
