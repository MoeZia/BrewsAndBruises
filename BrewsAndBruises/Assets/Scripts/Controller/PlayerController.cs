using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private AnimationController animationController;
    private InputControllerDiab inputController;
    private CombatController combatController;
    private Health health;

    private string playerID;

    public float maxHealth = 100f;
    public float maxStamina = 100f;
    private float currentHealth;
    private float currentStamina;
    public float moveSpeed = 5f;

    private bool isSpeedBoostActive = false;
    private float originalMoveSpeed;
    private float speedBoostEndTime;

    private CameraFollow CameraFollow;

    private AudioManager audioManager;

    void Start()
    {
        animationController = GetComponent<AnimationController>();
        health = GetComponent<Health>();
        inputController = GetComponent<InputControllerDiab>();
        combatController = GetComponent<CombatController>();
        audioManager = FindObjectOfType<AudioManager>();
        CameraFollow = FindObjectOfType<CameraFollow>();

        health.Initialize(100);
        playerID = health.GetID();
        health.OnHealthChanged.AddListener(OnHealthChanged);
        health.OnDeath.AddListener(OnDeath);

        inputController.Initialize();
        inputController.OnRunForward.AddListener(RunForward);
        inputController.OnRotateLeft.AddListener(RotateLeft);
        inputController.OnRotateRight.AddListener(RotateRight);
        inputController.OnJump.AddListener(Jump);
        inputController.OnIdle.AddListener(Idle);
        inputController.OnWeaponChange.AddListener(HandleWeaponChange);
        inputController.OnAttack.AddListener(PerformAttack);
        inputController.OnBlockStart.AddListener(BlockStart);
        inputController.OnBlockEnd.AddListener(BlockEnd);

        // Register all animations with the Animation Controller
        animationController.RegisterAnimation("RunForward", false);
        animationController.RegisterAnimation("RotateLeft", false);
        animationController.RegisterAnimation("RotateRight", false);
        animationController.RegisterAnimation("BlockingLoop", true);
        animationController.RegisterAnimation("Jump", true);
        animationController.RegisterAnimation("Idle", false);
        animationController.RegisterAnimation("Fist", true);
        animationController.RegisterAnimation("Trumpet", true);
        animationController.RegisterAnimation("Whip", true); // Assuming you add this
    }
    void Update()
    {
        //check if Health has changes
        if (currentHealth < health.GetCurrentHealth())
        {
            currentHealth = health.GetCurrentHealth();
            CameraFollow.Hitshake();
        }
        if (isSpeedBoostActive && Time.time > speedBoostEndTime)
        {
            EndSpeedBoost();
        }

    }

    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        health.Heal((int)currentHealth);
       
    }

    public void IncreaseStamina(float amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        // Assuming you have a stamina HUD or system to update the UI
        FindObjectOfType<StaminaHUD>().RecoverStamina(currentStamina);
        
    }

    public void StartSpeedBoost(float speedMultiplier, float duration)
    {
        if (!isSpeedBoostActive)
        {
            originalMoveSpeed = moveSpeed;
            moveSpeed *= speedMultiplier;
            isSpeedBoostActive = true;
            speedBoostEndTime = Time.time + duration;
            inputController.UpdateMoveSpeed(moveSpeed);
            
        }
    }

    private void EndSpeedBoost()
    {
        moveSpeed = originalMoveSpeed;
        isSpeedBoostActive = false;
        inputController.UpdateMoveSpeed(moveSpeed);
        Debug.Log("Speed boost ended. Move speed reset to " + moveSpeed);
    }

    void RunForward()
    {
        if (animationController.IsInAnimationState("JumpWhileRunning") && !animationController.IsAnimationFinished())
        {
            return;
        }
        animationController.TriggerAnimation("RunForward");
        audioManager.Play("running");
    }

    void RotateLeft()
    {
        //animationController.TriggerAnimation("RotateLeft");
    }
    void BlockStart(){
        animationController.TriggerAnimation("BlockingLoop");
    }
    void BlockEnd(){
        animationController.TriggerAnimation("Idle");
    }

    void RotateRight()
    {
        //animationController.TriggerAnimation("RotateRight");
    }

    void Jump()
    {
        animationController.TriggerAnimation("JumpWhileRunning");
    }

    void Idle()
    {
        animationController.TriggerAnimation("Idle");
        audioManager.Stop("running");
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    private void OnHealthChanged(string id, int currentHealth, int maxHealth)
    {
      //  Debug.Log($"Player Health Changed: {currentHealth}/{maxHealth}");
    }

    private void OnDeath(string id)
    {
        Debug.Log($"Player with ID {id} died");
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        audioManager.Stop("BackgroundMusic");
        audioManager.Stop("BackgroundPeople");
        audioManager.Play("lose");
        SceneManager.LoadSceneAsync("Lose");


        //GameObject eventSystem = GameObject.Find("EventSystem");
        //eventSystem.GetComponent<EndOfGameMenuScript>().ActivateMenu();
        //eventSystem.GetComponent<EndOfGameMenuScript>().setHeadLine("You Los");
    }

    private void HandleWeaponChange(CombatModel.WeaponType weapon)
    {
        combatController.HandleWeaponChange(weapon);
        //TriggerWeaponAnimation(weapon);
    }
    public void KnockBackForce(Vector3 froce)
    {
       GetComponent<Rigidbody>().AddForce(froce, ForceMode.Impulse);
    }

    private void TriggerWeaponAnimation(CombatModel.WeaponType weapon)
    {
        switch (weapon)
        {
            case CombatModel.WeaponType.Mug:
                //animationController.TriggerAnimation("FistEquip");
                break;
            case CombatModel.WeaponType.Breze:
                //animationController.TriggerAnimation("TrumpetEquip");
                break;
            case CombatModel.WeaponType.Trumpet:
                //animationController.TriggerAnimation("WhipEquip");
                break;
        }
    }

    private void PerformAttack()
    {
        if (!animationController.IsAnimationBlocking())
        {
            CombatModel.WeaponType currentWeapon = combatController.GetCurrentWeapon();
            switch (currentWeapon)
            {
                case CombatModel.WeaponType.Mug:
                    if(!animationController.IsInSpecificAnimationState("Fist", true))
                    {
                    break;
                    }
                    animationController.TriggerAnimation("Fist");
                    audioManager.Play("glassHit");
                    combatController.PerformAttack();
                    break;
                case CombatModel.WeaponType.Breze:
                    animationController.TriggerAnimation("Whip");
                    audioManager.Play("brezel");
                    combatController.PerformAttack();
                    break;
                case CombatModel.WeaponType.Trumpet:
                    animationController.TriggerAnimation("Whip"); 
                    combatController.PerformAttack();
                    break;
            }

        
        }
    }
}
