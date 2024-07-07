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

        /// Register all sounds with the Audio Manager
        /// FindObjectOfType<AudioManager>().Play("running");
        /// FindObjectOfType<AudioManager>().Play("glassHit");
        /// FindObjectOfType<AudioManager>().Play("brezel");
        /// FindObjectOfType<AudioManager>().Play("trumpet");
        /// FindObjectOfType<AudioManager>().Play("lose");
        /// FindObjectOfType<AudioManager>().Play("BackgroundMusic");
        /// FindObjectOfType<AudioManager>().Play("BackgroundPeople");
       // This is pretty intensive, becase it needs to be searched for every time so lets make a link to the Audio manager on start..
       AudioManager audioManager = FindObjectOfType<AudioManager>();
       /// and then we dont have to search every time we want to play a sound or stop a sound

        
    }

    void RunForward()
    {
        if (animationController.IsInAnimationState("JumpWhileRunning") && !animationController.IsAnimationFinished())
        {
            return;
        }
        animationController.TriggerAnimation("RunForward");
        FindObjectOfType<AudioManager>().Play("running");
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
        FindObjectOfType<AudioManager>().Stop("running");
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
        FindObjectOfType<AudioManager>().Stop("BackgroundMusic");
        FindObjectOfType<AudioManager>().Stop("BackgroundPeople");
        FindObjectOfType<AudioManager>().Play("lose");
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
                animationController.TriggerAnimation("FistEquip");
                break;
            case CombatModel.WeaponType.Breze:
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
                case CombatModel.WeaponType.Mug:
                    animationController.TriggerAnimation("Fist");
                    FindObjectOfType<AudioManager>().Play("glassHit");
                    break;
                case CombatModel.WeaponType.Breze:
                    animationController.TriggerAnimation("Whip");
                    FindObjectOfType<AudioManager>().Play("brezel");
                    break;
                case CombatModel.WeaponType.Whip:
                    animationController.TriggerAnimation("Whip"); 
                    FindObjectOfType<AudioManager>().Play("trumpet");
                    break;
            }

            combatController.PerformAttack();
        }
    }
}
