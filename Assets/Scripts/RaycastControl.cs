using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastControl : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rayLength = 40f;
    [SerializeField] private Image activeCrosshair;
    [SerializeField] private Image inActiveCrossHair;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float screenWidthAdjust = 1.5f;
    [SerializeField] private float screenHeighthAdjust = 4f;

    private Ray ray;
    private RaycastHit hit;
    private IInteractable current, previous;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        activeCrosshair.enabled = false;
        inActiveCrossHair.enabled = true;
    }

    private void Update()
    {

        ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / screenWidthAdjust, Screen.height / screenHeighthAdjust, 0f));
        bool isHited = Physics.Raycast(ray, out hit, rayLength, layerMask);

        if (isHited && hit.collider.TryGetComponent(out IInteractable device))
        {

            previous = current;
            current = device;

            if (previous != current) OnFocusEnter(ref current);

            if (Input.GetKeyDown(KeyCode.E)) current.OnInteraction();
            if (Input.GetKeyDown(KeyCode.Q)) current.OnInteraction(KeyCode.Q);
            if (Input.GetKeyDown(KeyCode.F)) current.OnInteraction(KeyCode.F);
            if (Input.GetKey(KeyCode.E) && !Input.GetKeyDown(KeyCode.Q) && !Input.GetKeyDown(KeyCode.F)) current.HoldInteraction();
            if (Input.GetKeyUp(KeyCode.E)) current.EndInteraction();

            if (previous != null && previous != current) OnFocusExit(ref previous);
        }

        // если луч не пересекает ничего (смотрим в небо)
        else
        {
            if (current != null) OnFocusExit(ref current);
            if (previous != null) OnFocusExit(ref previous);
        }
    }



    private void OnFocusEnter(ref IInteractable device)
    {
        device.OnFocusEnter();
        activeCrosshair.enabled = true;
        inActiveCrossHair.enabled = false;
    }

    private void OnFocusExit(ref IInteractable device)
    {
        device.OnFocusExit();
        device = null;
        activeCrosshair.enabled = false;
        inActiveCrossHair.enabled = true;
    }
}

