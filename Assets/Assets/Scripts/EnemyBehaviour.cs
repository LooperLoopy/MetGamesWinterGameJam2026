using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab; 
    [SerializeField] private ParticleBehaviour particlePrefab; 
    private GameObject Cross; 
    private GameObject environment; 
    private Canvas canvas; 
    private Camera mainCamera;
    private Vector3 position;
    private Vector2 screenPos;
    
    private GameObject button;
    private CanvasGroup canvasGroup;

    private EnemyManager enemyManager;

    // Vars
    private bool selected = false;
    private Key requiredKey;
    private float difficulty;

    private GameManager gameManager;

    public void Initialize(Transform weakpoint, Key requiredKey, int difficulty)
    {
        enemyManager = EnemyManager.Instance;
        gameManager = GameManager.Instance;

        this.requiredKey = requiredKey;

        canvas = gameManager.UISpace.transform.Find("Canvas").GetComponent<Canvas>();
        environment = gameManager.environment;
        mainCamera = gameManager.playerCamera.GetComponent<Camera>();

        position = weakpoint.position - new Vector3(0, 1, 0);
        screenPos = mainCamera.WorldToScreenPoint(position);

        button = Instantiate(buttonPrefab, canvas.transform);
        canvasGroup = button.GetComponent<CanvasGroup>();
        Cross = button.transform.Find("Cross").gameObject;

        hideButton();

        // Set button
        button.GetComponent<RectTransform>().position = screenPos;
        button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = requiredKey.ToString();

        this.difficulty = difficulty;
    }

    public float getDifficulty()
    {
        return difficulty;
    }

    public void showButton()
    {
        canvasGroup.alpha = 0.8f;
        Cross.SetActive(false);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void hideButton()
    {
        canvasGroup.alpha = 0f;
        Cross.SetActive(false);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void selectButton()
    {
        canvasGroup.alpha = 1f;
        Cross.SetActive(true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void onHit()
    {
        selected = false;

        // play a die animation
        ParticleBehaviour coins = Instantiate(particlePrefab, environment.transform);
        coins.transform.position = this.gameObject.transform.position;

        enemyManager.hitEnemy(this);
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
        Destroy(button);
    }

    public void deselect()
    {
        selected = false;
    }

    private void onSelect()
    {
        GameManager.addToEnemyQ.Invoke(this);
        if (selected)
        {
            showButton();
            selected = false;
            Debug.Log("unselected");
        }
        else
        {
            selectButton();
            selected = true;
            Debug.Log("selected");
        }
    }

    void Update()
    {
        if (gameManager.isEngagedf() && Keyboard.current[requiredKey].wasPressedThisFrame)
        {
            onSelect();
        }
    }
}
