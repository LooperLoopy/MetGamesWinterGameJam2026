using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private Transform hitZone;
    [SerializeField] private GameObject comboGUI;
    [SerializeField] private CrosshairBehaviour crosshair;
    [SerializeField] private Canvas canvas;

    private UnityEvent<int> onMiss;
    private UnityEvent<int> onHit;

    private CrosshairBehaviour curr;
    private bool hasHit = false;
    private int combo = 0;

    void Awake()
    {
        Instance = this;

        onHit = ScoreManager.onHit;
        onMiss = ScoreManager.onMiss;
    }

    void OnEnable()
    {
        onMiss.AddListener(missed);
        onHit.AddListener(hit);
    }

    void OnDisable()
    {
        onMiss.RemoveListener(missed);
        onHit.RemoveListener(hit);
    }

    void Start()
    {
        hitZone.gameObject.SetActive(false);
        comboGUI.SetActive(false);
    }

    public async Task spawnHits(List<EnemyBehaviour> enemies)
    {
        comboGUI.GetComponent<TextMeshProUGUI>().text = combo + 1 + "x";
        hitZone.gameObject.SetActive(true);
        comboGUI.SetActive(true);

        foreach (EnemyBehaviour enemy in enemies)
        {
            enemy.deselect();
            float difficulty = enemy.getDifficulty();

            float freq = difficulty * Random.Range(1, 2);
            float strength = difficulty * 5 * Random.Range(0.5f, 2f);
            float time = 3.5f / difficulty * Random.Range(0.9f, 1.1f);

            CrosshairBehaviour instance = Instantiate(crosshair, canvas.transform);
            instance.Initialize(freq, strength, time, CrosshairFunctions.getRandom(), CrosshairFunctions.GetRandomEdgePoint(canvas.GetComponent<RectTransform>()));
            
            curr = instance;

            await instance.Completion;

            if (hasHit)
            {
                enemy.onHit();
            }

            Destroy(instance.gameObject);

            if (!GameManager.Instance.isGaming())
            {
                break;
            }
        }

        curr = null;
        combo = 0;
        hitZone.gameObject.SetActive(false);
        comboGUI.SetActive(false);
    }

    public void onClick()
    {
        if (curr != null)
        {
            curr.onHit(combo);
        }
    }

    private void missed(int sus)
    {
        Debug.Log("Missed");
        hasHit = false;
        combo = 0;
        comboGUI.GetComponent<TextMeshProUGUI>().text = combo + 1 + "x";
    }

    private void hit(int c)
    {
        Debug.Log("Hit");
        hasHit = true;
        combo += 1;
        comboGUI.GetComponent<TextMeshProUGUI>().text = combo + 1 + "x";
    }
}
