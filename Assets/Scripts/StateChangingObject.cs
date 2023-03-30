using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChangingObject : MonoBehaviour, IInteractable
{
    protected bool isDoing = false;

    [SerializeField] protected SimpleFPSController player;
    [SerializeField] protected float timeMultiplierWhileDoing = 3f;

    [SerializeField] protected Transform whereToPutPlayerOnEnter;
    [SerializeField] protected Transform whereToPutPlayerOnExit;
    [SerializeField] protected GameObject whereToLook;

    [SerializeField] protected string messageToShow;
    protected string initialMessage;

    [SerializeField] protected float messageDelay = 1.5f;

    virtual public void Update()
    {
        if (isDoing)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StopActivity();
            }
        }
    }

    virtual public void Start()
    {
        initialMessage = messageToShow;
    }

    virtual public void StopActivity()
    {
        player.MoveToPoint(whereToPutPlayerOnExit);
        player.UnlockCamera(Quaternion.identity);
        GameManager.instance.timeSpeedMultiplier /= timeMultiplierWhileDoing;
        StopCoroutine(StartDoing());
        StartCoroutine(StopDoing());
        isDoing = false;
    }

    public void EndInteraction()
    {
    }

    public void EndLongInteraction()
    {
    }

    public void HoldInteraction()
    {
    }

    virtual public void OnFocusEnter()
    {
    }

    virtual public void OnFocusExit()
    {
    }

    public void OnInteraction(KeyCode keyCode)
    {
    }

    virtual public void OnInteraction()
    {
        if (!isDoing)
        {
            isDoing = true;
            player.LockCamera(whereToLook);
            player.MoveToPoint(whereToPutPlayerOnEnter);
            GameManager.instance.timeSpeedMultiplier *= timeMultiplierWhileDoing;
            StartCoroutine(StartDoing());
        }
    }

    virtual public IEnumerator StartDoing()
    {
        while (isDoing)
        {
            yield return new WaitForSeconds(1f / GameManager.instance.gameSpeed / GameManager.instance.timeSpeedMultiplier);
        }
    }

    protected IEnumerator StopDoing()
    {
        yield return new WaitForFixedUpdate();
        player.UnlockMovement();
    }
}
