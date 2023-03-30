using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : StateChangingObject, IInteractable
{
    private Animator fridgeAnimator;

    [SerializeField] private Money money;
    [SerializeField] private float foodCost = 3f;

    [SerializeField] private Transform foodSpawnPosition;
    [SerializeField] private GameObject foodPrefab;

    override public void Start()
    {
        base.Start();
        fridgeAnimator = GetComponent<Animator>();
    }

    public override void OnInteraction()
    {

        if (money.currentState >= 3f)
        {
            money.UpdateState(foodCost, -1);

            Instantiate(foodPrefab, foodSpawnPosition.position, Quaternion.identity);
        }
        else
        {
            StartCoroutine(ShowNoMoneyMessage());
        }
    }

    IEnumerator ShowNoMoneyMessage()
    {
        messageToShow = "not enough money";
        OnFocusEnter();
        yield return new WaitForSeconds(messageDelay);
        messageToShow = initialMessage;
        OnFocusExit();
    }

    override public void OnFocusEnter()
    {
        fridgeAnimator.SetBool("isLooking", true);
        UIManager.instance.ShowMessage(messageToShow);
    }

    override public void OnFocusExit()
    {
        fridgeAnimator.SetBool("isLooking", false);
        UIManager.instance.EraseMessage();
    }
}
