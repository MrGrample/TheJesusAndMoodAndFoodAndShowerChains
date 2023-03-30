using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : StateChangingObject, IInteractable
{
    [SerializeField] private Money moneyNeed;
    [SerializeField] private Need mood;
    [SerializeField] private float moodChangeWhileDoing = -1.5f;

    [SerializeField] private float moodTrashold = 25f;

    override public void Update()
    {
        if (isDoing)
        {
            if (Input.GetKeyDown(KeyCode.Q) || mood.currentState < moodTrashold)
            {
                StopActivity();
                if (mood.currentState < moodTrashold)
                {
                    StartCoroutine(ShowNoMoodMessage());
                }
            }
        }
    }

    override public IEnumerator StartDoing()
    {
        float randomValue;
        while (isDoing)
        {
            randomValue = Random.Range(0f, 100f);
            mood.UpdateState(moodChangeWhileDoing, 1);

            if (randomValue > 90)
            {
                moneyNeed.UpdateState(Random.Range(0f, 1f), 1);
            }

            yield return new WaitForSeconds(1f / GameManager.instance.gameSpeed / GameManager.instance.timeSpeedMultiplier);
        }
    }

    public override void OnInteraction()
    {
        if (mood.currentState >= moodTrashold)
        {
            base.OnInteraction();
        }
        else
        {
            StartCoroutine(ShowNoMoodMessage());
        }
    }

    IEnumerator ShowNoMoodMessage()
    {
        messageToShow = "not enough mood";
        OnFocusEnter();
        yield return new WaitForSeconds(messageDelay);
        messageToShow = initialMessage;
        OnFocusExit();
    }

    public override void OnFocusEnter()
    {
        UIManager.instance.ShowMessage(messageToShow);
    }

    public override void OnFocusExit()
    {
        UIManager.instance.EraseMessage();
    }
}
