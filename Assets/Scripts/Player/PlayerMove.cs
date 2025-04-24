using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public enum TurnDirection { Left, Right, Both }

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10f;

    public int laneIndex = 1;
    public float JumpHeight
    {
        get => playerStatus.statData.Jump;
    }

    public float gravity = -20f;
    public Lane way;
    private Vector3 targetPosition;
    private float verticalVelocity;
    public bool isJumping;
    private PlayerInput playerInput;
    private InputActionMap actionMap;
    private bool canMove = true;
    private Vector2 swipeStart;
    private Vector2 currentTouchPosition;
    [ReadOnly]
    public GameObject turnPivot;

    public Action<Vector3, float> onRotate;

    public static Action<int> OnJumpCounting;
    public bool canTurn = false;
    private TurnDirection allowedTurn;
    private PlayerStatus playerStatus;
    private Animator animator;
    private GameUIManager gameUIManager;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            actionMap = playerInput.currentActionMap;
        }
    }

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }
    private void Start()
    {
        way = FindObjectOfType<Lane>();
        if (way != null)
        {
            targetPosition = way.LaneIndexToPosition(laneIndex);
            transform.localPosition = targetPosition;
            Debug.Log("PlayerMove Initialized in Start: WayIndex = " + laneIndex);
        }
        else
        {
            Debug.LogError("Lane not found!");
        }

        var GameManager = GameObject.FindGameObjectWithTag(Utils.GameManagerTag);
        var GameManager_new = GameManager.GetComponent<GameManager_new>();
        gameUIManager = GameManager_new.UIManager;
        DisableInput();
        playerStatus = GetComponent<PlayerStatus>();
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
        if (playerStatus.IsDead())
        {
            Vector3 pos = transform.localPosition;
            // pos.y = 0f;
            transform.localPosition = pos;
            verticalVelocity = 0f;
            return;
        }
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
            Debug.Log("회전");
        gameUIManager.UnShowRotateButton();
        TryRotateLeft();
    }

    public void OnRotateRight(InputAction.CallbackContext context)
    {

        if (context.performed)
            Debug.Log("회전");
        gameUIManager.UnShowRotateButton();
        TryRotateRight();

    }

    private void TryJump()
    {
        // if (!isJumping && transform.position.y <= 0.01f)
        if (!isJumping)
        {
            isJumping = true;
            verticalVelocity = Mathf.Sqrt(-2f * gravity * JumpHeight);
            animator?.SetBool("Jump", true);
            OnJumpCounting?.Invoke(1);
        }
    }

    public void MoveLeft()
    {
        if (isJumping) return;
        laneIndex = Mathf.Clamp(laneIndex - 1, 0, 2);
        targetPosition = way.LaneIndexToPosition(laneIndex);
    }

    public void MoveRight()
    {
        if (isJumping) return;
        laneIndex = Mathf.Clamp(laneIndex + 1, 0, 2);
        targetPosition = way.LaneIndexToPosition(laneIndex);
    }



    public void MoveLaneIndexImmediate(int index)
    {
        var lanePosition = way.LaneIndexToPosition(index);
        lanePosition.y = transform.localPosition.y;
        laneIndex = index;

        //진행중이던 이동 코드 리셋
        targetPosition = lanePosition;
        transform.localPosition = lanePosition;
    }

    private void TryRotateLeft()
    {
        if (canTurn && (allowedTurn == TurnDirection.Left || allowedTurn == TurnDirection.Both))
        {
            onRotate?.Invoke(turnPivot.transform.position, -90f);
            // StartCoroutine(RemoveInvincibleAfterDelay(0.5f));
            canTurn = false;
        }


    }

    private void TryRotateRight()
    {
        if (canTurn && (allowedTurn == TurnDirection.Right || allowedTurn == TurnDirection.Both))
        {
            onRotate?.Invoke(turnPivot.transform.position, 90f);
            // StartCoroutine(RemoveInvincibleAfterDelay(0.5f));
            canTurn = false;
        }

    }
    private IEnumerator RemoveInvincibleAfterDelay(float delay)
    {
        playerStatus.SetInvincible(true);
        yield return new WaitForSeconds(delay);
        playerStatus.SetInvincible(false);
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

    public void OnTouch(InputAction.CallbackContext context)
    {
        // if (context.performed)
        // {
        //     Vector2 pos = Touchscreen.current.primaryTouch.position.ReadValue();
        //     Debug.Log("탭 입력 감지");

        //     if (pos.x < Screen.width * 0.5f) MoveLeft();
        //     else MoveRight();
        // }
    }

    public void OnTouchPress(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            swipeStart = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (context.canceled)
        {
            Vector2 current = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 delta = current - swipeStart;
            if (playerStatus.IsDead())
            {
                return;
            }
            if (delta.y > 30f && Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
            {
                TryJump();
            }
            // 왼쪽 스와이프
            else if (delta.x < -30f && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                // if (canTurn)
                //     TryRotateLeft();
                // else
                MoveLeft();
            }
            // 오른쪽 스와이프
            else if (delta.x > 30f && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                // if (canTurn)
                //     TryRotateRight();
                // else
                MoveRight();
            }
        }
    }



    public void DisableInput()
    {
        // canMove = false;
        actionMap.Disable();
    }

    public void EnableInput()
    {
        // canMove = true;
        actionMap.Enable();
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



    // 자동 회전
    public void TryAutoRotateLeft()
    {
        if (canTurn && (allowedTurn == TurnDirection.Left || allowedTurn == TurnDirection.Both))
        {
            onRotate?.Invoke(turnPivot.transform.position, -90f);
            canTurn = false;
        }
    }

    public void TryAutoRotateRight()
    {
        if (canTurn && (allowedTurn == TurnDirection.Right || allowedTurn == TurnDirection.Both))
        {
            onRotate?.Invoke(turnPivot.transform.position, 90f);
            canTurn = false;
        }
    }

}
