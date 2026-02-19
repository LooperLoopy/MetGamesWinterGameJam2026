using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : BaseMenu
{
    // Buttons
    [Header("Buttons")]
    [SerializeField] private Button StartButton;
    [SerializeField] private Button EndButton;

    GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();

        StartButton.onClick.AddListener(onStart);
        EndButton.onClick.AddListener(onExit);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public override void Open()
    {
        opened = true;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        handleOpen();
    }

    private void onStart()
    {
        Close();
        gameManager.startGame();
    }

    private void onExit()
    {
        Close();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
