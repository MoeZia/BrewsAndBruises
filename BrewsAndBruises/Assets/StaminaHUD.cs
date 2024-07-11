using UnityEngine;
using UnityEngine.UI;

public class StaminaHUD : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private CombatModel combatModel; // Reference to your combat model

    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;
    [SerializeField] private float staminaRecoveryRate = 5f; // Stamina recovered per second

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar(currentStamina, maxStamina);
    }

    public void UpdateStaminaBar(float currentStamina, float maxStamina) 
    {
        slider.value = currentStamina / maxStamina;
    }

    private void Update()
    {
        if (camera && target) 
        {
            transform.rotation = camera.transform.rotation;
            transform.position = target.position;
        }

        RecoverStaminaOverTime();
    }

    public bool UseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            UpdateStaminaBar(currentStamina, maxStamina);
            return true;
        }
        else
        {
            Debug.Log("Not enough stamina!");
            return false;
        }
    }

    public void RecoverStamina(float amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        UpdateStaminaBar(currentStamina, maxStamina);
    }

    private void RecoverStaminaOverTime()
    {
        currentStamina = Mathf.Min(currentStamina + staminaRecoveryRate * Time.deltaTime, maxStamina);
        UpdateStaminaBar(currentStamina, maxStamina);
    }
}
