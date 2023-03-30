using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : StateChangingObject, IInteractable
{
    [SerializeField] private float showerChangeWhileDoing = 1.5f;
    [SerializeField] private Need showerNeed;
    override public IEnumerator StartDoing()
    {
        while (isDoing)
        {
            showerNeed.UpdateState(showerChangeWhileDoing, 1);

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
