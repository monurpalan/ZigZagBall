using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject diamondPrefab;
    [SerializeField] private GameObject player;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text diamondText;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private TMP_Text gamesText;

    [Header("Game Settings")]
    [SerializeField] private float playerSpeed = 15f;
    [SerializeField] private int initialPlatforms = 10;
    [SerializeField] private float platformSpawnInterval = 0.2f;
    [Range(0, 1)]
    [SerializeField] private float diamondSpawnChance = 0.25f;

    public static GameManager Instance;

    private enum GameState { Start, Playing, GameOver }
    private GameState currentState = GameState.Start;

    private bool isMovingRight = true;

    private float platformSize;
    private Vector3 lastPose;

    private int score = 0;
    private int highScore = 0;
    private int diamonds = 0;
    private int games = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadPlayerData();
        UpdateUI();

        lastPose = blockPrefab.transform.position;
        platformSize = blockPrefab.transform.localScale.x;
        for (int i = 0; i < initialPlatforms; i++)
            SpawnPlatforms();
    }

    private void LoadPlayerData()
    {
        diamonds = PlayerPrefs.GetInt("Diamonds", 0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        games = PlayerPrefs.GetInt("Games", 0);
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Start:
                if (Input.GetMouseButtonDown(0))
                {
                    StartGame();
                }
                break;
            case GameState.Playing:
                if (Input.GetMouseButtonDown(0))
                {
                    ChangeDirection();
                }
                break;
            case GameState.GameOver:
                if (Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                break;
        }
    }

    private void StartGame()
    {
        currentState = GameState.Playing;
        startPanel.SetActive(false);
        player.GetComponent<Rigidbody>().velocity = Vector3.right * playerSpeed;
        BlockSpawn();
    }

    private void ChangeDirection()
    {
        isMovingRight = !isMovingRight;
        player.GetComponent<Rigidbody>().velocity = (isMovingRight ? Vector3.right : Vector3.back) * playerSpeed;
    }
    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void UpdateDiamonds()
    {
        diamonds++;
        diamondText.text = diamonds.ToString();
        PlayerPrefs.SetInt("Diamonds", diamonds);
    }

    public void GameOver()
    {
        if (currentState == GameState.GameOver) return;
        currentState = GameState.GameOver;
        startPanel.SetActive(true);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            highscoreText.text = $"Highscore: {highScore}";
        }

        games++;
        PlayerPrefs.SetInt("Games", games);
        gamesText.text = $"Games Played: {games}";

        CancelInvoke(nameof(SpawnPlatforms));
    }

    private void SpawnPlatforms()
    {
        // Rastgele bir sonraki platformu ve üzerinde elmas olup olmayacağını belirler.
        Vector3 pos = lastPose;

        if (Random.value > 0.5f)
            pos.x += platformSize;
        else
            pos.z -= platformSize;

        Instantiate(blockPrefab, pos, Quaternion.identity);
        lastPose = pos;

        // Belirlenen şansa göre platformun üzerine bir elmas yerleştirir.
        if (Random.value < diamondSpawnChance)
            Instantiate(diamondPrefab, lastPose + Vector3.up * 6f, Quaternion.identity);
    }

    private void BlockSpawn()
    {
        // Belirli aralıklarla sürekli olarak yeni platformlar oluşturur.
        InvokeRepeating(nameof(SpawnPlatforms), platformSpawnInterval, platformSpawnInterval);
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();
        diamondText.text = diamonds.ToString();
        highscoreText.text = $"Highscore: {highScore}";
        gamesText.text = $"Games Played: {games}";
    }
}