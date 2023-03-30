using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Need : MonoBehaviour
{

    [SerializeField] private int needIndex;
    [SerializeField] private float maxState = 100f;
    [SerializeField] private float minState = 0f;
    [SerializeField] private float stateChangeSpeed = 0.3f;

    public float currentState;
    private float barDivision;
    private int currentBar = 7;

    void Start()
    {
        currentState = maxState;
        barDivision = maxState / 7f;
    }

    public void UpdateState(int multiplier)
    {
        currentState += stateChangeSpeed * multiplier;
        if (currentState > maxState)
        {
            currentState = maxState;
        }
        if (currentState < minState)
        {
            currentState = minState;
            UIManager.instance.Lose();
        }

        currentBar = (int)(currentState / barDivision);

        UIManager.instance.UpdateNeed(needIndex, currentBar);
    }

    public void UpdateState(float value, int multiplier)
    {
        currentState += value * multiplier;
        if (currentState > maxState)
        {
            currentState = maxState;
        }
        if (currentState < minState)
        {
            currentState = minState;
        }

        currentBar = (int)(currentState / barDivision);

        UIManager.instance.UpdateNeed(needIndex, currentBar);
    }
}
