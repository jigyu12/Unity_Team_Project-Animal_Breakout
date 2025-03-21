using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10f;
    public int wayIndex = 1;
    public float jumpHeight = 2f;
    public float jumpDuration = 0.5f;
    public float gravity = -20f;
    public MapWay way;

    private Vector3 targetPosition;
    private float verticalVelocity;
    private bool isJumping;
    private float jumpTime;

    void Start()
    {
        targetPosition = way.WayIndexToPosition(wayIndex);
        transform.position = targetPosition;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.A)) MoveLeft();
        else if (Input.GetKeyDown(KeyCode.D)) MoveRight();
        else if (Input.GetKeyDown(KeyCode.W)) TryJump();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouch();
#endif

        UpdateJump();

        Vector3 moveTarget = new Vector3(targetPosition.x, transform.position.y + verticalVelocity * Time.deltaTime, targetPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
    }

    void TryJump()
    {
        if (!isJumping)
        {
            isJumping = true;
            jumpTime = 0f;
            verticalVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);
        }
    }

    void UpdateJump()
    {
        if (isJumping)
        {
            verticalVelocity += gravity * Time.deltaTime;
            transform.position += new Vector3(0, verticalVelocity * Time.deltaTime, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isJumping = false;
            verticalVelocity = 0f;

            Vector3 pos = transform.position;
            pos.y = 0f;
            transform.position = pos;
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

    Vector2 touchStart;

    void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            Vector2 delta = touch.position - touchStart;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (delta.x < 0) MoveLeft();
                else MoveRight();
            }
            else if (delta.y > 50f)
            {
                TryJump();
            }
        }
    }
}
