using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{

    private bool isConsumed = false;

    private Need foodNeed;
    [SerializeField] private float nutrition = 45f;

    [SerializeField] private string messageToShow = "consume it!";

    private void Start()
    {
        foodNeed = GameObject.FindGameObjectWithTag("FoodNeed").GetComponent<Need>();
    }

    override public void OnInteraction(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Q:
                if (IsDragable && Dragable != null)
                    Dragable.OnThrow();
                break;
            case KeyCode.F:
                ConsumeFood();
                break;
        }
    }

    private void ConsumeFood()
    {
        if (!isConsumed)
        {
            isConsumed = true;
            Debug.Log("consume");
            foodNeed.UpdateState(nutrition, 1);
        }
        else
        {
            UIManager.instance.PrintMessage("it is already consumed");
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
