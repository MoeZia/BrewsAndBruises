using UnityEngine;
using UnityEngine.Events;

public class InputControllerDiab : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Adjusted for faster movement
    public float jumpForce = 5.0f;
    public float rotationSpeed = 1000.0f; // Faster rotation to instantly face the direction
    private Rigidbody rb;
    private AnimationController animationController;

    public UnityEvent OnRunForward;
    public UnityEvent OnRotateLeft;
    public UnityEvent OnRotateRight;
    public UnityEvent OnJump;
    public UnityEvent OnIdle;
    public UnityEvent<CombatModel.WeaponType> OnWeaponChange;
    public UnityEvent OnAttack;

    private Vector3 targetPosition;

    [SerializeField]
    private bool isMoving = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animationController = GetComponent<AnimationController>();
        InitializeEvents();
    }

    void Update()
    {
        HandleCombatInput();

        HandleMovementInput();

        if (isMoving&& !hasReachedTarget())
            MoveTowardsTarget();
        else{
            isMoving = false;
        }
    }

    public void Initialize()
    {
        InitializeEvents();
    }

    private void InitializeEvents()
    {
        OnRunForward ??= new UnityEvent();
        OnRotateLeft ??= new UnityEvent();
        OnRotateRight ??= new UnityEvent();
        OnJump ??= new UnityEvent();
        OnIdle ??= new UnityEvent();
        OnWeaponChange ??= new UnityEvent<CombatModel.WeaponType>();
        OnAttack ??= new UnityEvent();
    }

    private void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button for movement
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                targetPosition = hit.point;
                isMoving = true;
                OnRunForward?.Invoke();
            }
        }

        if (isMoving && Vector3.Distance(transform.position, targetPosition) > 0.5f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            RotateTowards(direction);
        }
        if (!isMoving&& animationController.IsAnimationFinished())
        {
            OnIdle?.Invoke();
        }


        if (Input.GetKeyDown(KeyCode.Space)) // Handle jump
        {
            Jump();
            OnJump?.Invoke();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
        if(animationController.IsInAnimationState("Fist")&& animationController.IsAnimationFinished())
        {
            OnRunForward?.Invoke();
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleCombatInput()
{
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
        OnWeaponChange?.Invoke(CombatModel.WeaponType.Mug);
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
        OnWeaponChange?.Invoke(CombatModel.WeaponType.Breze);
    }

    // Check for attack input
    if (Input.GetMouseButtonDown(0)) // Left mouse button for attack
    {
        // Determine target position from mouse click
        Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray2, out RaycastHit hit, 100))
        {
        
            Vector3 direction = (hit.point - transform.position).normalized;
            // check if player looks in the right direction rotation TODO!!!
            
            
            RotateTowards(direction); // Rotate towards the target position on attack
        }

        OnAttack?.Invoke();
        Debug.Log("Attack");
    }
}



    public void Jump()
    {
        if (IsGrounded())
        {
            //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            OnJump?.Invoke();
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.1f + 0.1f * transform.localScale.y);
    }

    // Public method to check if the player is currently walking
    public bool IsWalking()
    {
        return isMoving;
    }

    private bool hasReachedTarget()
    {
        return Vector3.Distance(transform.position, targetPosition) < 0.5f;
    }
}
