using System;
using System.Threading.Tasks;
using UnityEngine;

public class CrosshairBehaviour : MonoBehaviour
{
    private bool active = false;
    private float strength = 100f;
    private float a;
    private float t;
    private float timer = 0f;
    private float totalTime;
    private Func<float, float, float> function;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 direction;
    private Vector2 perpendicular;
    private RectTransform rectTransform;

    public Task Completion => completionSource.Task;
    private TaskCompletionSource<bool> completionSource;

    void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void Initialize(float a, float s, float time, Func<float, float, float> op, Vector2 start)
    {
        completionSource = new TaskCompletionSource<bool>();

        totalTime = time;
        function = op;
        this.a = a;
        strength = s;

        startPos = start;
        endPos = -start;

        direction = (endPos - startPos).normalized;

        perpendicular = new Vector2(-direction.y, direction.x);

        rectTransform.anchoredPosition = startPos;

        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }

        timer += Time.deltaTime;

        t = timer / totalTime;
        t = Mathf.Clamp01(t);

        Vector2 basePos = Vector2.Lerp(startPos, endPos, t);

        float offsetAmount = function(a, t) * strength;

        Vector2 finalPos = basePos + perpendicular * offsetAmount;

        rectTransform.anchoredPosition = finalPos;

        if (t >= 1)
        {
            active = false;
            ScoreManager.onMiss.Invoke(10);
            completionSource.TrySetResult(true);
        }
    }

    public void onHit(int combo)
    {
        active = false;
        if (rectTransform.anchoredPosition.magnitude <= 33)
        {
            ScoreManager.onHit.Invoke(combo);
        }
        else
        {
            ScoreManager.onMiss.Invoke(10);
           
        }
        completionSource.TrySetResult(true);
    }

    void OnDestroy()
    {
        Debug.Log("Crosshair destroyed");
    }
}
