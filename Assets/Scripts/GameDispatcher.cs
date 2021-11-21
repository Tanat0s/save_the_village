﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class GameDispatcher : MonoBehaviour
{
    //Strings const
    private const string failTitle = "You fail!\nYou created:\nWheats: {0}\nFarmers: {1}\nKnights: {2}";
    private const string winTitle = "You win!";
    private const string pauseTitle = "PAUSE";
    //Images
    [SerializeField] private Image wheatImage;
    [SerializeField] private Image farmerImage;
    [SerializeField] private Image knightImage;
    [SerializeField] private Image invasionImage;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Image resultImage;
    [SerializeField] private Sprite failSprite;
    [SerializeField] private Sprite winSprite;

    //Win conditions
    [SerializeField] private int finalWheat;
    [SerializeField] private int finalFarmer;
    //Counters
    [SerializeField] private Text currentWheatsText;
    [SerializeField] private Text currentFarmersText;
    [SerializeField] private Text currentKnightsText;
    [SerializeField] private Text currentInvasionText;
    [SerializeField] private Text resultText;
    [SerializeField] private Text wheatsWinText;
    [SerializeField] private Text FarmersWinText;
    [SerializeField] private Text TimeToStartInvasionText;
    [SerializeField] private int wheatPerFarmer;
    [SerializeField] private int wheatPerKnight;
    [SerializeField] private int[] waves = new int[5];
    private int currentInvasionWave = 0;
    private int totalWheatsCount;
    private int totalFarmersCount;
    private int totalKnightsCount;
    private int currentWheatsCount;
    private int CurrentWheatsCount
    { 
        get { return currentWheatsCount; } 
        set 
        {
            if(currentWheatsCount == value)
            {
                return;
            }
            currentWheatsCount = value;
            UpdateCounter(currentWheatsText, currentWheatsCount.ToString());
        } 
    }
    private int currentFarmerCount;
    private int CurrentFarmerCount
    {
        get { return currentFarmerCount; }
        set
        {
            if (currentFarmerCount == value)
            {
                return;
            }
            currentFarmerCount = value;
            UpdateCounter(currentFarmersText, currentFarmerCount.ToString());
        }
    }
    private int currentKnightCount;
    private int CurrentKnightCount
    {
        get { return currentKnightCount; }
        set
        {
            if (currentKnightCount == value)
            {
                return;
            }
            currentKnightCount = value;
            UpdateCounter(currentKnightsText, currentKnightCount.ToString());
        }
    }

    private bool isPause = false;
    public bool IsPause 
    { 
        get { return isPause; }
        set 
        {
            if(isPause == value)
            {
                return;
            }

            isPause = value;
            Time.timeScale = isPause ? 0 : 1;
            ShowHidePausePanel();
        } 
    }
    private int actualFarmerOnGetWheat = 1;
    private int farmersInQueue;
    private int knightsInQueue;
    private bool isInvasion = false;
    //Timers
    [SerializeField] private int timeToGetWheat;
    [SerializeField] private int timeToHireFarmer;
    [SerializeField] private int timeToHireKnight;
    [SerializeField] private int timeToStartInvasion;
    [SerializeField] private int timeInvasion;
    private float currentTimeToGetWheat;
    private float currentTimeToHireFarmer;
    private float currentTimeToHireKnight;
    private float currentTimeToStartInvasion;
    private float currentTimeInvasion;
    //Costs
    [SerializeField] private int farmerCost;
    [SerializeField] private int knightCost;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        audioSource = GetComponent<AudioSource>();
    }

    private void Init()
    {
        CurrentWheatsCount = 1;
        TimeToStartInvasionText.text = timeToStartInvasion.ToString();
        UpdateCounter(currentInvasionText, waves[currentInvasionWave].ToString());
        UpdateCounter(wheatsWinText, $"Wheats: {CurrentWheatsCount}/{finalWheat}");
        UpdateCounter(FarmersWinText, $"Farmers: {CurrentFarmerCount}/{finalFarmer}");
    }

    void UpdateCounter(Text counterText, string counter)
    {
        counterText.text = counter;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsPause)
        {
            return;
        }

        if (isInvasion)
        {
            CheckInvasion();
        }
        else
        {
            CheckTimeToStartInvaision();
        }

        GetWheat();
        CheckFarmersInQueue();
        CheckKnightsInQueue();
        CheckWinCondition();
    }

    private void CheckTimeToStartInvaision()
    {
        currentTimeToStartInvasion += Time.deltaTime;
        if(currentTimeToStartInvasion > timeToStartInvasion)
        {
            isInvasion = true;
        }

        TimeToStartInvasionText.text = (timeToStartInvasion - currentTimeToStartInvasion).ToString("f0");
    }

    private void GetWheat()
    {
        if (CurrentFarmerCount > 0)
        {
            currentTimeToGetWheat += Time.deltaTime;
            wheatImage.fillAmount = currentTimeToGetWheat / timeToGetWheat;

            if (currentTimeToGetWheat > timeToGetWheat)
            {
                CurrentWheatsCount += wheatPerFarmer * actualFarmerOnGetWheat;
                totalWheatsCount += wheatPerFarmer * actualFarmerOnGetWheat;
                CurrentWheatsCount -= wheatPerKnight * currentKnightCount;
                if (CurrentWheatsCount < 0)
                {
                    CurrentKnightCount += currentWheatsCount;
                    CurrentWheatsCount = Math.Abs(currentWheatsCount);
                }
                actualFarmerOnGetWheat = CurrentFarmerCount;
                UpdateCounter(wheatsWinText, $"Wheats: {CurrentWheatsCount}/{finalWheat}");
                currentTimeToGetWheat = 0;
            }
        }
    }

    private void CheckFarmersInQueue()
    {
        if (farmersInQueue > 0)
        {
            currentTimeToHireFarmer += Time.deltaTime;
            farmerImage.fillAmount = currentTimeToHireFarmer / timeToHireFarmer;

            if (currentTimeToHireFarmer > timeToHireFarmer)
            {
                farmersInQueue--;
                CurrentFarmerCount++;
                totalFarmersCount++;
                UpdateCounter(FarmersWinText, $"Farmers: {CurrentFarmerCount}/{finalFarmer}");
                currentTimeToHireFarmer = 0;
            }
        }
    }

    private void CheckKnightsInQueue()
    {
        if (knightsInQueue > 0)
        {
            currentTimeToHireKnight += Time.deltaTime;
            knightImage.fillAmount = currentTimeToHireKnight / timeToHireKnight;

            if (currentTimeToHireKnight > timeToHireKnight)
            {
                knightsInQueue--;
                CurrentKnightCount++;
                totalKnightsCount++;
                currentTimeToHireKnight = 0;
            }
        }
    }

    private void CheckInvasion()
    {
        currentTimeInvasion += Time.deltaTime;
        invasionImage.fillAmount = currentTimeInvasion / timeInvasion;

        if (currentTimeInvasion > timeInvasion)
        {
            CurrentKnightCount -= waves[currentInvasionWave];
            if (CurrentKnightCount < 0)
            {
                CurrentKnightCount = 0;
                ShowResultPanel(false);
                return;
            }

            if (currentInvasionWave + 1 == waves.Length)
            {
                ShowResultPanel(true);
                return;
            }

            currentTimeInvasion = 0;
            UpdateCounter(currentInvasionText, waves[++currentInvasionWave].ToString());
        }
    }

    private void CheckWinCondition()
    {
        if (CurrentWheatsCount >= finalWheat
            || currentFarmerCount == finalFarmer)
        {
            ShowResultPanel(true);
        }
    }

    public void HireFarmer()
    {
        if (farmersInQueue > 0)
        {
            return;
        }

        if (CurrentWheatsCount >= farmerCost)
        {
            farmersInQueue++;
            CurrentWheatsCount -= farmerCost;
            UpdateCounter(wheatsWinText, $"Wheats: {CurrentWheatsCount}/{finalWheat}");
        }
    }

    public void HireKnight()
    {
        if (knightsInQueue > 0)
        {
            return;
        }

        if (CurrentWheatsCount >= knightCost)
        {
            knightsInQueue++;
            CurrentWheatsCount -= knightCost;
            UpdateCounter(wheatsWinText, $"Wheats: {CurrentWheatsCount}/{finalWheat}");
        }
    }

    private void ShowHidePausePanel()
    {        
        if (isPause)
        {
            resultPanel.SetActive(true);
            resultImage.sprite = winSprite;
            resultText.text = pauseTitle;
            resultPanel.GetComponent<Image>().color = new Color(0.65f, 0.98f, 0.75f, 0.39f);
            
            if(audioSource.volume > 0f)
                audioSource.volume = 0.2f;
        }
        else
        {
            resultPanel.SetActive(false);
            
            if(audioSource.volume > 0f)
                audioSource.volume = 0.8f;
        }
    }

    public void DisableMusic()
    {
        audioSource.volume = 0.0f;
    }

    public void EnableMusic()
    {
        audioSource.volume = 0.8f;
    }

    private void ShowResultPanel(bool isWin)
    {
        IsPause = true;
        resultPanel.SetActive(true);

        if (isWin)
        {
            resultImage.sprite = winSprite;
            resultText.text = winTitle;
            resultPanel.GetComponent<Image>().color = new Color(0.65f, 0.98f, 0.75f, 0.39f);
        }
        else
        {
            resultImage.sprite = failSprite;
            resultText.text = string.Format(failTitle, totalWheatsCount, totalFarmersCount, totalKnightsCount);
            resultPanel.GetComponent<Image>().color = new Color(0.98f, 0.65f, 0.65f, 0.39f);
        }
    }
}
