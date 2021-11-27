using System;
using UnityEngine;
using UnityEngine.UI;

public class GameDispatcher : MonoBehaviour
{
    //Strings const
    private const string failTitle = "You fail!\nYou created:\nWheats: {0}\nFarmers: {1}\nKnights: {2}";
    private const string winTitle = "You win!";
    private const string pauseTitle = "PAUSE";
    //Panels
    [SerializeField] private WheatScript wheatPanel;
    //Timers
    [SerializeField] private ImageTimerScript farmerTimer;
    [SerializeField] private ImageTimerScript knightTimer;
    [SerializeField] private ImageTimerScript invasionTimer;
    [SerializeField] private TextTimerScript prepareToInvasionTimer;
    //Images
    [SerializeField] private CounterTextScript farmerCounter;
    [SerializeField] private CounterTextScript knightCounter;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Image resultImage;
    [SerializeField] private Sprite failSprite;
    [SerializeField] private Sprite winSprite;

    //Win conditions
    [SerializeField] private int finalFarmer;
    //Counters
    [SerializeField] private Text currentInvasionText;
    [SerializeField] private Text resultText;
    [SerializeField] private Text FarmersWinText;
    [SerializeField] private int[] waves = new int[5];
    private int currentInvasionWave = 0;
    private int totalKnightsCount;

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

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        audioSource = GetComponent<AudioSource>();
    }

    private void Init()
    {
        farmerTimer.OnTick += GetFarmer;
        knightTimer.OnTick += GetKnight;
        invasionTimer.OnTick += CheckInvasion;
        prepareToInvasionTimer.OnTick += CheckTimeToStartInvaision;
        prepareToInvasionTimer.StartTimer();

        UpdateCounter(currentInvasionText, waves[currentInvasionWave].ToString());
        UpdateCounter(FarmersWinText, $"Farmers: {farmerCounter.Count}/{finalFarmer}");
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

        if(farmerCounter.Count == 1)
        {
            wheatPanel.StartGetWheat();
        }

        CheckWinCondition();
    }

    private void CheckTimeToStartInvaision()
    {
        invasionTimer.StartTimer();
        prepareToInvasionTimer.StopTimer();
    }

    private void GetFarmer()
    {
        farmerTimer.StopTimer();
        farmerCounter.Count++;
        UpdateCounter(FarmersWinText, $"Farmers: {farmerCounter.Count}/{finalFarmer}");
    }

    private void GetKnight()
    {
        knightTimer.StopTimer();
        knightCounter.Count++;
        totalKnightsCount++;
    }

    private void CheckInvasion()
    {
        knightCounter.Count -= waves[currentInvasionWave];
        if (knightCounter.Count < 0)
        {
            knightCounter.Count = 0;
            ShowResultPanel(false);
            return;
        }

        if (currentInvasionWave + 1 == waves.Length)
        {
            ShowResultPanel(true);
            return;
        }

        UpdateCounter(currentInvasionText, waves[++currentInvasionWave].ToString());
    }

    private void CheckWinCondition()
    {
        if (wheatPanel.IsEnoughWheats()
            || farmerCounter.Count == finalFarmer)
        {
            ShowResultPanel(true);
        }
    }

    public void HireFarmer()
    {
        if (farmerTimer.IsStart)
        {
            return;
        }

        if (wheatPanel.TryHireFarmer())
        {
            farmerTimer.StartTimer();
        }
    }

    public void HireKnight()
    {
        if (knightTimer.IsStart)
        {
            return;
        }

        if (wheatPanel.TryHireKnight())
        {
            knightTimer.StartTimer();
        }
    }

    private void ShowHidePausePanel()
    {        
        if (IsPause)
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
            resultText.text = string.Format(failTitle, wheatPanel.TotalWheatCount, farmerCounter.Count, totalKnightsCount);
            resultPanel.GetComponent<Image>().color = new Color(0.98f, 0.65f, 0.65f, 0.39f);
        }
    }
}
