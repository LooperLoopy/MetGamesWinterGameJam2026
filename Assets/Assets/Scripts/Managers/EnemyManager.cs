using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float enemySpawnChance = 0.7f;

    [Header("workspace")]
    [SerializeField] private Transform workspace;
    private Transform prefabs;

    [Header("Enemies")]
    [SerializeField] private EnemyBehaviour Enemy1;

    private EnemyBehaviour[] randomEnemy;

    private System.Random random = new System.Random();

    private List<EnemyBehaviour> instances;

    private EnvironmentManager environmentManager;

    private static readonly List<Key> alphaKeys = new List<Key>
    {
        Key.A, Key.B, Key.C, Key.D, Key.E,
        Key.F, Key.G, Key.H, Key.I, Key.J,
        Key.K, Key.L, Key.M, Key.N, Key.O,
        Key.P, Key.Q, Key.R, Key.S, Key.T,
        Key.U, Key.V, Key.W, Key.X, Key.Y,
        Key.Z
    };

    void Awake()
    {
        Instance = this;

        instances = new List<EnemyBehaviour>();
        
        prefabs = workspace.Find("Prefabs");

        randomEnemy = new EnemyBehaviour[]
        {
            Enemy1
        };
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        environmentManager = EnvironmentManager.Instance;
    }

    public void spawnEnemies()
    {
        List<Key> alphaKeysCopy = new List<Key>(alphaKeys);

        foreach (Transform t in environmentManager.getEnemyPoints())
        {
            float ran = (float)random.NextDouble();

            if (enemySpawnChance <= ran)
            {
                continue;
            }

            EnemyBehaviour randomEn = randomEnemy[UnityEngine.Random.Range(0, randomEnemy.Length)];

            EnemyBehaviour instance = Instantiate(randomEn, t.position, Quaternion.identity);
            instance.transform.SetParent(prefabs);

            Transform weakPoint = instance.transform.Find("WeakPoint").transform;

            instances.Add(instance);

            Key randomKey = alphaKeysCopy[Random.Range(0, alphaKeysCopy.Count)];
            alphaKeysCopy.Remove(randomKey);

            instance.Initialize(weakPoint, randomKey);
        }

        if (instances.Count == 0)
        {
            spawnEnemies();
        }
    }

    public void Check()
    {
        if (instances.Count == 0)
        {
            GameManager.newRoom.Invoke();
        }
    }

    public void Clear()
    {
        instances.Clear();
    }

    public void hitEnemy(EnemyBehaviour e)
    {
        Destroy(e);
        instances.Remove(e);
    }

    public void showHitPoints()
    {
        foreach (EnemyBehaviour instance in instances)
        {
            instance.showButton();
        }
    }

    public void hideHitPoints()
    {
        foreach (EnemyBehaviour instance in instances)
        {
            instance.hideButton();
        }
    }
}
