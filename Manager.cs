using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUIManager : MonoBehaviour
{
    [Header("Player Reference")]
    public PlayerStats playerStats;
    
    [Header("Upgrade Buttons")]
    public Button moveSpeedButton;
    public Button attackDamageButton;
    public Button attackSpeedButton;
    public Button healthButton;
    
    [Header("Stat Display Text")]
    public TextMeshProUGUI moveSpeedText;
    public TextMeshProUGUI attackDamageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI healthText;
    
    [Header("Cost Display Text")]
    public TextMeshProUGUI moveSpeedCostText;
    public TextMeshProUGUI attackDamageCostText;
    public TextMeshProUGUI attackSpeedCostText;
    public TextMeshProUGUI healthCostText;
    
    [Header("Currency Display")]
    public TextMeshProUGUI currencyText;
    
    void Start()
    {
        // Find PlayerStats if not assigned
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();
        
        // Set up button listeners
        if (moveSpeedButton) moveSpeedButton.onClick.AddListener(UpgradeMoveSpeed);
        if (attackDamageButton) attackDamageButton.onClick.AddListener(UpgradeAttackDamage);
        if (attackSpeedButton) attackSpeedButton.onClick.AddListener(UpgradeAttackSpeed);
        if (healthButton) healthButton.onClick.AddListener(UpgradeHealth);
        
        // Initial UI update
        UpdateUI();
    }
    
    void Update()
    {
        // Update UI every frame to show real-time changes
        UpdateUI();
    }
    
    public void UpgradeMoveSpeed()
    {
        if (playerStats.UpgradeMoveSpeed())
        {
            // Play upgrade sound effect here if you have one
            Debug.Log("Move Speed Upgraded!");
        }
        else
        {
            Debug.Log("Not enough currency for Move Speed upgrade!");
        }
    }
    
    public void UpgradeAttackDamage()
    {
        if (playerStats.UpgradeAttackDamage())
        {
            Debug.Log("Attack Damage Upgraded!");
        }
        else
        {
            Debug.Log("Not enough currency for Attack Damage upgrade!");
        }
    }
    
    public void UpgradeAttackSpeed()
    {
        if (playerStats.UpgradeAttackSpeed())
        {
            Debug.Log("Attack Speed Upgraded!");
        }
        else
        {
            Debug.Log("Not enough currency for Attack Speed upgrade!");
        }
    }
    
    public void UpgradeHealth()
    {
        if (playerStats.UpgradeHealth())
        {
            Debug.Log("Health Upgraded!");
        }
        else
        {
            Debug.Log("Not enough currency for Health upgrade!");
        }
    }
    
    void UpdateUI()
    {
        if (playerStats == null) return;
        
        // Update currency display
        if (currencyText)
            currencyText.text = $"Gold: {playerStats.currency}";
        
        // Update stat values
        if (moveSpeedText)
            moveSpeedText.text = $"Speed: {playerStats.GetCurrentMoveSpeed():F1}";
        if (attackDamageText)
            attackDamageText.text = $"Damage: {playerStats.GetCurrentAttackDamage():F0}";
        if (attackSpeedText)
            attackSpeedText.text = $"Attack Speed: {playerStats.GetCurrentAttackSpeed():F1}";
        if (healthText)
            healthText.text = $"Health: {playerStats.GetCurrentHealth():F0}";
        
        // Update cost displays
        if (moveSpeedCostText)
            moveSpeedCostText.text = $"Cost: {playerStats.moveSpeedCost}";
        if (attackDamageCostText)
            attackDamageCostText.text = $"Cost: {playerStats.attackDamageCost}";
        if (attackSpeedCostText)
            attackSpeedCostText.text = $"Cost: {playerStats.attackSpeedCost}";
        if (healthCostText)
            healthCostText.text = $"Cost: {playerStats.healthCost}";
        
        // Update button interactability based on currency
        if (moveSpeedButton)
            moveSpeedButton.interactable = playerStats.currency >= playerStats.moveSpeedCost;
        if (attackDamageButton)
            attackDamageButton.interactable = playerStats.currency >= playerStats.attackDamageCost;
        if (attackSpeedButton)
            attackSpeedButton.interactable = playerStats.currency >= playerStats.attackSpeedCost;
        if (healthButton)
            healthButton.interactable = playerStats.currency >= playerStats.healthCost;
    }
}
