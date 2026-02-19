using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI susText;

    private static int score = 0;
    private static int suspicion = 0;

    public static UnityEvent<int> onMiss = new UnityEvent<int>();
    public static UnityEvent<int> onHit = new UnityEvent<int>();

    void Awake()
    {
        Instance = this;


    }

    void Start()
    {
        Reset();
    }

    void OnEnable()
    {
        onMiss.AddListener(addSuspicion);
        onHit.AddListener(addScore);
    }

    void OnDisable()
    {
        onMiss.RemoveListener(addSuspicion);
        onHit.RemoveListener(addScore);
    }

    public void Reset()
    {
        score = 0;
        suspicion = 0;

        scoreText.text = "Score:\n" + score;
        susText.text = "Suspicion Level:\n" + suspicion + "%";
    }

    public void addScore(int combo)
    {
        score += 1 + combo;

        scoreText.text = "Score:\n" + score;

        addSuspicion(-1 * (1 + combo));
    }

    public void addSuspicion(int susToAdd)
    {
        suspicion += susToAdd;
        suspicion = Math.Clamp(suspicion, 0, 100);

        susText.text = "Suspicion Level:\n" + suspicion + "%";

        if (suspicion == 100)
        {
            GameManager.Instance.gameEnd();
        }
    }
}
