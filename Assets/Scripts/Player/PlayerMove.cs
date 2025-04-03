using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TurnDirection { Left, Right, Both }

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10f;
    public int wayIndex = 1;
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public Lane way;

    private Vector3 targetPosition;
    private float verticalVelocity;
    private bool isJumping;
    private bool canMove = true;
    private Vector2 swipeStart;
    private Vector2 currentTouchPosition;
    [ReadOnly]
    public GameObject turnPivot;

    public Action<Vector3, float> onRotate;

    private bool canTurn = false;
    private TurnDirection allowedTurn;

    private Animator animator;
    private void Start()
    {
        way = FindObjectOfType<Lane>();
        if (way != null)
        {
            targetPosition = way.WayIndexToPosition(wayIndex);
            transform.localPosition = targetPosition;
            animator = GetComponentInChildren<Animator>();
            Debug.Log("PlayerMove Initialized in Start: WayIndex = " + wayIndex);
        }
        else
        {
            Debug.LogError("Lane not found!");
        }
    }

    // public void Initialize(Lane way)
    // {
    //     this.way = way;
    //     targetPosition = way.WayIndexToPosition(wayIndex);
    //     transform.localPosition = targetPosition;
    //     animator = GetComponentInChildren<Animator>();
    //     Debug.Log("PlayerMove Initialized: WayIndex = " + wayIndex);
    // }

    private void Update()
    {
        UpdateJump();
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 moveTarget = new Vector3(
            targetPosition.x,
            transform.localPosition.y + verticalVelocity * Time.deltaTime,
            transform.localPosition.z
        );
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveTarget, moveSpeed * Time.deltaTime);
    }

    public IEnumerator MoveTo(Vector3 destination)
    {
        var direction = (destination - transform.position).normalized;
        while (true)
        {
            Vector3 deltaMove = direction * moveSpeed * Time.deltaTime;
            if (deltaMove.sqrMagnitude > Vector3.Distance(transform.position, destination))
            {
                transform.position = destination;
                yield break;
            }
            else
            {
                transform.position = transform.position + deltaMove;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void UpdateJump()
    {
        if (isJumping)
        {
            verticalVelocity += gravity * Time.deltaTime;
            transform.localPosition += new Vector3(0, verticalVelocity * Time.deltaTime, 0);
        }
    }


    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (!canMove) return;
        if (context.performed) MoveLeft();
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (!canMove) return;
        if (context.performed) MoveRight();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (!canMove) return;
        if (context.performed) TryJump();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (!canMove) return;
        if (context.performed)
        {
            float direction = context.ReadValue<float>();
            if (direction < 0) TryRotateLeft();
            else if (direction > 0) TryRotateRight();
        }
    }
    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
            TryRotateLeft();
    }

    public void OnRotateRight(InputAction.CallbackContext context)
    {
        if (context.performed)
            TryRotateRight();
    }

    private void TryJump()
    {
        if (!isJumping && transform.position.y <= 0.01f)
        {
            isJumping = true;
            verticalVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);
            animator?.SetBool("Jump", true);
        }
    }

    private void MoveLeft()
    {
        if (isJumping) return;
        wayIndex = Mathf.Clamp(wayIndex - 1, 0, 2);
        targetPosition = way.WayIndexToPosition(wayIndex);
    }

    private void MoveRight()
    {
        if (isJumping) return;
        wayIndex = Mathf.Clamp(wayIndex + 1, 0, 2);
        targetPosition = way.WayIndexToPosition(wayIndex);
    }
   
    private void TryRotateLeft()
    {
        if (canTurn && (allowedTurn == TurnDirection.Left || allowedTurn == TurnDirection.Both))
        {
            onRotate?.Invoke(turnPivot.transform.position, 90f);
            canTurn = false;
        }
    }

    private void TryRotateRight()
    {
        if (canTurn && (allowedTurn == TurnDirection.Right || allowedTurn == TurnDirection.Both))
        {
            onRotate?.Invoke(turnPivot.transform.position, -90f);
            canTurn = false;
        }
    }

    public void SetCanTurn(bool value, GameObject turnPivot, TurnDirection direction)
    {
        canTurn = value;
        allowedTurn = direction;
        this.turnPivot = turnPivot;
    }
    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        currentTouchPosition = context.ReadValue<Vector2>();
    }
    public void OnTouchPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            swipeStart = currentTouchPosition;
        }
        else if (context.canceled)
        {
            Vector2 delta = currentTouchPosition - swipeStart;

            if (Mathf.Abs(delta.y) > 50f && Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
            {
                TryJump();
            }
            else if (Mathf.Abs(delta.x) > 50f && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x < 0f)
                    TryRotateLeft();
                else
                    TryRotateRight();
            }
            else if (delta.magnitude < 10f)
            {
                if (currentTouchPosition.x < Screen.width * 0.5f) MoveLeft();
                else MoveRight();
            }
        }
    }
    public void DisableInput()
    {
        canMove = false;
    }

    public void EnableInput()
    {
        canMove = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isJumping = false;
            verticalVelocity = 0f;
            Vector3 newPosition = transform.localPosition;
            newPosition.y = 0f;
            transform.localPosition = newPosition;
            animator?.SetBool("Jump", false);
        }
    }
}
