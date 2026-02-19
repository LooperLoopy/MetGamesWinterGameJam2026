using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EngageMenu : BaseMenu
{
    // Buttons
    [Header("Buttons")]
    [SerializeField] private Button ComfirmButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();

        ComfirmButton.onClick.AddListener(onConfirm);
    }

    protected override void handleOpen()
    {
        EventSystem.current.SetSelectedGameObject(ComfirmButton.gameObject);
    }

    private void onConfirm()
    {
        Close();
        GameManager.engageToggle.Invoke();
    }

    public override void swapSides(){}
}