using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : StateChangingObject, IInteractable
{
    [SerializeField] private float showerChangeWhileDoing = -1.5f;
    [SerializeField] private Need showerNeed;
    [SerializeField] private float moodChangeWhileDoing = 1.5f;
    [SerializeField] private Need moodNeed;

    [SerializeField] private float showerTrashold = 25f;

    override public void Update()
    {
        if (isDoing)
        {
            if (Input.GetKeyDown(KeyCode.Q) || showerNeed.currentState < showerTrashold)
            {
                StopActivity();
                if (showerNeed.currentState < showerTrashold)
                {
                    StartCoroutine(ShowNoShowerMessage());
                }
            }
        }
    }

    IEnumerator ShowNoShowerMessage()
    {
        messageToShow = "you stink";
        OnFocusEnter();
        yield return new WaitForSeconds(messageDelay);
        messageToShow = initialMessage;
        OnFocusExit();
    }

    public override void OnInteraction()
    {
        if (showerNeed.currentState >= showerTrashold)
        {
            base.OnInteraction();
        }
        else
        {
            StartCoroutine(ShowNoShowerMessage());
        }
    }

    override public IEnumerator StartDoing()
    {
        while (isDoing)
        {
            showerNeed.UpdateState(showerChangeWhileDoing, 1);
            moodNeed.UpdateState(moodChangeWhileDoing, 1);

            yield return new WaitForSeconds(1f / GameManager.instance.gameSpeed / GameManager.instance.timeSpeedMultiplier);
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
