using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private float dragRotationSpeed = 100f;
    private Transform holded;
    private SimpleFPSController controller;


    private void Start()
    {
        controller = GetComponentInParent<SimpleFPSController>();
    }

    public void Take(Transform item)
    {
        holded = item;
        StartCoroutine(RotateItem());
    }

    public void Release()
    {
        holded = null;
        StopAllCoroutines();
    }


    private IEnumerator RotateItem()
    {
        Vector3 startRotation = holded.eulerAngles;

        while (holded != null)
        {
            if (Input.GetMouseButton(1))
            {
                if (controller.enabled)
                    controller.enabled = false;

                float x = Input.GetAxis("Mouse X") * dragRotationSpeed * Time.deltaTime;
                float y = Input.GetAxis("Mouse Y") * dragRotationSpeed * Time.deltaTime;

                startRotation.x += y;
                startRotation.y += x;

                holded.rotation = Quaternion.Euler(startRotation);
            }

            else if (controller.enabled == false)
            {
                controller.enabled = true;
            }

            yield return null;
        }
    }





}

