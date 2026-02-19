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
    [SerializeField] private ObjectBehaviour Obj2;

    private ObjectBehaviour[] randomObject;

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

        randomObject = new ObjectBehaviour[] 
        {
            Obj1,
            Obj2
        };
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
            ObjectBehaviour randomObj = randomObject[UnityEngine.Random.Range(0, randomObject.Length)];

            ObjectBehaviour instance = Instantiate(randomObj, t.position, Quaternion.identity);
            instance.transform.SetParent(prefabs);

            foreach (Transform spawnPoint in instance.transform.Find("SpawnPoints").transform)
            {
                EnemySpawnPoints.Add(spawnPoint);
            }

            instance.Initialize();
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
        enemyManager.Clear();
    }

    public List<Transform> getEnemyPoints()
    {
        return EnemySpawnPoints;
    }
}
