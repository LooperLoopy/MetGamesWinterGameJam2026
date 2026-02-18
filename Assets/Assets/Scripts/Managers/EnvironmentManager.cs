using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance { get; private set; }

    [Header("workspace")]
    [SerializeField] private Transform workspace;
    private Transform ObjectPoints;
    private Transform prefabs;

    [Header("Objects")]
    [SerializeField] private ObjectBehaviour Obj1;

    private List<Transform> ObjectSpawnPoints;
    private List<Transform> EnemySpawnPoints;

    private EnemyManager enemyManager;

    void Awake()
    {
        Instance = this;

        ObjectSpawnPoints = new List<Transform>();
        EnemySpawnPoints = new List<Transform>();

        prefabs = workspace.Find("Prefabs");
        ObjectPoints = workspace.Find("ObjectSpawnPoints");

        // Get all pre-spawn points
        foreach (Transform child in ObjectPoints)
        {
            ObjectSpawnPoints.Add(child);
        }
    }

    void Start()
    {
        enemyManager = EnemyManager.Instance;
    }

    public void spawnObjects()
    {
        if (enemyManager == null)
            enemyManager = EnemyManager.Instance;

        foreach (Transform t in ObjectSpawnPoints)
        {
            ObjectBehaviour instance = Instantiate(Obj1, t.position, Quaternion.identity);
            instance.transform.SetParent(prefabs);

            foreach (Transform spawnPoint in instance.transform.Find("SpawnPoints").transform)
            {
                EnemySpawnPoints.Add(spawnPoint);
            }

            instance.Initialize(5);
        }

        enemyManager.spawnEnemies();
    }

    public void clearObjects()
    {
        foreach(Transform child in prefabs.transform)
        {
            Destroy(child.gameObject);
        }
        EnemySpawnPoints.Clear();
    }

    public List<Transform> getEnemyPoints()
    {
        return EnemySpawnPoints;
    }
}
