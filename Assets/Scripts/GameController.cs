using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject soccerBall;
    public GameObject target;
    public Text scoreText;
    public Slider countdownTimerSlider;
    public GameObject countdownTimerSliderFill;
    public Text startGamePrompt;
    public GameObject mainMenuPanel;
    public GameObject playPanel;
    public GameObject optionsPanel;
    public GameObject gameOverPanel;
    public Text gameOverScoreText;
    public Text gameOverHighScoreText;
    public Text newHighScorePromptText;
    public Text juggleHighScoreText;
    public Text timeTrialHighScoreText;
    public Text targetPracticeHighScoreText;
    public Text beatTheClockHighScoreText;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject losingCollider;
    public AudioSource highScoreResetSound;

    private Transform soccerBallTransform;
    private Rigidbody soccerBallRigidbody;
    private Vector3 originalPosition;
    private TargetController targetController;
    private Color originalScoreTextColor;
    [Range(0, 3)]
    private int gameMode;
    private const int JuggleGameMode = 0;
    private const int TimeTrialGameMode = 1;
    private const int TargetPracticeGameMode = 2;
    private const int BeatTheClockGameMode = 3;
    private int score;
    private float scoreTime;
    private float countdownTimer;
    private int juggleHighScore;
    private float timeTrialHighScore;
    private int targetPracticeHighScore;
    private float beatTheClockHighScore;

    private void Awake()
    {
        soccerBallTransform = soccerBall.GetComponent<Transform>();
        soccerBallRigidbody = soccerBall.GetComponent<Rigidbody>();
        targetController = target.GetComponent<TargetController>();

        // Soccer Ball
        soccerBall.SetActive(false);
        soccerBallRigidbody.useGravity = false;
        originalPosition = soccerBallTransform.position;

        // UI
        originalScoreTextColor = scoreText.color;

        // States
        gameMode = JuggleGameMode;
        score = 0;
        scoreTime = 0;
        juggleHighScore = PlayerPrefs.GetInt("JuggleHighScore", 0);
        juggleHighScoreText.text = $"HIGH SCORE: {juggleHighScore.ToString()}";
        timeTrialHighScore = PlayerPrefs.GetFloat("TimeTrialHighScore", 0);
        timeTrialHighScoreText.text = $"HIGH SCORE: {timeTrialHighScore.ToString("0.00")} SECONDS";
        targetPracticeHighScore = PlayerPrefs.GetInt("TargetPracticeHighScore", 0);
        targetPracticeHighScoreText.text = $"HIGH SCORE: {targetPracticeHighScore.ToString()}";
        beatTheClockHighScore = PlayerPrefs.GetFloat("BeatTheClockHighScore", 0);
        beatTheClockHighScoreText.text = $"HIGH SCORE: {beatTheClockHighScore.ToString("0.00")} SECONDS";
    }

    void Update()
    {
        if (startGamePrompt.enabled && Input.GetMouseButtonDown(0))
            StartGame();

        if (gameOverPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
                LoadMainMenu();

            if (Input.GetMouseButtonDown(0))
                ResetGame(gameMode);
        }
    }

    public void LoadMainMenu()
    {
        gameOverPanel.SetActive(false);
        playPanel.SetActive(false);
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void LoadPlayPanel()
    {
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void LoadOptionsPanel()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    private void StartGame()
    {
        startGamePrompt.enabled = false;
        soccerBallRigidbody.useGravity = true;
        soccerBallRigidbody.mass = 0.1f;

        // Boundaries
        losingCollider.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.1f, 2.0f));
        leftWall.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 2.0f));
        rightWall.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 2.0f));

        if (gameMode == TimeTrialGameMode || gameMode == BeatTheClockGameMode)
            StartCoroutine(RunTimer());

        if (gameMode == TargetPracticeGameMode || gameMode == BeatTheClockGameMode)
            target.SetActive(true);

        if (gameMode == BeatTheClockGameMode)
        {
            countdownTimerSliderFill.SetActive(true);
            StartCoroutine(RunCountdownTimer());
        }
    }

    public void EndGame()
    {
        // Soccer Ball
        soccerBall.SetActive(false);
        soccerBallRigidbody.velocity = new Vector3(0, 0, 0);
        soccerBallRigidbody.useGravity = false;

        // Target
        target.SetActive(false);

        // UI
        gameOverPanel.SetActive(true);
        scoreText.enabled = false;
        StopAllCoroutines();

        // High Scores
        switch (gameMode)
        {
            case JuggleGameMode:
                gameOverScoreText.rectTransform.localPosition = new Vector3(350.0f, 40.0f, 0);
                gameOverScoreText.text = $"SCORE: {score.ToString()}";
                gameOverHighScoreText.text = $"HIGH SCORE: {(score > juggleHighScore ? score.ToString() : juggleHighScore.ToString())}";

                if (score > juggleHighScore)
                {
                    juggleHighScore = score;
                    newHighScorePromptText.enabled = true;
                    PlayerPrefs.SetInt("JuggleHighScore", juggleHighScore);
                    juggleHighScoreText.text = $"HIGH SCORE: {juggleHighScore.ToString()}";
                }
                break;
            case TimeTrialGameMode:
                gameOverScoreText.rectTransform.localPosition = new Vector3(402.8f, 40.0f, 0);
                gameOverScoreText.text = $"TIME: {scoreTime.ToString("0.00")} SECONDS";
                gameOverHighScoreText.text = $"HIGH SCORE: {(scoreTime > timeTrialHighScore ? scoreTime.ToString("0.00") : timeTrialHighScore.ToString("0.00"))} SECONDS";

                if (scoreTime > timeTrialHighScore)
                {
                    timeTrialHighScore = scoreTime;
                    newHighScorePromptText.enabled = true;
                    PlayerPrefs.SetFloat("TimeTrialHighScore", timeTrialHighScore);
                    timeTrialHighScoreText.text = $"HIGH SCORE: {timeTrialHighScore.ToString("0.00")} SECONDS";
                }
                break;
            case TargetPracticeGameMode:
                gameOverScoreText.rectTransform.localPosition = new Vector3(350.0f, 40.0f, 0);
                gameOverScoreText.text = $"SCORE: {score.ToString()}";
                gameOverHighScoreText.text = $"HIGH SCORE: {(score > targetPracticeHighScore ? score.ToString() : targetPracticeHighScore.ToString())}";

                if (score > targetPracticeHighScore)
                {
                    targetPracticeHighScore = score;
                    newHighScorePromptText.enabled = true;
                    PlayerPrefs.SetInt("TargetPracticeHighScore", targetPracticeHighScore);
                    targetPracticeHighScoreText.text = $"HIGH SCORE: {targetPracticeHighScore.ToString()}";
                }
                break;
            case BeatTheClockGameMode:
                gameOverScoreText.rectTransform.localPosition = new Vector3(402.8f, 40.0f, 0);
                gameOverScoreText.text = $"TIME: {scoreTime.ToString("0.00")} SECONDS";
                gameOverHighScoreText.text = $"HIGH SCORE: {(scoreTime > beatTheClockHighScore ? scoreTime.ToString("0.00") : beatTheClockHighScore.ToString("0.00"))} SECONDS";
                countdownTimerSliderFill.SetActive(false);

                if (scoreTime > beatTheClockHighScore)
                {
                    beatTheClockHighScore = scoreTime;
                    newHighScorePromptText.enabled = true;
                    PlayerPrefs.SetFloat("BeatTheClockHighScore", beatTheClockHighScore);
                    beatTheClockHighScoreText.text = $"HIGH SCORE: {beatTheClockHighScore.ToString("0.00")} SECONDS";
                }
                break;
        }
    }

    public void ResetGame(int mode)
    {
        gameMode = mode;
        soccerBall.SetActive(true);
        soccerBallTransform.position = originalPosition;
        soccerBallRigidbody.angularVelocity = Vector3.zero;
        newHighScorePromptText.enabled = false;
        scoreText.enabled = true;
        scoreText.color = originalScoreTextColor;
        playPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        startGamePrompt.enabled = true;

        switch (gameMode)
        {
            case JuggleGameMode:
                score = 0;
                scoreText.text = score.ToString();
                break;
            case TimeTrialGameMode:
                scoreTime = 0;
                scoreText.text = scoreTime.ToString("0.00");
                break;
            case TargetPracticeGameMode:
                score = 0;
                scoreText.text = score.ToString();
                targetController.ResetTargetPosition();
                break;
            case BeatTheClockGameMode:
                scoreTime = 0;
                countdownTimer = 15.0f;
                scoreText.text = scoreTime.ToString("0.00");
                targetController.ResetTargetPosition();
                break;
        }
    }

    public void HitJuggler()
    {
        if (gameMode == JuggleGameMode)
        {
            score++;
            scoreText.text = score.ToString();

            if (score > juggleHighScore)
                scoreText.color = gameOverHighScoreText.color;
        }
    }

    public void HitTarget()
    {
        switch (gameMode)
        {
            case TargetPracticeGameMode:
                score++;
                scoreText.text = score.ToString();

                if (score > targetPracticeHighScore)
                    scoreText.color = gameOverHighScoreText.color;
                break;
            case BeatTheClockGameMode:
                countdownTimer += 5.0f;

                if (countdownTimer > 15.0f)
                    countdownTimer = 15.0f;
                break;
        }
    }

    private IEnumerator RunTimer()
    {
        while (soccerBall.activeSelf)
        {
            scoreTime += 0.01f;
            scoreText.text = scoreTime.ToString("0.00");

            switch (gameMode)
            {
                case TimeTrialGameMode:
                    if (scoreTime > timeTrialHighScore)
                        scoreText.color = gameOverHighScoreText.color;
                    break;
                case BeatTheClockGameMode:
                    if (scoreTime > beatTheClockHighScore)
                        scoreText.color = gameOverHighScoreText.color;
                    break;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator RunCountdownTimer()
    {
        while (countdownTimer > 0)
        {
            countdownTimer -= 0.01f;
            countdownTimerSlider.value = countdownTimer / 15.0f;

            yield return new WaitForSeconds(0.01f);
        }

        if (countdownTimer <= 0)
            EndGame();
    }

    public void FadeInText(Text text)
    {
        text.CrossFadeAlpha(1.0f, 1.0f, false);
    }

    public void FadeOutText(Text text)
    {
        text.CrossFadeAlpha(0, 1.0f, false);
    }

    public void ResetHighScores()
    {
        juggleHighScore = 0;
        timeTrialHighScore = 0;
        targetPracticeHighScore = 0;
        beatTheClockHighScore = 0;
        juggleHighScoreText.text = $"HIGH SCORE: {juggleHighScore.ToString()}";
        timeTrialHighScoreText.text = $"HIGH SCORE: {timeTrialHighScore.ToString("0.00")} SECONDS";
        targetPracticeHighScoreText.text = $"HIGH SCORE: {targetPracticeHighScore.ToString()}";
        beatTheClockHighScoreText.text = $"HIGH SCORE: {beatTheClockHighScore.ToString("0.00")} SECONDS";
        PlayerPrefs.SetInt("JuggleHighScore", 0);
        PlayerPrefs.SetFloat("TimeTrialHighScore", 0);
        PlayerPrefs.SetInt("TargetPracticeHighScore", 0);
        PlayerPrefs.SetFloat("BeatTheClockHighScore", 0);
        highScoreResetSound.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
