using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int hours;
    private int minutes;
    private int days;

    [SerializeField] private GameObject sun;

    public int gameSpeed = 3;
    [SerializeField] private Need[] needs;

    public float timeSpeedMultiplier = 1f;

    public bool timeStopped = false;

    public static GameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        hours = 8;
        minutes = 0;
        days = 7;
        UIManager.instance.UpdateTime(hours, minutes);
        UIManager.instance.UpdateDay(days);
        StartTimer();
    }

    public void StartTimer()
    {
        timeStopped = false;
        StartCoroutine(countTime());
    }

    public void UpdateNeeds()
    {
        foreach (Need need in needs)
        {
            need.UpdateState(-1);
        }
    }

    IEnumerator countTime()
    {
        float sunRotation = 0f;
        while(!timeStopped)
        {
            yield return new WaitForSeconds(1f / (gameSpeed * timeSpeedMultiplier));

            sunRotation += 0.25f;
            sun.transform.rotation = Quaternion.Euler(sunRotation, -30, 0);

            RenderSettings.skybox.SetFloat("_Exposure", 1.4f);
            DynamicGI.UpdateEnvironment();

            minutes++;
            if (minutes >= 60)
            {
                minutes = minutes - 60;
                hours += 1;
            }
            if (hours == 24)
            {
                hours = 0;
                days--;
                if (days == 0)
                {
                    UIManager.instance.Lose();
                }
            }
            UIManager.instance.UpdateTime(hours, minutes);
            UIManager.instance.UpdateDay(days);

            UpdateNeeds();
        }
    }
}
