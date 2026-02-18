using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab; 
    private Canvas canvas; 
    private Camera mainCamera;
    private Vector3 position;
    private Vector2 screenPos;
    
    private GameObject button;
    private CanvasGroup canvasGroup;

    private EnemyManager enemyManager;

    // Vars
    private bool selected = false;
    private float difficulty;

    public void Initialize(Transform weakpoint)
    {
        enemyManager = EnemyManager.Instance;

        canvas = GameManager.Instance.UISpace.transform.Find("Canvas").GetComponent<Canvas>();
        mainCamera = GameManager.Instance.playerCamera.GetComponent<Camera>();

        position = weakpoint.position - new Vector3(0, 1, 0);
        screenPos = mainCamera.WorldToScreenPoint(position);

        button = Instantiate(buttonPrefab, canvas.transform);
        canvasGroup = button.GetComponent<CanvasGroup>();

        hideButton();

        // Set position
        button.GetComponent<RectTransform>().position = screenPos;

        button.GetComponent<Button>().onClick.AddListener(onSelect);

        difficulty = 3;
    }

    public float getDifficulty()
    {
        return difficulty;
    }

    public void showButton()
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void hideButton()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void selectButton()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void onHit()
    {
        selected = false;

        // play a die animation

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
}
