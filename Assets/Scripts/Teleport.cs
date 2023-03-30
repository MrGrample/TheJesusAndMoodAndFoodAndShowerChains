using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IInteractable
{

    [SerializeField] private Transform tpPoint;
    [SerializeField] private SimpleFPSController player;

    public void EndInteraction()
    {
    }

    public void EndLongInteraction()
    {
    }

    public void HoldInteraction()
    {
    }

    public void OnFocusEnter()
    {
    }

    public void OnFocusExit()
    {
    }

    public void OnInteraction(KeyCode keyCode)
    {
    }

    public void OnInteraction()
    {
        player.MoveToPoint(tpPoint);
        player.UnlockMovement();
    }
}
