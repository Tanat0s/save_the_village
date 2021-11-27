using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheatScript : MonoBehaviour
{
    public ImageTimerScript wheatTimer;
    public CounterTextScript wheatCounter;
    public event Action<int> WheatUpdated;
    public int TotalWheatCount => totalWheatsCount;

    [SerializeField] private int wheatPerFarmer;
    [SerializeField] private int wheatPerKnight;
    [SerializeField] private int finalWheat;
    [SerializeField] private int farmerCost;
    [SerializeField] private int knightCost;
    [SerializeField] private CounterTextScript farmerCounter;
    [SerializeField] private CounterTextScript knightCounter;
    [SerializeField] private Text wheatsWinText;

    private int actualFarmerOnGetWheat = 1;
    private int totalWheatsCount;
    // Start is called before the first frame update
    void Start()
    {
        wheatCounter.Count = 1;
        wheatTimer.OnTick += GetWheat;
        UpdateWheatResult();
    }

    private void UpdateWheatResult()
    {
        wheatsWinText.text = $"Wheats: {wheatCounter.Count}/{finalWheat}";
    }

    private void GetWheat()
    {
        wheatCounter.Count += wheatPerFarmer * actualFarmerOnGetWheat;
        totalWheatsCount += wheatPerFarmer * actualFarmerOnGetWheat;
        wheatCounter.Count -= wheatPerKnight * knightCounter.Count;

        if (wheatCounter.Count < 0)
        {
            knightCounter.Count += wheatCounter.Count;
            wheatCounter.Count = Math.Abs(wheatCounter.Count);
        }
        actualFarmerOnGetWheat = farmerCounter.Count;
        WheatUpdated?.Invoke(wheatCounter.Count);
        UpdateWheatResult();
    }

    public void StartGetWheat()
    {
        wheatTimer.StartTimer();
    }

    public bool TryHireFarmer()
    {
        if (wheatCounter.Count < farmerCost)
        {
            return false;
        }

        wheatCounter.Count -= farmerCost;
        UpdateWheatResult();
        return true;
    }

    public bool TryHireKnight()
    {
        if (wheatCounter.Count < knightCost)
        {
            return false;
        }

        wheatCounter.Count -= knightCost;
        UpdateWheatResult();
        return true;
    }

    public bool IsEnoughWheats()
    {
        return wheatCounter.Count >= finalWheat;
    }
}
