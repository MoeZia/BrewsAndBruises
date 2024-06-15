using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    
    private int currentHealth;

    
    public string id;

    private int c = 0;


    public UnityEvent<string, int, int> OnHealthChanged;


    public UnityEvent<string> OnDeath;

    [SerializeField] HealthBarScript healthBar;


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
        initAndUpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("blabla");
        currentHealth -= damage;
        OnHealthChanged?.Invoke(id, currentHealth, maxHealth);
      
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
        // destroy the game object !!! change this to a death animation or something ..... 
        Destroy(gameObject);
    }

    private void initAndUpdateHealthBar(float currentHealth, float maxHealth) {

        if(gameObject.CompareTag("Player")) {
            healthBar = GameObject.FindGameObjectsWithTag("HealthBar")[0].GetComponent<HealthBarScript>();
        } else {
            // is enemy   
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
