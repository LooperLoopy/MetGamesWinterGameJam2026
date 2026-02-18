using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Objects")]
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject playerCamera;
    [SerializeField] public GameObject UISpace;
    
    private Vector3 camStart;

    //Events
    public static UnityEvent onPlayerSwitch = new UnityEvent();
    public static UnityEvent engageToggle = new UnityEvent();
    public static UnityEvent endGame = new UnityEvent();
    public static UnityEvent newRoom = new UnityEvent();
    public static UnityEvent<Vector3> moveCamera = new UnityEvent<Vector3>();
    public static UnityEvent<EnemyBehaviour> addToEnemyQ = new UnityEvent<EnemyBehaviour>();

    //All Managers
    private static MenuManager menuManager;
    private static EnvironmentManager environmentManager;
    private static EnemyManager enemyManager;
    private static TargetManager targetManager;
    private PlayerController playerController;

    // Gameplay
    [Header("Gameplay")]
    private bool isEngaged = false;
    private List<EnemyBehaviour> enemiesQ;
    [SerializeField] private float time = 30f;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        camStart = playerCamera.transform.position;

        enemiesQ = new List<EnemyBehaviour>();

        menuManager = MenuManager.Instance;
        environmentManager = EnvironmentManager.Instance;
        enemyManager = EnemyManager.Instance;
        targetManager = TargetManager.Instance;

        environmentManager.spawnObjects();
    }
    void OnEnable()
    {
        onPlayerSwitch.AddListener(switchPlayerPos);
        moveCamera.AddListener(moveCam);
        engageToggle.AddListener(toggleEngage);
        addToEnemyQ.AddListener(addToQ);
        newRoom.AddListener(moveToNewRoom);
    }
    void OnDisable()
    {
        onPlayerSwitch.RemoveListener(switchPlayerPos);
        moveCamera.RemoveListener(moveCam);
        engageToggle.RemoveListener(toggleEngage);
        addToEnemyQ.RemoveListener(addToQ);
        newRoom.RemoveListener(moveToNewRoom);
    }

    private void moveToNewRoom()
    {
        environmentManager.clearObjects();
        environmentManager.spawnObjects();
    }

    private void addToQ(EnemyBehaviour e)
    {
        if (enemiesQ.Contains(e))
        {
            enemiesQ.Remove(e);
        }
        else
        {
            enemiesQ.Add(e);
        }
    }

    private void moveCam(Vector3 v)
    {
        if (v.magnitude == 0)
        {
            playerCamera.transform.position = camStart;
            return;
        }
        playerCamera.transform.position += v;
    }

    private async void toggleEngage()
    {
        Vector3 v = new Vector3(0, 1, 0);
        Vector3 reset = new Vector3(0, 0, 0);

        if (!isEngaged)
        {
            menuManager.openEngage();
            moveCamera.Invoke(v);
            enemyManager.showHitPoints();
        }
        else
        {
            enemyManager.hideHitPoints();

            if (enemiesQ.Count > 0)
            {
                Debug.Log(enemiesQ.Count);
                await targetManager.spawnHits(enemiesQ);
                enemiesQ.Clear();
            }

            menuManager.openOptions();
            moveCamera.Invoke(reset);

            enemyManager.Check();
        }
        
        isEngaged = !isEngaged;
    }

    private void switchPlayerPos()
    {
        playerController.switchSides();
        menuManager.switchMenuSides();
    }
    public bool isPlayerRight()
    {
        return playerController.playerIsRight();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && isEngaged)
        {
            targetManager.onClick();
        }
    }
}
