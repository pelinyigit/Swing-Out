using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Color mainColor;
    public Color gemColor;

    public GameObject[] levelCounts;
    public GameObject[] gemCountInLevelTexts;
    public GameObject[] gemCountTotalTexts;
    public GameObject[] gemIcons;
    public GameObject gameName;
    public GameObject tutorialHand;
    public GameObject tutorialText;
    public GameObject[] panels;
    public GameObject restartButton;
    public GameObject joystick;

    private bool isLevelStartedForUI;
    private bool isLevelCompletedForUI;
    private bool isLevelFailedForUI;

    private bool letCountGems;
    private float timerCountGems;
    private float speedCountGems;
    private int startGemCountInLevel;
    private int endGemCountInLevel;
    private int startGemCountTotal;
    private int endGemCountTotal;

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
    }

    void Start()
    {
        PrepareUI();
    }

    private void PrepareUI()
    {
        for (int i = 0; i < levelCounts.Length; i++)
        {
            levelCounts[i].GetComponent<TextMeshProUGUI>().color = mainColor;
            levelCounts[i].GetComponent<TextMeshProUGUI>().text = "LEVEL " + GameManager.instance.currentLevel.ToString();
        }

        for (int i = 0; i < gemCountInLevelTexts.Length; i++)
        {
            gemCountInLevelTexts[i].GetComponent<TextMeshProUGUI>().color = mainColor;
            gemCountInLevelTexts[i].GetComponent<TextMeshProUGUI>().text = "0";
        }

        for (int i = 0; i < gemCountTotalTexts.Length; i++)
        {
            gemCountTotalTexts[i].GetComponent<TextMeshProUGUI>().color = mainColor;
            gemCountTotalTexts[i].GetComponent<TextMeshProUGUI>().text = GameManager.instance.gemCountTotal.ToString();
        }

        for (int i = 0; i < gemIcons.Length; i++)
        {
            gemIcons[i].GetComponent<Image>().color = gemColor;
        }

        gameName.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = mainColor;
        tutorialText.GetComponent<TextMeshProUGUI>().color = mainColor;
        restartButton.GetComponent<Image>().color = mainColor;

        gemIcons[0].SetActive(false);
        restartButton.SetActive(false);

        transform.up = Vector3.forward * -1f;
    }

    void Update()
    {
        LevelStartForUI();

        CountingGems();
    }

    public void LevelStartForUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isLevelStartedForUI)
            {
                gameName.SetActive(false);
                tutorialHand.SetActive(false);
                tutorialText.SetActive(false);
                gemIcons[0].SetActive(true);
                restartButton.SetActive(true);

                GameManager.instance.isLevelStarted = true;
                GameManager.instance.isLevelCompleted = false;
                GameManager.instance.isLevelFailed = false;
                joystick.SetActive(true);
                isLevelStartedForUI = true;
            }
        }
    }

    public void LevelCompleteForUI(float delay)
    {
        if (!isLevelCompletedForUI)
        {
            StartCoroutine(WaitForLevelComplete(delay));

            isLevelCompletedForUI = true;
        }
    }

    IEnumerator WaitForLevelComplete(float delay)
    {
        yield return new WaitForSeconds(delay);

        panels[0].SetActive(false);
        panels[1].SetActive(true);

        yield return new WaitForSeconds(1f);

        CountGems();
    }

    public void LevelFailForUI(float delay)
    {
        if (!isLevelFailedForUI)
        {
            StartCoroutine(WaitForLevelFail(delay));

            isLevelFailedForUI = true;
        }
    }

    IEnumerator WaitForLevelFail(float delay)
    {
        yield return new WaitForSeconds(delay);

        panels[0].SetActive(false);
        panels[2].SetActive(true);

        yield return new WaitForSeconds(1.25f);

        GameManager.instance.RestartLevel();
    }

    public void RefreshGemCountInLevel()
    {
        for (int i = 0; i < gemCountInLevelTexts.Length; i++)
        {
            gemCountInLevelTexts[i].GetComponent<TextMeshProUGUI>().text = GameManager.instance.gemCountInLevel.ToString();

            if (gemCountInLevelTexts[i].activeInHierarchy)
            {
                gemCountInLevelTexts[i].GetComponent<Animator>().SetTrigger("PopUp");
            }
        }
    }

    private void RefreshGemCountTotal()
    {
        for (int i = 0; i < gemCountTotalTexts.Length; i++)
        {
            gemCountTotalTexts[i].GetComponent<TextMeshProUGUI>().text = GameManager.instance.gemCountTotalTemp.ToString();
            
            if (gemCountTotalTexts[i].activeSelf)
            {
                gemCountTotalTexts[i].GetComponent<Animator>().SetTrigger("PopUp");
            }
        }
    }

    public void CountGems()
    {
        speedCountGems = 2f;
        letCountGems = true;
        startGemCountInLevel = GameManager.instance.gemCountInLevel;
        endGemCountInLevel = 0;
        startGemCountTotal = GameManager.instance.gemCountTotalTemp;
        endGemCountTotal = GameManager.instance.gemCountTotal;
    }

    private void CountingGems()
    {
        if (letCountGems)
        {
            timerCountGems += Time.deltaTime * speedCountGems;

            GameManager.instance.gemCountInLevel = (int) Mathf.Lerp(startGemCountInLevel, endGemCountInLevel, timerCountGems);
            GameManager.instance.gemCountTotalTemp = (int) Mathf.Lerp(startGemCountTotal, endGemCountTotal, timerCountGems);

            RefreshGemCountInLevel();
            RefreshGemCountTotal();

            if (timerCountGems >= 1f)
            {
                timerCountGems = 0f;
                letCountGems = false;
            }
        }
    }

    public void NextLevelButton()
    {
        GameManager.instance.NextLevel();
    }

    public void RestartButton()
    {
        GameManager.instance.RestartLevel();
    }
}