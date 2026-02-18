using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float enemySpawnChance = 0.5f;

    [Header("workspace")]
    [SerializeField] private Transform workspace;
    private Transform prefabs;

    [Header("Enemies")]
    [SerializeField] private EnemyBehaviour Enemy1;

    private System.Random random = new System.Random();

    private List<EnemyBehaviour> instances;

    private EnvironmentManager environmentManager;

    void Awake()
    {
        Instance = this;

        instances = new List<EnemyBehaviour>();
        
        prefabs = workspace.Find("Prefabs");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        environmentManager = EnvironmentManager.Instance;
    }

    public void spawnEnemies()
    {
        foreach (Transform t in environmentManager.getEnemyPoints())
        {
            float ran = (float)random.NextDouble();

            if (enemySpawnChance <= ran)
            {
                continue;
            }

            EnemyBehaviour instance = Instantiate(Enemy1, t.position, Quaternion.identity);
            instance.transform.SetParent(prefabs);

            Transform weakPoint = instance.transform.Find("WeakPoint").transform;

            instances.Add(instance);

            instance.Initialize(weakPoint);
        }
    }

    public void Check()
    {
        if (instances.Count == 0)
        {
            GameManager.newRoom.Invoke();
        }
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
