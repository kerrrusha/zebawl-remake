using Assets.Scripts.Entity;
using Assets.Scripts.Model;
using Assets.Scripts.Service;
using Assets.Scripts.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float deadlineHeight = 15;
    public Transform playerTransform;

    private GameSaveService gameSaveService;
    private LevelService levelService;
    private Level currentLevel;

    void Start()
    {
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
        SceneManager.LoadScene(currentLevel.name);
        playerTransform.position = currentLevel.initialPlayerPosition;
    }

    void Update()
    {
        
    }
}