using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private Transform hitZone;
    [SerializeField] private CrosshairBehaviour crosshair;
    [SerializeField] private Canvas canvas;

    public static UnityEvent onMiss = new UnityEvent();
    public static UnityEvent onHit = new UnityEvent();

    private CrosshairBehaviour curr;
    private bool hasHit = false;

    void Awake()
    {
        Instance = this;
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
    }

    public async Task spawnHits(List<EnemyBehaviour> enemies)
    {
        hitZone.gameObject.SetActive(true);

        foreach (EnemyBehaviour enemy in enemies)
        {
            enemy.deselect();
            float difficulty = enemy.getDifficulty();

            float freq = difficulty * Random.Range(1, 2);
            float strength = difficulty * 10 * Random.Range(0.5f, 2f);
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
        }

        hitZone.gameObject.SetActive(false);
    }

    public void onClick()
    {
        if (curr != null)
        {
            curr.onHit();
        }
    }

    private void missed()
    {
        Debug.Log("Missed");
        hasHit = false;
    }

    private void hit()
    {
        Debug.Log("Hit");
        hasHit = true;
    }
}
