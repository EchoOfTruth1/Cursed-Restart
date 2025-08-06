using UnityEngine;
using System.Collections;

[System.Serializable]
public class Stat
{
    public float baseValue;
    public float multiplier = 1f;
    public float flatBonus = 0f;

    public float GetValue()
    {
        return (baseValue + flatBonus) * multiplier;
    }
}

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public Stat moveSpeed;
    public Stat attackDamage;
    public Stat attackSpeed;
    public Stat health;
    public Stat armor;
    public Stat critChance;
    public Stat critDamage;

    [Header("Current Runtime Stats")]
    public float currentHealth = 20f;
    public float energy = 100f;

    [Header("Upgrade Costs")]
    public int moveSpeedCost = 10;
    public int attackDamageCost = 15;
    public int attackSpeedCost = 20;
    public int healthCost = 12;
    public int armorCost = 18;

    [Header("Currency")]
    public int currency = 200;

    void Start()
    {
        // Initialize health on start
        currentHealth = GetMaxHealth();
    }

    // Getters
    public float GetCurrentMoveSpeed() => moveSpeed.GetValue();
    public float GetCurrentAttackDamage() => attackDamage.GetValue();
    public float GetCurrentAttackSpeed() => attackSpeed.GetValue();
    public float GetCurrentArmor() => armor.GetValue();
    public float GetMaxHealth() => health.GetValue();

    public float GetCurrentHealth()
{
    return currentHealth;
}

    // Upgrade Methods
    public bool UpgradeMoveSpeed()
    {
        if (currency >= moveSpeedCost)
        {
            currency -= moveSpeedCost;
            moveSpeed.flatBonus += 0.5f;
            moveSpeedCost = Mathf.RoundToInt(moveSpeedCost * 1.2f);
            return true;
        }
        return false;
    }

    public bool UpgradeAttackDamage()
    {
        if (currency >= attackDamageCost)
        {
            currency -= attackDamageCost;
            attackDamage.flatBonus += 2f;
            attackDamageCost = Mathf.RoundToInt(attackDamageCost * 1.2f);
            return true;
        }
        return false;
    }

    public bool UpgradeAttackSpeed()
    {
        if (currency >= attackSpeedCost)
        {
            currency -= attackSpeedCost;
            attackSpeed.flatBonus += 0.1f;
            attackSpeedCost = Mathf.RoundToInt(attackSpeedCost * 1.2f);
            return true;
        }
        return false;
    }

    public bool UpgradeHealth()
    {
        if (currency >= healthCost)
        {
            currency -= healthCost;
            health.flatBonus += 10f;
            healthCost = Mathf.RoundToInt(healthCost * 1.2f);
            return true;
        }
        return false;
    }

    public bool UpgradeArmor()
    {
        if (currency >= armorCost)
        {
            currency -= armorCost;
            armor.flatBonus += 1f;
            armorCost = Mathf.RoundToInt(armorCost * 1.2f);
            return true;
        }
        return false;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
    }

    public void ResetStats()
    {
        moveSpeed.flatBonus = 0f;
        attackDamage.flatBonus = 0f;
        attackSpeed.flatBonus = 0f;
        health.flatBonus = 0f;
        armor.flatBonus = 0f;

        moveSpeedCost = 10;
        attackDamageCost = 15;
        attackSpeedCost = 20;
        healthCost = 12;
        armorCost = 18;

        currency = 200;
        currentHealth = GetMaxHealth();
        energy = 100f;
    }

    public void Heal(float amount)
{
    currentHealth += amount;
    currentHealth = Mathf.Min(currentHealth, GetMaxHealth());
}

public void ApplyEnergyBoost(float amount, float duration)
{
    StartCoroutine(EnergyBoostRoutine(amount, duration));
}

private IEnumerator EnergyBoostRoutine(float amount, float duration)
{
    energy += amount;
    yield return new WaitForSeconds(duration);
    energy -= amount;

    // Optional: Clamp energy so it doesn't go below 0
    energy = Mathf.Max(0f, energy);
}
// Add this to your PlayerStats.cs file
public void TakeDamage(float damage)
{
    // Apply armor reduction
    float actualDamage = Mathf.Max(1f, damage - GetCurrentArmor());
    
    currentHealth -= actualDamage;
    currentHealth = Mathf.Max(0f, currentHealth);
    
    Debug.Log($"Player takes {actualDamage} damage! Health: {currentHealth}/{GetMaxHealth()}");
    
    // Notify health bar of damage
    FloatingHealthBar healthBar = FindObjectOfType<FloatingHealthBar>();
    if (healthBar != null)
    {
        healthBar.OnPlayerTakeDamage(actualDamage);
    }
    
    // Check if player died
    if (currentHealth <= 0)
    {
        OnPlayerDeath();
    }
}

private void OnPlayerDeath()
{
    Debug.Log("Player has died!");
    // Add death logic here
    // For now, respawn with half health
    currentHealth = GetMaxHealth() * 0.5f;
}

}
