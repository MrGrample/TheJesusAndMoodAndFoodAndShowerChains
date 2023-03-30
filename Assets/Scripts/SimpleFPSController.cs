using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (CharacterController))]
public class SimpleFPSController : MonoBehaviour
{


    [SerializeField] [Range(0.2f, 10f)] private float walkSpeed = 1f;
    [SerializeField] [Range(1f, 20f)] private float runSpeed = 10f;

    [SerializeField] [Range(1f, 100f)] private float mouseSensetivityX;
    [SerializeField] [Range(1f, 100f)] private float mouseSensetivityY;

    [SerializeField] private float headRotationMinAngle;
    [SerializeField] private float headRotationMaxAngle;

    [SerializeField] private Transform head;
    [SerializeField] private Transform feet;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float gravityScale = 9.8f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private LayerMask groundSurface;

    private const string SIDE = "Horizontal";
    private const string FORWARD = "Vertical";
    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";
    private const string JUMP = "Jump";


    private CharacterController controller;
    private Vector3 moveDirection;
    private Vector3 gravitation;
    private float headRotation;

    private GameObject lookTarget;

    public bool cameraLock = false;
    private Vector3 bodyHeadDifference;

    private bool isMovementLocked = false;
    private bool isRespawning = false;
    [SerializeField] private GameObject respawnPoint;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        bodyHeadDifference = head.position - this.transform.position;
    }

    public void Respawn()
    {
        StartCoroutine(Respawning());
    }

    public void MoveToPoint(Transform point)
    {
        isMovementLocked = true;
        transform.position = point.position;
    }

    public void UnlockMovement()
    {
        isMovementLocked = false;
        isRespawning = false;
    }

    private void Update()
    {
        if (!cameraLock)
        {
            MouseLook();
        }
        else
        {
            this.transform.LookAt(lookTarget.transform.position - bodyHeadDifference);
        }
        if (!isMovementLocked)
        {
            isGrounded = Physics.CheckSphere(feet.position, 0.5f, groundSurface);
            if (isGrounded && gravitation.y < 0) gravitation.y = -1f;


            Jump();
            Move();
        }
        else
        {
            if(isRespawning)
            {
                transform.position = respawnPoint.transform.position;
            }
        }
          
    }


    private void Jump()
    {
        if (isGrounded)
        {
            if (Input.GetButtonDown(JUMP))
                gravitation.y = Mathf.Sqrt(jumpHeight * gravityScale);
        }
        else
        {
            gravitation.y -= gravityScale * Time.deltaTime;
           
        }

        controller.Move(gravitation * Time.deltaTime);
    }


    private void MouseLook()
    {
        float bodyRotation = Input.GetAxis(MOUSE_X);
        float rotationAxe = Input.GetAxis(MOUSE_Y);

        transform.Rotate(Vector3.up * bodyRotation * mouseSensetivityX * Time.deltaTime);
        headRotation += rotationAxe * mouseSensetivityY * Time.deltaTime;

        headRotation = Mathf.Clamp(headRotation, headRotationMinAngle, headRotationMaxAngle);
        head.localRotation = Quaternion.Euler(Vector3.left * headRotation);
    }

    public void LockCamera(GameObject target)
    {
        cameraLock = true;
        lookTarget = target;
    }

    public void UnlockCamera(Quaternion whereToLook)
    {
        transform.rotation = whereToLook;

        cameraLock = false;
        lookTarget = null;
    }

    private void Move()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float side = Input.GetAxis(SIDE);
        float forward = Input.GetAxis(FORWARD);

        moveDirection = transform.right * side + transform.forward * forward;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    IEnumerator Respawning()
    {
        isRespawning = true;
        yield return new WaitForSeconds(0.5f);
        UnlockMovement();
    }
}
