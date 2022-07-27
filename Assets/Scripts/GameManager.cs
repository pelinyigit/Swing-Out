using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool isSDKComplete;
    public bool isThisLoaderScene;
    public int startLevelCountForLoop;
    [HideInInspector]
    public int levelCountOfSDK;

    private const string level = "level";
    private const string gem = "gem";

    public int currentLevel;
    public int gemCountInLevel;
    public int gemCountTotal;
    public int gemCountTotalTemp;

    public bool isLevelStarted;
    public bool isLevelCompleted;
    public bool isLevelFailed;

    private List<int> levelNumbers;
    private int[] randomLevels;
    private int totalLevelCount;
    public GameObject loaderPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (isSDKComplete)
        {
            levelCountOfSDK = 2;
        }
        else
        {
            levelCountOfSDK = 0;
        }

        RandomizeLevels();

        Assign();

        if (isThisLoaderScene)
        {
            loaderPanel.SetActive(true);
            LevelLoad();
        }
    }

    void Start()
    {
        isLevelStarted = false;
    }

    private void Assign()
    {
        if (!PlayerPrefs.HasKey(level))
        {
            PlayerPrefs.SetInt(level, 1);
        }

        if (!PlayerPrefs.HasKey(gem))
        {
            PlayerPrefs.SetInt(gem, 0);
        }

        currentLevel = PlayerPrefs.GetInt(level);
        gemCountTotal = PlayerPrefs.GetInt(gem);
    }

    private void RandomizeLevels()
    {
        totalLevelCount = SceneManager.sceneCountInBuildSettings - levelCountOfSDK;

        if (totalLevelCount < startLevelCountForLoop)
        {
            startLevelCountForLoop = totalLevelCount;
        }

        levelNumbers = new List<int>();

        for (int i = startLevelCountForLoop; i < totalLevelCount + 1; i++)
        {
            levelNumbers.Add(i);
        }

        randomLevels = new int[totalLevelCount - startLevelCountForLoop + 1];

        // Random.InitState(totalLevelCount);

        for (int i = 0; i < randomLevels.Length; i++)
        {
            int randomIndex = Random.Range(0, levelNumbers.Count);

            randomLevels[i] = levelNumbers[randomIndex];

            levelNumbers.RemoveAt(randomIndex);
        }
    }

    private void LevelLoad()
    {
        if (currentLevel > totalLevelCount)
        {
            currentLevel = randomLevels[(currentLevel - 1) % randomLevels.Length];
        }

        SceneManager.LoadSceneAsync(currentLevel + (levelCountOfSDK - 1));
    }

    public void LevelComplete()
    {
        if (!(isLevelCompleted || isLevelFailed))
        {
            UIManager.instance.LevelCompleteForUI(3f);
            currentLevel++;
            gemCountTotalTemp = gemCountTotal;
            gemCountTotal += gemCountInLevel;
            Save();
            isLevelCompleted = true;
        }
    }

    public void LevelFail()
    {
        if (!(isLevelFailed || isLevelCompleted))
        {
            UIManager.instance.LevelFailForUI(0.5f);
            isLevelFailed = true;
        }
    }

    public void CollectGem()
    {
        gemCountInLevel++;
        UIManager.instance.RefreshGemCountInLevel();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(level, currentLevel);
        PlayerPrefs.SetInt(gem, gemCountTotal);
    }

    public void RestartLevel()
    {
        LevelLoad();
    }

    public void NextLevel()
    {
        LevelLoad();
    }
}
