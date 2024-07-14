using UnityEngine;
using UnityEngine.Events;

public class InputControllerDiab : MonoBehaviour
{
    public float moveSpeed = 9.0f; // Adjusted for faster movement
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
    public UnityEvent OnBlockStart;
    public UnityEvent OnBlockEnd;

    private Vector3 targetPosition;

    [SerializeField]
    private bool isMoving = false;
    [SerializeField]
    public bool isBlocking = false; // Public field for blocking state

    public GameObject mousePointer;
    private GameObject currentPointer;

    [SerializeField] private StaminaHUD staminaHUD; // Reference to the StaminaHUD
    [SerializeField] private TrumpetWeapon trumpetWeapon; // Reference to the TrumpetWeapon

    private CombatModel combatModel;
    private AudioManager audioManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animationController = GetComponent<AnimationController>();
        InitializeEvents();
        combatModel = new CombatModel(); // Initialize CombatModel
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        HandleCombatInput();
        HandleMovementInput();
        HandleBlockingInput(); // Handle blocking input

        if (isMoving && !hasReachedTarget() && !isBlocking)
            MoveTowardsTarget();
        else
        {
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
        OnBlockStart ??= new UnityEvent();
        OnBlockEnd ??= new UnityEvent();
    }

    private void HandleMovementInput()
    {
        if (Input.GetMouseButtonDown(1) && !isBlocking) // Right mouse button for movement
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
            {
                targetPosition = hit.point;
                isMoving = true;
                addMousePointer(targetPosition);
                audioManager.Play("click");
                OnRunForward?.Invoke();
            }
        }

        if (isMoving && Vector3.Distance(transform.position, targetPosition) > 0.5f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            RotateTowards(direction);
        }
        if (!isMoving && animationController.IsAnimationFinished() && !isBlocking)
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
        if (animationController.IsInAnimationState("Fist") && animationController.IsAnimationFinished())
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
            combatModel.SetWeapon(CombatModel.WeaponType.Mug);
            audioManager.Play("rascheln");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnWeaponChange?.Invoke(CombatModel.WeaponType.Breze);
            combatModel.SetWeapon(CombatModel.WeaponType.Breze);
            audioManager.Play("rascheln");

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnWeaponChange?.Invoke(CombatModel.WeaponType.Trumpet);
            combatModel.SetWeapon(CombatModel.WeaponType.Trumpet);
            audioManager.Play("rascheln");
        }

        // Check for attack input
        if (Input.GetMouseButtonDown(0) && !isBlocking) // Left mouse button for attack
        {
            if (combatModel.GetCurrentWeapon() == CombatModel.WeaponType.Trumpet)
            {
                trumpetWeapon.StartAttack(); // Start attack for trumpet
            }
            else
            {
                // Handle other weapon attacks here
                float requiredStamina = 7f; // Default stamina usage
                if (combatModel.GetCurrentWeapon() == CombatModel.WeaponType.Mug)
                {
                    if(staminaHUD.UseStamina(requiredStamina)){
                    // Determine target position from mouse click
                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit, 100))
                    {
                        Vector3 direction = (hit.point - transform.position).normalized;
                        RotateTowards(direction); // Rotate towards the target position on attack
                    }

                    OnAttack?.Invoke();
                }
                }
                else if (combatModel.GetCurrentWeapon() == CombatModel.WeaponType.Breze)
                {
                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit, 100))
                    {
                        Vector3 direction = (hit.point - transform.position).normalized;
                        RotateTowards(direction); // Rotate towards the target position on attack
                    }

                    OnAttack?.Invoke();
                }
            }
        }
        else if (!Input.GetMouseButton(0) && combatModel.GetCurrentWeapon() == CombatModel.WeaponType.Trumpet)
        {
            trumpetWeapon.StopAttack(); // Stop attack for trumpet
        }
    }

    private void HandleBlockingInput()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // While Shift key is held down
        {
            if (!isBlocking)
            {
                isBlocking = true;
                OnBlockStart?.Invoke();
            }
        }
        else
        {
            if (isBlocking)
            {
                isBlocking = false;
                OnBlockEnd?.Invoke();
            }
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

    private void addMousePointer(Vector3 position)
    {
        position.y += 0.1f;

        if (currentPointer)
        {
            Destroy(currentPointer);
        }
        currentPointer = Instantiate(mousePointer, position, Quaternion.identity);
    }
    public void UpdateMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
