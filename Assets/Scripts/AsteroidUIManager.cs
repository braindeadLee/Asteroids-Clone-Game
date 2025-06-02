using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AsteroidUIManager : MonoBehaviour
{

    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HighscoreText;
    public TextMeshProUGUI LifeText;
    public TextMeshProUGUI StartButtonText;
    public TextMeshProUGUI AnnouncementText;

    public Button StartButton;

    public float TitleAnimationDuration = 3f;
    public float StartButtonFlickerSpeed = 0.09f;

    public static event UnityAction OnStartClicked;

    private Coroutine flickerCoroutine;
    private void Start() => StartButton.onClick.AddListener(OnStartClick);

    private void OnDestroy() => StartButton.onClick.RemoveListener(OnStartClick);

    public void StartingScreen()
    {
        ScoreText.text = string.Empty;
        HighscoreText.text = string.Empty;
        LifeText.text = string.Empty;
        AnnouncementText.text = string.Empty;

        TitleText.gameObject.SetActive(true);
        StartButton.gameObject.SetActive(true);

        StopFlicker();

        flickerCoroutine = StartCoroutine(StartButtonFlicker(StartButtonFlickerSpeed));

        StartCoroutine(AnimateTitleText(40f, -80f, 250f, 1.8f, TitleAnimationDuration));
    }

    public void UpdateAnnouncementText(string text)
    {
        AnnouncementText.text = text;
    }

    public void UpdateLivesText(int lives)
    {
        string livesText = "";

        for(int i = 0; i < lives; i++)
        {
            livesText += "A";
        }

        LifeText.text = livesText;
    }

    public void UpdateScoreText(int score) => ScoreText.text = score.ToString();
    public void UpdateHighscoreText(int highscore) => HighscoreText.text = "Highscore: " + highscore.ToString();


    private void OnStartClick()
    {
        TitleText.gameObject.SetActive(false);

        StopFlicker();

        StartButton.gameObject.SetActive(false);

        OnStartClicked?.Invoke();
    }

    private IEnumerator AnimateTitleText(float sizeStart, float spacingStart, float sizeEnd, float spacingEnd, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            TitleText.fontSize = Mathf.Lerp(sizeStart, sizeEnd, t);
            TitleText.characterSpacing = Mathf.Lerp(spacingStart, spacingEnd, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        TitleText.fontSize = sizeEnd;
        TitleText.characterSpacing = spacingEnd;
    }

    private IEnumerator StartButtonFlicker(float flickerSpeed)

    {
        while (true)
        {
            if (StartButtonText.color == Color.white)
            {
                StartButtonText.color = Color.black;
            } else
            {
                StartButtonText.color = Color.white;
            }

            yield return new WaitForSeconds(flickerSpeed);
        }
    }

    private void StopFlicker()
    {
        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);
        StartButtonText.color = Color.white;
    }
}
