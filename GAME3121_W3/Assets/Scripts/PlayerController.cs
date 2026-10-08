using System;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float thrustForce = 1.0f;
    Rigidbody2D rb;
    [SerializeField] float maxSpeed = 5.0f;
    [SerializeField] GameObject booster;

    private float elapsedTime = 0.0f;
    [SerializeField] float scoreMultiplier = 10.0f;
    private float score = 0.0f;

    [SerializeField] private PanelRenderer panel;
    [SerializeField] private VisualElementReference<Label> labelReference;
    private Label scoreText;

    [SerializeField] GameObject ps;

    [SerializeField] private VisualElementReference<Label> highScoreLabelReference;
    private Label highScoreText;

    [SerializeField] private VisualElementReference<Button> restartButtonReference;
    private Button restartButton;

    [SerializeField] AudioClip wallHit;
    [SerializeField] AudioClip bgMusic;
    AudioSource audioSource;
    AudioSource musicSource;

    [SerializeField] AudioClip obstacleBeep;

    [SerializeField] float beepStartDistance = 8.0f;
    [SerializeField] float fastestBeepInterval = 0.08f;
    [SerializeField] float slowestBeepInterval = 0.8f;

    private AudioSource beepSource;
    private float beepTimer = 0.0f;
    private Transform closestObstacle;
    private float previousDistance;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.2f;
        musicSource.clip = bgMusic;
        musicSource.Play();

        beepSource = gameObject.AddComponent<AudioSource>();
        beepSource.playOnAwake = false;
        beepSource.volume = 0.65f;
    }


    void Update()
    {
        elapsedTime += Time.deltaTime;

        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);

        if (scoreText != null)
        {

            scoreText.text = "Score: " + score.ToString();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            transform.up = direction;

            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }


        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            booster.gameObject.SetActive(true);
        }

        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            booster.gameObject.SetActive(false);
        }

        UpdateObstacleWarning();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            
            SaveHighScore();

            if (restartButton != null)
            {
                restartButton.style.display = DisplayStyle.Flex;
            }

            GameObject particleSystemPrefab;
            particleSystemPrefab = Instantiate(ps, this.gameObject.transform.position, Quaternion.identity);

            Destroy(particleSystemPrefab, 2.0f);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            audioSource.PlayOneShot(wallHit, 3.0f);
        }

    }

    private void UpdateObstacleWarning()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        float closestDistance = Mathf.Infinity;
        Rigidbody2D closestRb = null;

        foreach (GameObject obstacle in obstacles)
        {
            float distance = Vector2.Distance(transform.position, obstacle.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObstacle = obstacle.transform;
                closestRb = obstacle.GetComponent<Rigidbody2D>();
            }
        }

        if (closestObstacle == null || closestDistance > beepStartDistance)
        {
            beepTimer = 0.0f;
            previousDistance = closestDistance;
            return;
        }

        float distanceChange = closestDistance - previousDistance;

        beepTimer -= Time.deltaTime;

        if (beepTimer <= 0.0f)
        {
            beepSource.PlayOneShot(obstacleBeep);

            float distancePercent = Mathf.InverseLerp(beepStartDistance, 0.0f, closestDistance);

            float beepInterval = Mathf.Lerp(slowestBeepInterval, fastestBeepInterval, distancePercent);

            if (distanceChange > 0.0f)
            {
                beepInterval *= 1.5f;
            }

            beepTimer = beepInterval;
        }

        previousDistance = closestDistance;
    }

    private void Awake()
    {

        panel.RegisterUIReloadCallback(OnUIReload);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        panel.UnregisterUIReloadCallback(OnUIReload);
    }

    private void OnDestroy()
    {
        labelReference.UnregisterReferenceUnloadedCallback(HandleLabelReferenceCallback);
        highScoreLabelReference.UnregisterReferenceUnloadedCallback(HandleHighScoreLabelReferenceCallback);
        restartButtonReference.UnregisterReferenceUnloadedCallback(HandleRestartButtonReferenceCallback);
    }

    private void OnUIReload(PanelRenderer panel, VisualElement root, int version)
    {

        labelReference.RegisterReferenceResolvedCallback(HandleLabelReferenceCallback);
        highScoreLabelReference.RegisterReferenceResolvedCallback(HandleHighScoreLabelReferenceCallback);
        restartButtonReference.RegisterReferenceResolvedCallback(HandleRestartButtonReferenceCallback);
    }

    private void HandleLabelReferenceCallback(Label label)
    {

        scoreText = label;
    }

    private void SaveHighScore()
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", Mathf.FloorToInt(score));
            PlayerPrefs.Save();
        }
    }

    private void UpdateHighScoreDisplay()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    private void HandleHighScoreLabelReferenceCallback(Label label)
    {
        highScoreText = label;
        UpdateHighScoreDisplay();
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void HandleRestartButtonReferenceCallback(Button button)
    {
        restartButton = button;

        restartButton.style.display = DisplayStyle.None;

        restartButton.clicked += RestartGame;
    }

}
