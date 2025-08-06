using UnityEngine;

public class Bread : MonoBehaviour
{
    public string itemName = "Bread";
    public float healAmount = 10f;
    public float energyBoostAmount = 3f;
    public float energyBoostDuration = 10f;

    // Reference to the player's stats (set this in inspector or find at runtime)
    public PlayerStats playerStats;

    // Call this method to "use" the bread
    public void Use()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats reference is missing!");
            return;
        }

        playerStats.Heal(healAmount);
        playerStats.ApplyEnergyBoost(energyBoostAmount, energyBoostDuration);

        Debug.Log($"{itemName} used: Healed {healAmount} health and boosted energy by {energyBoostAmount} for {energyBoostDuration} seconds.");
        
        // Here you can add code to remove bread from inventory after use, etc.
    }
}
