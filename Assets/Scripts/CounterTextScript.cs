using UnityEngine;
using UnityEngine.UI;

public class CounterTextScript : MonoBehaviour
{

    private int currentCount;
    public int Count
    {
        get { return currentCount; }
        set
        {
            if (currentCount == value)
            {
                return;
            }
            currentCount = value;
            text.text = currentCount.ToString();
        }
    }

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
