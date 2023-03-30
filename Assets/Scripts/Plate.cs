using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : StateChangingObject, IInteractable
{

    [SerializeField] private Need mood;
    [SerializeField] private float moodChangeWhileDoing = -1.5f;

    [SerializeField] private float cookingTime = 5f;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Transform foodSpawnPosition;

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

    IEnumerator ShowNoMoodMessage()
    {
        messageToShow = "not enough mood";
        OnFocusEnter();
        yield return new WaitForSeconds(messageDelay);
        messageToShow = initialMessage;
        OnFocusExit();
    }

    override public void StopActivity()
    {
        base.StopActivity();
        StopCoroutine(Cooking());
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

    override public IEnumerator StartDoing()
    {
        StartCoroutine(Cooking());
        while (isDoing)
        {
            mood.UpdateState(moodChangeWhileDoing, 1);

            yield return new WaitForSeconds(1f / GameManager.instance.gameSpeed / GameManager.instance.timeSpeedMultiplier);
        }
    }

    private IEnumerator Cooking()
    {
        yield return new WaitForSeconds(cookingTime / GameManager.instance.gameSpeed / GameManager.instance.timeSpeedMultiplier);

        if(isDoing)
        {
            Instantiate(foodPrefab, foodSpawnPosition.position, Quaternion.identity);

            StopActivity();
        }
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
