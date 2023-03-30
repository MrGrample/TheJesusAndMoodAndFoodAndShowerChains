using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private float minState = 0f;

    public float currentState;

    void Start()
    {
        currentState = minState;
        UIManager.instance.UpdateMoney(currentState);
    }

    public void UpdateState(float value, int multiplier)
    {
        currentState += value * multiplier;

        Debug.Log(currentState);

        UIManager.instance.UpdateMoney(currentState);
    }
}
