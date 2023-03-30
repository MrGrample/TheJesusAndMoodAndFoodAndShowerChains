using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : StateChangingObject, IInteractable
{

    [SerializeField] private float sleepChangeWhileDoing = 1.5f;
    [SerializeField] private float moodChangeWhileDoing = 1.5f;
    [SerializeField] private Need sleepNeed;
    [SerializeField] private Need moodNeed;
    override public IEnumerator StartDoing()
    {
        while (isDoing)
        {
            sleepNeed.UpdateState(sleepChangeWhileDoing, 1);
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
