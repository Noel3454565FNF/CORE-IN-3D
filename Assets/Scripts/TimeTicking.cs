using UnityEngine;
using TMPro;

public class TimeTicking : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI timetext;

    [Header("Values")]
    public bool IsRunning = false;
    public float totalMilliseconds = 0;

    private void Start()
    {
        //StartTimer(1, 10, 0);
    }

    public void StartTimer(int mins, int secs, int mills)
    {
        totalMilliseconds = (mins * 60 + secs) * 1000 + mills;
        IsRunning = true;
    }

    private void Update()
    {
        if (IsRunning && totalMilliseconds > 0)
        {
            totalMilliseconds -= Time.deltaTime * 1000; // Subtract real-time elapsed milliseconds

            if (totalMilliseconds <= 0)
            {
                totalMilliseconds = 0;
                IsRunning = false;
            }

            int minutes = Mathf.FloorToInt(totalMilliseconds / 60000);
            int seconds = Mathf.FloorToInt((totalMilliseconds % 60000) / 1000);
            int milliseconds = Mathf.FloorToInt(totalMilliseconds % 1000);

            timetext.text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
        }
    }
}
