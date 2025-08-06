using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    [Header("Player Reference")]
    public PlayerStats player;

    [Header("Stat Text Elements")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackDamageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critDamageText;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI energyText;

    [Header("Health Bar")]
    public Slider healthBar;
    public TextMeshProUGUI healthBarText;

    [Header("Energy Bar")]
    public Slider energyBar;
    public TextMeshProUGUI energyBarText;

    [Header("Upgrade Cost Display")]
    public TextMeshProUGUI speedCostText;
    public TextMeshProUGUI attackCostText;
    public TextMeshProUGUI attackSpeedCostText;
    public TextMeshProUGUI healthCostText;
    public TextMeshProUGUI armorCostText;

    [Header("UI Settings")]
    public bool showDetailedStats = true;
    public bool showUpgradeCosts = true;
    public Color canAffordColor = Color.green;
    public Color cantAffordColor = Color.red;

    void Update()
    {
        if (player == null) return;

        UpdateBasicStats();
        UpdateHealthBar();
        UpdateEnergyBar();
        
        if (showDetailedStats)
            UpdateDetailedStats();
            
        if (showUpgradeCosts)
            UpdateUpgradeCosts();
    }

    void UpdateBasicStats()
    {
        // Basic stats that should always be shown
        if (speedText != null)
            speedText.text = $"Speed: {player.GetCurrentMoveSpeed():F1}";

        if (healthText != null)
            healthText.text = $"Health: {player.currentHealth:F0}/{player.GetMaxHealth():F0}";

        if (attackDamageText != null)
            attackDamageText.text = $"Attack: {player.GetCurrentAttackDamage():F1}";

        if (currencyText != null)
            currencyText.text = $"Gold: {player.currency}";
    }

    void UpdateDetailedStats()
    {
        // Additional detailed stats
        if (attackSpeedText != null)
            attackSpeedText.text = $"Attack Speed: {player.GetCurrentAttackSpeed():F2}";

        if (armorText != null)
            armorText.text = $"Armor: {player.GetCurrentArmor():F1}";

        if (critChanceText != null)
            critChanceText.text = $"Crit Chance: {player.critChance.GetValue():F1}%";

        if (critDamageText != null)
            critDamageText.text = $"Crit Damage: {player.critDamage.GetValue():F1}x";

        if (energyText != null)
            energyText.text = $"Energy: {player.energy:F1}";
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercentage = player.currentHealth / player.GetMaxHealth();
            healthBar.value = healthPercentage;
        }

        if (healthBarText != null)
        {
            healthBarText.text = $"{player.currentHealth:F0}/{player.GetMaxHealth():F0}";
        }
    }

    void UpdateEnergyBar()
    {
        if (energyBar != null && player.energy > 0)
        {
            // Assuming max energy for display purposes - you might want to add a maxEnergy field
            float maxEnergy = 100f; // Adjust this based on your game design
            energyBar.value = player.energy / maxEnergy;
            energyBar.gameObject.SetActive(true);
        }
        else if (energyBar != null)
        {
            energyBar.gameObject.SetActive(false);
        }

        if (energyBarText != null)
        {
            energyBarText.text = $"Energy: {player.energy:F1}";
        }
    }

    void UpdateUpgradeCosts()
    {
        // Update upgrade cost displays with color coding
        if (speedCostText != null)
        {
            speedCostText.text = $"Upgrade Speed: {player.moveSpeedCost}g";
            speedCostText.color = player.currency >= player.moveSpeedCost ? canAffordColor : cantAffordColor;
        }

        if (attackCostText != null)
        {
            attackCostText.text = $"Upgrade Attack: {player.attackDamageCost}g";
            attackCostText.color = player.currency >= player.attackDamageCost ? canAffordColor : cantAffordColor;
        }

        if (attackSpeedCostText != null)
        {
            attackSpeedCostText.text = $"Upgrade A.Speed: {player.attackSpeedCost}g";
            attackSpeedCostText.color = player.currency >= player.attackSpeedCost ? canAffordColor : cantAffordColor;
        }

        if (healthCostText != null)
        {
            healthCostText.text = $"Upgrade Health: {player.healthCost}g";
            healthCostText.color = player.currency >= player.healthCost ? canAffordColor : cantAffordColor;
        }

        if (armorCostText != null)
        {
            armorCostText.text = $"Upgrade Armor: {player.armorCost}g";
            armorCostText.color = player.currency >= player.armorCost ? canAffordColor : cantAffordColor;
        }
    }

    // Public methods for external calls
    public void SetShowDetailedStats(bool show)
    {
        showDetailedStats = show;
    }

    public void SetShowUpgradeCosts(bool show)
    {
        showUpgradeCosts = show;
    }

    // Method to flash health bar when taking damage
    public void FlashHealthBar()
    {
        if (healthBar != null)
            StartCoroutine(FlashBarCoroutine(healthBar));
    }

    // Method to flash energy bar when using energy
    public void FlashEnergyBar()
    {
        if (energyBar != null)
            StartCoroutine(FlashBarCoroutine(energyBar));
    }

    System.Collections.IEnumerator FlashBarCoroutine(Slider bar)
    {
        Color originalColor = bar.fillRect.GetComponent<UnityEngine.UI.Image>().color;
        bar.fillRect.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        bar.fillRect.GetComponent<UnityEngine.UI.Image>().color = originalColor;
    }

    // Debug method to validate all UI references
    void Start()
    {
        ValidateUIReferences();
    }

    void ValidateUIReferences()
    {
        if (player == null)
        {
            Debug.LogWarning("StatUI: Player reference is not assigned!");
            return;
        }

        int missingRefs = 0;
        
        if (speedText == null) { Debug.LogWarning("StatUI: speedText is not assigned"); missingRefs++; }
        if (healthText == null) { Debug.LogWarning("StatUI: healthText is not assigned"); missingRefs++; }
        if (attackDamageText == null) { Debug.LogWarning("StatUI: attackDamageText is not assigned"); missingRefs++; }
        if (currencyText == null) { Debug.LogWarning("StatUI: currencyText is not assigned"); missingRefs++; }

        if (missingRefs > 0)
        {
            Debug.LogWarning($"StatUI: {missingRefs} UI references are missing. The script will still work but some stats won't be displayed.");
        }
        else
        {
            Debug.Log("StatUI: All essential UI references are properly assigned!");
        }
    }
}
