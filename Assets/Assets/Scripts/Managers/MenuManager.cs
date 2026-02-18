using System;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    private GameManager gameManager;

    // Menu Objects
    [Header("Menus")]
    [SerializeField] GameObject option_menu;
    [SerializeField] GameObject engage_menu;

    // Vars
    private GameObject[] menus;
   
    void Awake()
    {
        Instance = this;
        menus = new[] {option_menu};
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        openOptions();
    }

    public void openOptions()
    {
        Open(option_menu);
    }

    public void openEngage()
    {
        Open(engage_menu);
    }

    public void switchMenuSides()
    {
        for (int i = 0; i < menus.Length; i++)
        {
            BaseMenu baseMenu = menus[i].GetComponent<BaseMenu>();

            if (baseMenu == null)
            {
                continue;
            }

            if (gameManager.isPlayerRight() != baseMenu.isRight)
            {
                baseMenu.swapSides();
            }
        }
    }

    public void Open(GameObject menu)
    {
        BaseMenu baseMenu = menu.GetComponent<BaseMenu>();

        if (baseMenu == null)
        {
            return;
        }

        baseMenu.Open();
    }

    public void Close(GameObject menu)
    {
        BaseMenu baseMenu = menu.GetComponent<BaseMenu>();

        if (baseMenu == null)
        {
            return;
        }

        baseMenu.Close();
    }

    public bool menuIsRight(GameObject menu)
    {
        BaseMenu baseMenu = menu.GetComponent<BaseMenu>();

        if (baseMenu == null)
        {
            return false;
        }

        return baseMenu.isRight;
    }
}
