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

    private bool canTurn = false;
    private TurnDirection allowedTurn;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        targetPosition = way.WayIndexToPosition(wayIndex);
        transform.position = targetPosition;
    }

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

    void TryJump()
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

    void MoveLeft()
    {
        wayIndex = Mathf.Clamp(wayIndex - 1, 0, 2);
        targetPosition = way.WayIndexToPosition(wayIndex);
    }

    void MoveRight()
    {
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

    void TryRotateLeft()
    {
        if (canTurn && allowedTurn == TurnDirection.Left)
        {
            mapSpawner.Rotate(90f);
            canTurn = false;
        }
    }

    void TryRotateRight()
    {
        if (canTurn && allowedTurn == TurnDirection.Right)
        {
            mapSpawner.Rotate(-90f);
            canTurn = false;
        }
    }

    public void SetCanTurn(bool value, TurnDirection direction)
    {
        canTurn = value;
        allowedTurn = direction;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isJumping = false;
            verticalVelocity = 0f;

            if (animator != null)
            {
                animator.SetBool("Jump", false);
            }
        }
    }
}
