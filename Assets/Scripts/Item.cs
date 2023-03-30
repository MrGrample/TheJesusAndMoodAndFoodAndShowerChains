using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    public bool IsDragable;
    public Renderer[] Look;
    public bool IsEnable { get; private set; }

    [HideInInspector] public Dragable Dragable;

    private void Awake()
    {
        if (IsDragable)
            Dragable = gameObject.AddComponent<Dragable>();
    }

    public void EndInteraction()
    {
        if (IsDragable && Dragable != null)
            Dragable.OnRelease();
    }

    public void HoldInteraction()
    {
        if (IsDragable && Dragable != null)
        {
            Dragable.OnBeginHold();
            OnFocusExit();
        }
    }

    public void EndLongInteraction()
    {

    }

    virtual public void OnFocusEnter()
    {
    }

    virtual public void OnFocusExit()
    {
    }

    virtual public void OnInteraction()
    {
        IsEnable = !IsEnable;
    }

    virtual public void OnInteraction(KeyCode keyCode)
    {
        switch(keyCode)
        {
            case KeyCode.Q:
                if (IsDragable && Dragable != null)
                    Dragable.OnThrow();
                break;
        } 
    }
}
