using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public string id;
    public UnityEvent<string, int, int> OnHealthChanged;
    public UnityEvent<string> OnDeath;
    [SerializeField] private HealthBarScript healthBar;

    [SerializeField] private GameObject deathEffectPrefab;  // Reference to your particle system prefab

    void Start()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
        }
        Initialize(maxHealth);
    }

    public void Initialize(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(id, currentHealth, maxHealth);
        initAndUpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(id, currentHealth, maxHealth);
        Debug.Log(healthBar);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(id, currentHealth, maxHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke(id);
        Debug.Log(gameObject.name + " died");
        
        // Instantiate the death effect before destroying the object
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void initAndUpdateHealthBar(float currentHealth, float maxHealth) {
        if(gameObject.CompareTag("Player")) {
            healthBar = GameObject.FindGameObjectsWithTag("HealthBar")[0].GetComponent<HealthBarScript>();
        } else {
            healthBar = GetComponentInChildren<HealthBarScript>();
        }
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public string GetID()
    {
        return id;
    }
}
