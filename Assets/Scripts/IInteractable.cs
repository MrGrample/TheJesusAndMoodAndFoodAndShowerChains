using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnInteraction(KeyCode keyCode);
    void OnInteraction();
    void HoldInteraction();
    void EndInteraction();
    void OnFocusEnter();
    void OnFocusExit();
    void EndLongInteraction();
}
