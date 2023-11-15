using Assets.Scripts.Entity;
using Assets.Scripts.Model;
using Assets.Scripts.Service;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public float deadlineHeight = 7;

    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private Level currentLevel;

    //services
    private GameSaveService gameSaveService;
    private LevelService levelService;

    //UI
    private GameObject ingameCanvas;
    private GameObject menuCanvas;
    private GameObject storeCanvas;
    private List<GameObject> canvasList;

    //while script loading
    void Awake()
    {
        Instance = this;

        playerTransform = GameObject.FindWithTag("Player").transform;
        playerRigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();

        levelService = new LevelService();
        gameSaveService = new GameSaveService();

        ingameCanvas = GameObject.Find("ingame-canvas");
        menuCanvas = GameObject.Find("menu-canvas");
        storeCanvas = GameObject.Find("store-canvas");
        canvasList = new List<GameObject> { ingameCanvas, menuCanvas, storeCanvas };

        CheckInitialization();
    }

    //with first frame
    private void Start()
    {
        Debug.Log(gameSaveService.GetInfo());
        LoadCurrentLevel();
        ShowUIElement(ingameCanvas);
    }

    private void CheckInitialization()
    {
        CommonUtil.CheckNotNull(playerTransform, nameof(playerTransform));
        CommonUtil.CheckNotNull(playerRigidbody, nameof(playerRigidbody));

        CommonUtil.CheckNotNull(gameSaveService, nameof(gameSaveService));
        CommonUtil.CheckNotNull(levelService, nameof(levelService));

        CommonUtil.CheckNotNull(ingameCanvas, nameof(ingameCanvas));
        CommonUtil.CheckNotNull(menuCanvas, nameof(menuCanvas));
        CommonUtil.CheckNotNull(storeCanvas, nameof(storeCanvas));
    }

    public void FinishLevel()
    {
        try
        {
            currentLevel = levelService.FindNext(currentLevel);
            gameSaveService.Add(currentLevel.name);
        } 
        catch (IndexOutOfRangeException e)
        {
            Debug.Log(e.Message);
            return;
        }
        LoadCurrentScene();
    }

    public void OpenMenu()
    {
        FreezeGame();
        ShowUIElement(menuCanvas);
    }

    public void CloseMenu()
    {
        UnfreezeGame();
        ShowUIElement(ingameCanvas);
    }

    private void ShowUIElement(GameObject menuCanvas)
    {
        HideAllUIElements();
        menuCanvas.SetActive(true);
    }

    private void HideAllUIElements()
    {
        foreach (GameObject canvas in canvasList)
        {
            canvas.SetActive(false);
        }
    }

    private void FreezeGame()
    {
        Time.timeScale = 0f;
    }

    private void UnfreezeGame()
    {
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Debug.Log($"Restarting game...");
        Level firstLevel = levelService.GetAll().ToArray()[0];
        gameSaveService.Add(firstLevel);

        LoadCurrentLevel();
        CloseMenu();
    }

    private void LoadCurrentLevel()
    {
        GameSave currentSave = gameSaveService.GetLatestSave();
        Debug.Log($"Loaded latest save: {currentSave}");

        currentLevel = levelService.FindByName(currentSave.LevelName);
        Debug.Log($"Found current level: {currentLevel}");

        LoadCurrentScene();
    }

    private void LoadCurrentScene()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerTransform.position = currentLevel.initialPlayerPosition;

        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;

        if (SceneManager.GetActiveScene().name.Equals(currentLevel.name))
        {
            Debug.LogWarning($"Attempt to load current scene while it's already loaded: {currentLevel.name}");
            return;
        }

        Debug.Log($"Loading scene: {currentLevel.name}");
        SceneManager.LoadScene(currentLevel.name);
    }

    void Update()
    {
        CheckInput();
        CheckGameOver();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuCanvas.activeSelf)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    private void CheckGameOver()
    {
        if (currentLevel.initialPlayerPosition.y - playerTransform.position.y > deadlineHeight)
        {
            playerTransform.position = currentLevel.initialPlayerPosition;
        }
    }
}