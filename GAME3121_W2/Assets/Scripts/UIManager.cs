using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject InstructionsText;
    [SerializeField] private GameObject BackButton;
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject SettingsButton;
    [SerializeField] private GameObject QuitButton;
    [SerializeField] private GameObject InstructionsButton;
    [SerializeField] private GameObject TitleText;
    [SerializeField] private GameObject SettingsText;
    [SerializeField] private GameObject SlowButton;
    [SerializeField] private GameObject MediumButton;
    [SerializeField] private GameObject FastButton;
    [SerializeField] private GameObject MusicSlider;
    [SerializeField] private GameObject MusicText;

    private EnemyMovement em;
    private float speed;
    public bool bIsPlaying = false;

    private PlayerController pc;

    public void Start()
    {
        em = FindAnyObjectByType<EnemyMovement>();
        bIsPlaying = false;

        pc = FindAnyObjectByType<PlayerController>();
    }

    public void OnPlayButton()
    {
        BG.gameObject.SetActive(false);
        Player.gameObject.SetActive(true);
        Enemy.gameObject.SetActive(true);


        em.SetSpeed(speed);
        bIsPlaying = true;

        
    }

    public void OnInstructionsButton()
    {
        InstructionsButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(true);
        PlayButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);
        SettingsButton.gameObject.SetActive(false);
        InstructionsText.gameObject.SetActive(true);
        TitleText.gameObject.SetActive(false);

    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnSettingsButton()
    {
        TitleText.gameObject.SetActive(false);
        SettingsText.gameObject.SetActive(true);
        PlayButton.gameObject.SetActive(false);
        InstructionsButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(false);
        SettingsButton.gameObject.SetActive(false);
        SlowButton.gameObject.SetActive(true);
        MediumButton.gameObject.SetActive(true);
        FastButton.gameObject.SetActive(true);
        MusicSlider.gameObject.SetActive(true);
        MusicText.gameObject.SetActive(true);
    }

    public void OnBackButton()
    {
        InstructionsButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(false);
        PlayButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);
        SettingsButton.gameObject.SetActive(true);
        InstructionsText.gameObject.SetActive(false);
        TitleText.gameObject.SetActive(true);
        SettingsText.gameObject.SetActive(false);
        SlowButton.gameObject.SetActive(false);
        MediumButton.gameObject.SetActive(false);
        FastButton.gameObject.SetActive(false);
        MusicSlider.gameObject.SetActive(false);
        MusicText.gameObject.SetActive(false);
    }

    
    public void OnSlowButton()
    {
        speed = 1.0f;
    }

    public void OnMeduimButton()
    {
        speed = 2.0f;
    }

    public void OnFastButton()
    {
        speed = 3.0f;
    }

    public void OnMusicSlider(float volumeLevel)
    {
        pc.musicSource.volume = volumeLevel;
    }

}
