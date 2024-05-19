using UnityEngine;

public class CombatView : MonoBehaviour
{
    private AnimationController animationController;

    void Start()
    {
        animationController = GetComponent<AnimationController>();
    }

    public void PlayAttackAnimation(CombatModel.WeaponType weapon)
    {
        switch (weapon)
        {
            case CombatModel.WeaponType.Fist:
                animationController.TriggerAnimation("Fist");
                break;
            case CombatModel.WeaponType.Trumpet:
                animationController.TriggerAnimation("Trumpet");
                break;
            default:
                break;
        }
    }

    public void ApplyAttackEffects(CombatModel.WeaponType weapon, GameObject enemy, float force)
    {
        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
        if (enemyRb != null)
        {
            Vector3 pushDirection = enemy.transform.position - transform.position;
            enemyRb.AddForce(pushDirection.normalized * force, ForceMode.Impulse);
        }
    }
}
