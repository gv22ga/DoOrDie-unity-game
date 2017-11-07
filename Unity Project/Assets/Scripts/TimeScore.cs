using UnityEngine;
using System.Collections;
using System.Timers;

public class TimeScore : MonoBehaviour
{
    public static int timeElapsed = 0;
    public static int seconds = 0;
    public static int minutes = 0;
    private string secondsString;
    private string minutesString;
    private GameObject timeScore;
    public string timeString = "00:00";

    void Awake()
    {
        timeScore = GameObject.Find("TimeScore");
        Timer myTimer = new Timer();
        myTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
        myTimer.Interval = 1000;
        myTimer.Start();
    }


    void Update()
    {
        if (seconds/10 == 0)
        {
            secondsString = "0" + seconds.ToString();
        }
        else
        {
            secondsString = seconds.ToString();
        }
        if (minutes/10 == 0)
        {
            minutesString = "0" + minutes.ToString();
        }
        else
        {
            minutesString = minutes.ToString();
        }
        timeScore.GetComponent<GUIText>().text = minutesString+":"+secondsString;
    }

    public static void DisplayTimeEvent(object source, ElapsedEventArgs e)
    {
        timeElapsed++;
        seconds++;
        if (seconds == 60)
        {
            seconds = 0;
            minutes++;
        }
    }

}
