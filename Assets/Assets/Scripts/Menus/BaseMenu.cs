using UnityEngine;

public class BaseMenu : MonoBehaviour
{
    [Header("Canvas Items")]
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected CanvasGroup canvasGroup;

    protected RectTransform rectTranform;

    protected bool opened = false;
    public bool isRight = false;

    protected virtual void Awake()
    {
        rectTranform = gameObject.GetComponent<RectTransform>();
        rectTranform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTranform.anchorMin = new Vector2(0.5f, 0.5f);

        if (canvas == null)
            canvas = GetComponent<Canvas>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        opened = false;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void Open()
    {
        if (!GameManager.Instance.isGaming())
        {
            return;
        }

        opened = true;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        handleOpen();
    }

    public virtual void Close()
    {
        opened = false;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        handleClose();
    }

    public virtual void swapSides()
    {
        Vector2 v = new Vector2(-rectTranform.anchoredPosition.x, rectTranform.anchoredPosition.y);
        moveTo(v);
        isRight = !isRight;
    }

    protected virtual void moveTo(Vector2 movePoint)
    {
        rectTranform.anchoredPosition = movePoint;
    }

    protected virtual void handleOpen(){}
    protected virtual void handleClose(){}

}