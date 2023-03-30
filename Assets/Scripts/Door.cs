using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [SerializeField] private float openingSpeed = 1f;
    [SerializeField] private float openingAngle = 90f;
    private bool isOpening = false;

    [SerializeField] bool openingDirection = true;

    public bool isClosed = false;

    private void Start()
    {
        if (!openingDirection)
        {
            openingSpeed = -openingSpeed;
        }
    }

    public void OnInteraction()
    {
        if (!isClosed)
        {
            isOpening = !isOpening;
            StartCoroutine(rotateDoor());
        }
        else
        {
            UIManager.instance.PrintMessage("Closed");
        }
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

    public void OnFocusEnter()
    {
    }

    public void OnFocusExit()
    {
    }

    public void OnInteraction(KeyCode keyCode)
    {
    }

    IEnumerator rotateDoor()
    {
        Vector3 rotationVector;

        if (isOpening)
        {
            for (float i = 0; i < openingAngle / openingSpeed; i++)
            {
                rotationVector = transform.eulerAngles;
                rotationVector.y += openingSpeed;
                transform.rotation = Quaternion.Euler(rotationVector);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            for (float i = 0; i < openingAngle / openingSpeed; i++)
            {
                rotationVector = transform.eulerAngles;
                rotationVector.y -= openingSpeed;
                transform.rotation = Quaternion.Euler(rotationVector);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
