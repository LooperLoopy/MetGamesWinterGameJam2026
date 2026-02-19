using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionsMenu : BaseMenu
{
    // Buttons
    [Header("Buttons")]
    [SerializeField] private Button EngageButton;
    [SerializeField] private Button PeekButton;
    [SerializeField] private Button MoveButton;
    [SerializeField] private Button button;

    // Position
    private Vector2 mousePosition;
    private Vector2 opPosition;

    // vars
    private float maxDistance = 400f;
    float maxTilt = 15f;

    void face_to_cursor()
    {
        mousePosition = Mouse.current.position.ReadValue();
        opPosition = (Vector2)rectTranform.position;

        Vector2 direction = mousePosition - opPosition;
        float xdistance = mousePosition.x - opPosition.x;
        float ydistance = mousePosition.y - opPosition.y;
        float xratio = Mathf.Clamp01(Mathf.Abs(xdistance) / maxDistance);
        float yratio = Mathf.Clamp01(Mathf.Abs(ydistance) / maxDistance);

        float zangle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (zangle > 90f) zangle -= 180f;
        if (zangle < -90f) zangle += 180f;
        zangle *= 0.1f * xratio;

        float xTilt = -ydistance / maxDistance * maxTilt;
        float yTilt =  xdistance / maxDistance * maxTilt;

        xTilt = Mathf.Clamp(xTilt, -maxTilt, maxTilt);
        yTilt = Mathf.Clamp(yTilt, -maxTilt, maxTilt);

        rectTranform.rotation = Quaternion.Euler(xTilt, yTilt, zangle);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        rectTranform.anchoredPosition = new Vector2(-75, -100);

        EngageButton.onClick.AddListener(onEngage);
        PeekButton.onClick.AddListener(onPeek);
        MoveButton.onClick.AddListener(onMove);
    }

    protected override void handleOpen()
    {
        EventSystem.current.SetSelectedGameObject(EngageButton.gameObject);
    }

    private void onEngage()
    {
        this.Close();
        
        GameManager.engageToggle.Invoke();
    }

    async private void onPeek()
    {
        ScoreManager.onMiss.Invoke(10);

        this.Close();
        Vector3 v = new Vector3(0, 1, 0);
        Vector3 reset = new Vector3(0, 0, 0);

        GameManager.moveCamera.Invoke(v);
        
        await Task.Delay(200);
        EnemyManager.Instance.showHitPoints();

        await Task.Delay(200);

        this.Open();
        GameManager.moveCamera.Invoke(reset);
        EnemyManager.Instance.hideHitPoints();
    }

    private void onMove()
    {
        GameManager.onPlayerSwitch.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (opened)
        {
            face_to_cursor();
        }
    }
}