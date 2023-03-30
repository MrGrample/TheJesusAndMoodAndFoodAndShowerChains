using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ritual : StateChangingObject, IInteractable
{
    [SerializeField] private Need mood;
    [SerializeField] private float moodChangeWhileDoing1 = -1.5f;
    [SerializeField] private float moodChangeWhileDoing3 = -1.5f;
    [SerializeField] private Money moneyNeed;

    [SerializeField] private float moodTrashold = 25f;

    private int stage = 1;

    [SerializeField] private float stage1NeededCondition = 100f;
    [SerializeField] private float stage1Speed = 0.15f;
    private float stage1CurrentCondition = 0f;

    private float stage1BarDivision;
    private int stage1CurrentBar = 0;

    [SerializeField] private float stage2NeededMoney = 10f;

    public override void Start()
    {
        base.Start();
        stage = 1;

        stage1BarDivision = stage1NeededCondition / 7f;
    }

    override public void Update()
    {
        if (isDoing)
        {
            if (Input.GetKeyDown(KeyCode.Q) || mood.currentState < moodTrashold)
            {
                StopActivity();
                UIManager.instance.HideRitualBar();
                if (mood.currentState < moodTrashold)
                {
                    StartCoroutine(ShowNoMoodMessage());
                }
            }
        }
    }

    override public IEnumerator StartDoing()
    {
        switch (stage)
        {
            case 1:
                UIManager.instance.ShowRitualBar();
                while (isDoing)
                {
                    mood.UpdateState(moodChangeWhileDoing1, 1);

                    stage1CurrentCondition += stage1Speed;
                    stage1CurrentBar = (int)(stage1CurrentCondition / stage1BarDivision);

                    if (stage1CurrentCondition >= stage1NeededCondition)
                    {
                        stage++;
                        messageToShow = "stage 2   give " + stage2NeededMoney + "$";
                        OnFocusEnter();
                        initialMessage = messageToShow;
                        StopActivity();
                        UIManager.instance.HideRitualBar();
                        Debug.Log(stage);
                    }

                    Debug.Log(stage1CurrentCondition);

                    UIManager.instance.UpdateRitual(stage1CurrentBar);

                    yield return new WaitForSeconds(1f / GameManager.instance.gameSpeed / GameManager.instance.timeSpeedMultiplier);
                }
                break;
            case 2:
                if (moneyNeed.currentState >= stage2NeededMoney)
                {
                    moneyNeed.UpdateState(-stage2NeededMoney, 1);
                    stage++;
                    messageToShow = "stage 3   give time and mood";
                    OnFocusEnter();
                    initialMessage = messageToShow;
                    stage1CurrentCondition = 0;
                    Debug.Log(stage);
                }
                else
                {
                    ShowNoMoneyMessage();
                }
                break;
            case 3:
                UIManager.instance.ShowRitualBar();
                while (isDoing)
                {
                    mood.UpdateState(moodChangeWhileDoing3, 1);

                    stage1CurrentCondition += stage1Speed;
                    stage1CurrentBar = (int)(stage1CurrentCondition / stage1BarDivision);

                    if (stage1CurrentCondition >= stage1NeededCondition)
                    {
                        StopActivity();
                        UIManager.instance.HideRitualBar();
                        UIManager.instance.Win();
                    }

                    UIManager.instance.UpdateRitual(stage1CurrentBar);

                    yield return new WaitForSeconds(1f / GameManager.instance.gameSpeed / GameManager.instance.timeSpeedMultiplier);
                }
                break;

        }
        
    }

    public override void OnInteraction()
    {
        if (mood.currentState >= moodTrashold)
        {
            if (stage == 1 || stage == 3)
            {
                base.OnInteraction();
            }
            else
            {
                StartCoroutine(StartDoing());
            }
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

    IEnumerator ShowNoMoneyMessage()
    {
        messageToShow = "not enough money";
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
