using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Health,
        Stamina,
        Speed
    }

    public PowerUpType powerUpType;
    public float powerUpValue = 20f;
    public float speedDuration = 5f;

    [SerializeField]
    private GameObject healthEffectPrefab;
    [SerializeField]
    private GameObject staminaEffectPrefab;
    [SerializeField]
    private GameObject speedEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                ApplyPowerUp(playerController);
                TriggerEffect();
                Destroy(gameObject); // Destroy the power-up object after applying the effect
            }
        }
    }

    private void ApplyPowerUp(PlayerController player)
    {
        switch (powerUpType)
        {
            case PowerUpType.Health:
                player.IncreaseHealth(powerUpValue);
                break;
            case PowerUpType.Stamina:
                player.IncreaseStamina(powerUpValue);
                break;
            case PowerUpType.Speed:
                player.StartSpeedBoost(powerUpValue, speedDuration);
                break;
        }
    }

    private void TriggerEffect()
    {
        GameObject effectPrefab = null;
        switch (powerUpType)
        {
            case PowerUpType.Health:
                effectPrefab = healthEffectPrefab;
                break;
            case PowerUpType.Stamina:
                effectPrefab = staminaEffectPrefab;
                break;
            case PowerUpType.Speed:
                effectPrefab = speedEffectPrefab;
                break;
        }

        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
    }
}
