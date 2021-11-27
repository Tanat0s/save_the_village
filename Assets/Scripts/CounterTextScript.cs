using UnityEngine;
using UnityEngine.UI;

public class CounterTextScript : MonoBehaviour
{

    private int currentCount;
    private Text text;

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
            text = text ?? GetComponent<Text>();
            text.text = currentCount.ToString();
        }
    }
}
