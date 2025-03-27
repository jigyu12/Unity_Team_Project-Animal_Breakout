using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TurnDirection { Left, Right }

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10f;
    public int wayIndex = 1;
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public MapWay way;
    public MapSpawn mapSpawner;

    private Vector3 targetPosition;
    private float verticalVelocity;
    private bool isJumping;

    private Vector2 swipeStart;
    private Vector2 currentTouchPosition;

    [ReadOnly]
    public GameObject turnPivot;

    public Action<Vector3, float> onRotate;


    private bool canTurn = false;
    private TurnDirection allowedTurn;

    private Animator animator;

    public void Init(MapWay way, MapSpawn mapSpawner)
    {
        this.way = way;
        this.mapSpawner = mapSpawner;

        targetPosition = way.WayIndexToPosition(wayIndex);
        transform.position = targetPosition;

        animator = GetComponent<Animator>();
    }


    // void Start()
    // {
    //     animator = GetComponent<Animator>();
    //     targetPosition = way.WayIndexToPosition(wayIndex);
    //     transform.position = targetPosition;
    // }

    void Update()
    {
        UpdateJump();

        Vector3 moveTarget = new Vector3(
            targetPosition.x,
            transform.position.y + verticalVelocity * Time.deltaTime,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
    }

    void UpdateJump()
    {
        if (isJumping)
        {
            verticalVelocity += gravity * Time.deltaTime;
            transform.position += new Vector3(0, verticalVelocity * Time.deltaTime, 0);
        }
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed) MoveLeft();
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed) MoveRight();



    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) TryJump();
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

    private void TryJump()
    {
        if (!isJumping && transform.position.y <= 0.01f)
        {
            isJumping = true;
            verticalVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);
            if (animator != null)
            {
                animator.SetBool("Jump", true);
            }
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

    private void TryRotateLeft()
    {
        if (canTurn && allowedTurn == TurnDirection.Left)
        {
            //mapSpawner.Rotate(90f);
            onRotate?.Invoke(turnPivot.transform.position, 90f);
            canTurn = false;
        }
    }

    private void TryRotateRight()
    {
        if (canTurn && allowedTurn == TurnDirection.Right)
        {
            //mapSpawner.Rotate(-90f);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isJumping = false;
            verticalVelocity = 0f;
            Vector3 newPosition = transform.position;
            newPosition.y = 0f;
            transform.position = newPosition;
            if (animator != null)
            {
                animator.SetBool("Jump", false);
            }
        }
    }
}
