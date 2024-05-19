using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    
    public string id;


    public UnityEvent<string, int, int> OnHealthChanged;


    public UnityEvent<string> OnDeath;

    void Start()
    {
        // Generate a unique identifier if not set
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
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(id, currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(id, currentHealth, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke(id);
        Debug.Log(gameObject.name + " died");
        // destroy the game object !!! change this to a death animation or something ..... 
        Destroy(gameObject);
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
