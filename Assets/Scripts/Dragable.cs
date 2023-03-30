using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class Dragable : MonoBehaviour, IDragable
{
    public Transform SnapPoint { get; set; }
    public bool IsTaken { get; set; }

    [SerializeField] private float grabTimeDelay = 1f;
    private float grabTime;
    private Rigidbody physics;

    private Hand hand;

    private Transform playerPosition;

    private void Awake()
    {
        IsTaken = false;
        hand = FindObjectOfType<Hand>();
        playerPosition = FindObjectOfType<SimpleFPSController>().transform;
    }


    public void OnBeginHold()
    {
        if (IsTaken == false)
        {
            grabTime += Time.deltaTime;
            if (grabTime >= grabTimeDelay)
            {
                grabTime = 0f;
                OnBeginTake();
            }
        }
    }


    public void OnBeginTake()
    {
        physics = GetComponent<Rigidbody>();
        physics.isKinematic = true;
        physics.useGravity = false;

      
        SnapPoint = hand.transform;
        OnTake();
    }

    public void OnTake()
    {
        StartCoroutine(AttachToHand());
    }


    public void OnRelease()
    {

        Debug.Log("Release!!!");
        if (physics != null)
        {
            physics.isKinematic = false;
            physics.useGravity = true;
        }

        transform.parent = null;
        hand.Release();
        IsTaken = false;
    }

    public void OnThrow()
    {
        if (IsTaken)
        {
            OnRelease();
            physics.AddForce((hand.transform.position - playerPosition.position)*200);
        }
    }

    private IEnumerator AttachToHand()
    {
        while (Vector3.Distance(transform.position, SnapPoint.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, SnapPoint.position, 15f * Time.deltaTime);
            yield return null;
        }

        transform.parent = SnapPoint;
        transform.localPosition = Vector3.zero;
        IsTaken = true;
        hand.Take(transform);

        yield return null;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(IsTaken) OnRelease();
    }


}
