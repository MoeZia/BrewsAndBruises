using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AnimationController animationController;
    private InputControllerDiab inputController;
    private CombatController combatController;
    private Health health;

    private string playerID;

    void Start()
    {
        animationController = GetComponent<AnimationController>();
        health = GetComponent<Health>();
        inputController = GetComponent<InputControllerDiab>();
        combatController = GetComponent<CombatController>();

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

        // Register all animations with the Animation Controller
        animationController.RegisterAnimation("RunForward", false);
        animationController.RegisterAnimation("RotateLeft", false);
        animationController.RegisterAnimation("RotateRight", false);
        animationController.RegisterAnimation("Jump", true);
        animationController.RegisterAnimation("Idle", false);
        animationController.RegisterAnimation("Fist", true);
        animationController.RegisterAnimation("Trumpet", true);
        animationController.RegisterAnimation("Whip", true); // Assuming you add this
    }

    void RunForward()
    {
        if (animationController.IsInAnimationState("JumpWhileRunning") && !animationController.IsAnimationFinished())
        {
            return;
        }
        animationController.TriggerAnimation("RunForward");
    }

    void RotateLeft()
    {
        //animationController.TriggerAnimation("RotateLeft");
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
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    private void OnHealthChanged(string id, int currentHealth, int maxHealth)
    {
        Debug.Log($"Player Health Changed: {currentHealth}/{maxHealth}");
    }

    private void OnDeath(string id)
    {
        Debug.Log($"Player with ID {id} died");
    }

    private void HandleWeaponChange(CombatModel.WeaponType weapon)
    {
        combatController.HandleWeaponChange(weapon);
        TriggerWeaponAnimation(weapon);
    }

    private void TriggerWeaponAnimation(CombatModel.WeaponType weapon)
    {
        switch (weapon)
        {
            case CombatModel.WeaponType.Fist:
                animationController.TriggerAnimation("FistEquip");
                break;
            case CombatModel.WeaponType.Trumpet:
                animationController.TriggerAnimation("TrumpetEquip");
                break;
            case CombatModel.WeaponType.Whip:
                animationController.TriggerAnimation("WhipEquip");
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
                case CombatModel.WeaponType.Fist:
                    animationController.TriggerAnimation("Fist");
                    break;
                case CombatModel.WeaponType.Trumpet:
                    animationController.TriggerAnimation("Trumpet");
                    break;
                case CombatModel.WeaponType.Whip:
                    animationController.TriggerAnimation("Whip"); 
                    break;
            }

            combatController.PerformAttack();
        }
    }
}
