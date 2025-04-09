using UnityEngine;
using TMPro;

public class TimeTicking : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI timetext;
    public AudioClip EverySeconds;
    public AudioSource EverySecondsSource;

    [Header("Values")]
    public bool IsRunning = false;
    public float totalMilliseconds = 0;

    private float previousWholeSecond = -1f;

    private void Start()
    {
        EverySecondsSource.clip = EverySeconds;
    }

    public void StartTimer(int mins, int secs, int mills)
    {
        totalMilliseconds = (mins * 60 + secs) * 1000 + mills;
        IsRunning = true;
        previousWholeSecond = Mathf.Floor(totalMilliseconds / 1000f); // Set initial whole second
    }

    private void Update()
    {
        if (IsRunning && totalMilliseconds > 0)
        {
            totalMilliseconds -= Time.deltaTime * 1000f;

            if (totalMilliseconds <= 0)
            {
                totalMilliseconds = 0;
                IsRunning = false;
            }

            // Time breakdown
            int minutes = Mathf.FloorToInt(totalMilliseconds / 60000);
            int seconds = Mathf.FloorToInt((totalMilliseconds % 60000) / 1000);
            int milliseconds = Mathf.FloorToInt(totalMilliseconds % 1000);

            timetext.text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";

            // Check for second change
            float currentWholeSecond = Mathf.Floor(totalMilliseconds / 1000f);
            if (currentWholeSecond != previousWholeSecond)
            {
                EverySecondsSource.Play();
                previousWholeSecond = currentWholeSecond;
            }
        }
    }
}
