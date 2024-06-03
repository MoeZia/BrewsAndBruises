using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AnimationController animationController;
    private InputController inputController;
    private CombatController combatController;
    private Health health;

    private string playerID;

    void Start()
    {
        animationController = GetComponent<AnimationController>();
        health = GetComponent<Health>();
        inputController = GetComponent<InputController>();
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

        animationController.RegisterAnimation("RunForward", false);
        animationController.RegisterAnimation("RotateLeft", false);
        animationController.RegisterAnimation("RotateRight", false);
        animationController.RegisterAnimation("Jump", true);
        animationController.RegisterAnimation("Idle", false);
        animationController.RegisterAnimation("Fist", true);
        animationController.RegisterAnimation("Trumpet", true);
    }

    void RunForward()
    {
        if (animationController.IsInAnimationState("JumpWhileRunning") && !animationController.IsAnimationFinished())
        {
            return;
        }
        else
        {
            animationController.TriggerAnimation("RunForward");
        }
    }

    void RotateLeft()
    {
       
    }

    void RotateRight()
    {
        
    }

    void Jump()
    {
        animationController.TriggerAnimation("JumpWhileRunning");
    }

    void Idle()
    {
        if (animationController.IsInAnimationState("JumpWhileRunning") && !animationController.IsAnimationFinished())
        {
            return;
        }
        else
        {
            animationController.TriggerAnimation("Idle");
        }
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    private void OnHealthChanged(string id, int currentHealth, int maxHealth)
    {
        Debug.Log("Player Health Changed: " + currentHealth + "/" + maxHealth);
    }

    private void OnDeath(string id)
    {
        Debug.Log("Player with ID " + id + " died");
    }

    private void HandleWeaponChange(CombatModel.WeaponType weapon)
    {
        combatController.HandleWeaponChange(weapon);
        Debug.Log("Weapon changed to: " + weapon);
    }

    private void PerformAttack()
    {
        if (!animationController.IsAnimationBlocking())
        {
            switch (combatController.GetCurrentWeapon())
            {
                case CombatModel.WeaponType.Fist:
                    animationController.TriggerAnimation("Fist");
                    break;
                case CombatModel.WeaponType.Trumpet:
                    animationController.TriggerAnimation("Trumpet");
                    break;
            }

            combatController.PerformAttack();
           // Debug.Log("Performed attack.");
        }
    }
}
