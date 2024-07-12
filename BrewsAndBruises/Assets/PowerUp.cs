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
    public float powerUpValue = 20f; // The value to add for Health and Stamina or the multiplier for Speed
    public float speedDuration = 5f; // Duration for speed power-up effect

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                ApplyPowerUp(playerController);
                Destroy(gameObject); // Destroy the power-up object
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
}
