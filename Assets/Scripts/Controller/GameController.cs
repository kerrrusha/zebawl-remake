using Assets.Scripts.Entity;
using Assets.Scripts.Model;
using Assets.Scripts.Service;
using Assets.Scripts.Util;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public float deadlineHeight = 7;
    public Transform playerTransform;

    private GameSaveService gameSaveService;
    private LevelService levelService;
    private Level currentLevel;

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

    void Start()
    {
        Instance = this;

        CommonUtil.CheckNotNull(playerTransform, nameof(playerTransform));

        levelService = new LevelService();

        gameSaveService = new GameSaveService();
        Debug.Log(gameSaveService.GetInfo());

        LoadCurrentLevel();
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
        if (!SceneManager.GetActiveScene().name.Equals(currentLevel.name))
        {
            SceneManager.LoadScene(currentLevel.name);
        }
        playerTransform.position = currentLevel.initialPlayerPosition;
    }

    void Update()
    {
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        if (currentLevel.initialPlayerPosition.y - playerTransform.position.y > deadlineHeight)
        {
            playerTransform.position = currentLevel.initialPlayerPosition;
        }
    }
}