using System;
using UnityEngine;
using UnityEngine.UI;

public class TextTimerScript : MonoBehaviour
{
    private float currentTime = 0f;
    public bool IsStart { get; private set; } = false;

    [SerializeField] private int TimePeriod;

    public event Action OnTick;
    private Text image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Text>();
    }

    public void StartTimer()
    {
        IsStart = true;
    }

    public void StopTimer()
    {
        IsStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStart)
            return;

        currentTime += Time.deltaTime;
        UpdateTimer();

        if (currentTime > TimePeriod)
        {
            currentTime = 0;

            if (OnTick != null)
                OnTick?.Invoke();
        }
    }

    public void UpdateTimer()
    {
        image.text = (TimePeriod - currentTime).ToString("f0");
    }
}
