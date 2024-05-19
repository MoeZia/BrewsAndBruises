using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AnimationController animationController;
    private MovementController movementController;
    private Health health;

    private string playerID;

    void Start()
    {
        animationController = GetComponent<AnimationController>();
        health = GetComponent<Health>();
        movementController = GetComponent<MovementController>();

        health.Initialize(100);
        playerID = health.GetID();
        health.OnHealthChanged.AddListener(OnHealthChanged);
        health.OnDeath.AddListener(OnDeath);

        movementController.Initialize();
        movementController.OnRunForward.AddListener(RunForward);
        movementController.OnRotateLeft.AddListener(RotateLeft);
        movementController.OnRotateRight.AddListener(RotateRight);
        movementController.OnJump.AddListener(Jump);
        movementController.OnIdle.AddListener(Idle);
    }

    void RunForward()
    {
        if(animationController.IsInAnimationState("JumpWhileRunning") && !animationController.IsAnimationFinished())
        {
            return;
        }else {
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
        // Additional jump logic can be added here if necessary
    }

    void Idle()
    {
        if(animationController.IsInAnimationState("JumpWhileRunning") && !animationController.IsAnimationFinished())
        {
            return;
        }else {
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
}
