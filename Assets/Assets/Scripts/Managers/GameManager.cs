using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private static ScoreManager scoreManager;
    private PlayerController playerController;

    // Input
    [Header("Input Asset")]
    [SerializeField] private InputActionAsset inputAsset;
    private InputActionMap playerMap;
    private InputAction attackAction;

     
    // Gameplay
    [Header("Gameplay")]
    [SerializeField] private float time = 30f;
    [SerializeField] private int engageTime = 700;
    private bool isEngaged = false;
    private bool canAttack = false;
    private bool isPlaying = false;
    private bool engaging = false;
    private List<EnemyBehaviour> enemiesQ;
    


    private void Awake()
    {
        Instance = this;

        playerMap = inputAsset.FindActionMap("Player");

        attackAction = playerMap.FindAction("Attack");
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
        scoreManager = ScoreManager.Instance;
    }

    public void startGame()
    {
        isPlaying = true;
        menuManager.openOptions();
        environmentManager.spawnObjects();
        scoreManager.Reset();
    }

    public void gameEnd()
    {
        isPlaying = false;
        menuManager.openStart();
        environmentManager.clearObjects();
    }

    void OnEnable()
    {
        playerMap.Enable();

        onPlayerSwitch.AddListener(switchPlayerPos);
        moveCamera.AddListener(moveCam);
        engageToggle.AddListener(toggleEngage);
        addToEnemyQ.AddListener(addToQ);
        newRoom.AddListener(moveToNewRoom);

        attackAction.performed += ctx => onAttack();
    }
    void OnDisable()
    {
        playerMap.Disable();

        onPlayerSwitch.RemoveListener(switchPlayerPos);
        moveCamera.RemoveListener(moveCam);
        engageToggle.RemoveListener(toggleEngage);
        addToEnemyQ.RemoveListener(addToQ);
        newRoom.RemoveListener(moveToNewRoom);

        attackAction.performed -= ctx => onAttack();
    }

    private void moveToNewRoom()
    {
        if (!isPlaying)
        {
            return;
        }
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

    private async Task moveCamTask(Vector3 v)
    {
        Vector3 start = playerCamera.transform.position;
        Vector3 end;

        if (v.magnitude == 0)
            end = camStart;
        else
            end = start + v;

        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            t = Mathf.Clamp01(t);

            playerCamera.transform.position = Vector3.Lerp(start, end, t);

            await Task.Yield();
        }

        playerCamera.transform.position = end;
    }

    private async void moveCam(Vector3 v)
    {
        await moveCamTask(v);
    }

    private async void toggleEngage()
    {
        if (engaging){
            return;
        }

        Vector3 v = new Vector3(0, 1, 0);
        Vector3 reset = new Vector3(0, 0, 0);

        engaging = true;

        if (!isEngaged)
        {
            isEngaged = true;

            menuManager.openEngage();
            await moveCamTask(v);
            enemyManager.showHitPoints();

            engaging = false;

            await Task.Delay(engageTime);

            if (isEngaged)
            {
                menuManager.closeEngage();
                toggleEngage();
            }
        }
        else
        {
            isEngaged = false;

            enemyManager.hideHitPoints();

            if (enemiesQ.Count > 0)
            {
                canAttack = true;
                Debug.Log(enemiesQ.Count);
                await targetManager.spawnHits(enemiesQ);
                enemiesQ.Clear();
                canAttack = false;
            }
            else
            {
                ScoreManager.onMiss.Invoke(20);
            }

            await moveCamTask(reset);

            engaging = false;
            
            menuManager.openOptions();
            enemyManager.Check();
        }
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

    public bool isEngagedf()
    {
        return isEngaged;
    }

    public bool isGaming()
    {
        return isPlaying;
    }

    private void onAttack()
    {
        if (canAttack)
        {
            targetManager.onClick();
        }
    }
}
