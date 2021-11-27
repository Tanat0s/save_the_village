using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    private float currentTime = 0f;
    private bool isStart = false;
    [SerializeField] private int TimePeriod;
    
    public event Action OnTick;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void StartTimer()
    {
        isStart = true;
    }

    public void StopTimer()
    {
        isStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
            return;

        currentTime += Time.deltaTime;

        if(currentTime > TimePeriod)
        {
            currentTime = 0;

            if(OnTick != null)
                OnTick?.Invoke();
        }

        image.fillAmount = currentTime / TimePeriod;
    }
}
