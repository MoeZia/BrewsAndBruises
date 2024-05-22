using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float jumpForce = 5.0f;
    public float rotationSpeed = 100.0f;
    private Rigidbody rb;
    private Vector3 moveDirection;

    public UnityEvent OnRunForward;
    public UnityEvent OnRotateLeft;
    public UnityEvent OnRotateRight;
    public UnityEvent OnJump;
    public UnityEvent OnIdle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitializeEvents();
    }

    void Update()
    {
        HandleMovementInput();
        Move();
    }

    public void Initialize()
    {
        InitializeEvents();
    }

    private void InitializeEvents()
    {
        if (OnRunForward == null)
            OnRunForward = new UnityEvent();
        if (OnRotateLeft == null)
            OnRotateLeft = new UnityEvent();
        if (OnRotateRight == null)
            OnRotateRight = new UnityEvent();
        if (OnJump == null)
            OnJump = new UnityEvent();
        if (OnIdle == null)
            OnIdle = new UnityEvent();
    }

    private void HandleMovementInput()
    {
        bool isMoving = false;

        if (Input.GetKey(KeyCode.W))
        {
            SetMoveDirection(Vector3.forward);
            OnRunForward?.Invoke();
            isMoving = true;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Rotate(Vector3.up, -rotationSpeed);
            OnRotateLeft?.Invoke();
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Rotate(Vector3.up, rotationSpeed);
            OnRotateRight?.Invoke();
            isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            OnJump?.Invoke();
            isMoving = true;
        }

        if (!isMoving)
        {
            OnIdle?.Invoke();
        }
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    private void Move()
    {
        if (moveDirection != Vector3.zero)
        {
            Vector3 move = transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
    }

    private void Rotate(Vector3 axis, float angle)
    {
        transform.Rotate(axis, angle * Time.deltaTime);
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }
}
