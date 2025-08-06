// Create: FloatingHealthBar.cs
using UnityEngine;
using TMPro;

public class FloatingHealthBar : MonoBehaviour
{
    [Header("Health Bar Settings")]
    public Color healthColor = Color.red;
    public Color lowHealthColor = new Color(1f, 0.3f, 0.3f); // Darker red
    public float lowHealthThreshold = 0.25f; // 25% health
    
    [Header("Position Settings")]
    public Vector3 offset = new Vector3(0f, 1.5f, 0f);
    public bool followPlayer = true;
    
    [Header("Damage Effect Settings")]
    public float damageFlashDuration = 0.3f;
    public Color damageFlashColor = Color.white;
    public float punchScale = 1.2f;
    public float punchDuration = 0.2f;
    
    private PlayerStats playerStats;
    private Transform playerTransform;
    private TextMeshPro healthText;
    private Camera mainCamera;
    private Vector3 originalScale;
    private bool isFlashing = false;
    
    void Start()
    {
        SetupHealthBar();
    }
    
    void SetupHealthBar()
    {
        // Find player
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
            playerTransform = playerStats.transform;
        
        // Get main camera
        mainCamera = Camera.main;
        
        // Create or get TextMeshPro component
        healthText = GetComponent<TextMeshPro>();
        if (healthText == null)
        {
            healthText = gameObject.AddComponent<TextMeshPro>();
        }
        
        // Configure text
        healthText.text = "100/100";
        healthText.color = healthColor;
        healthText.fontSize = 8;
        healthText.fontStyle = FontStyles.Bold;
        healthText.alignment = TextAlignmentOptions.Center;
        
        // Store original scale
        originalScale = transform.localScale;
        
        // Subscribe to health changes if we add an event system later
        UpdateHealthDisplay();
    }
    
    void Update()
    {
        if (playerStats == null || playerTransform == null) return;
        
        // Follow player
        if (followPlayer)
        {
            transform.position = playerTransform.position + offset;
        }
        
        // Always face camera
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                            mainCamera.transform.rotation * Vector3.up);
        }
        
        // Update health display
        UpdateHealthDisplay();
    }
    
    void UpdateHealthDisplay()
    {
        if (playerStats == null) return;
        
        float currentHealth = playerStats.currentHealth;
        float maxHealth = playerStats.GetMaxHealth();
        
        // Update text
        healthText.text = $"{currentHealth:F0}/{maxHealth:F0}";
        
        // Update color based on health percentage
        float healthPercentage = currentHealth / maxHealth;
        if (healthPercentage <= lowHealthThreshold)
        {
            healthText.color = lowHealthColor;
        }
        else if (!isFlashing)
        {
            healthText.color = healthColor;
        }
    }
    
    public void OnPlayerTakeDamage(float damageAmount)
    {
        // Trigger damage effects
        StartCoroutine(DamageEffect());
        
        // Optional: Show floating damage number
        ShowFloatingDamage(damageAmount);
    }
    
    System.Collections.IEnumerator DamageEffect()
    {
        isFlashing = true;
        
        // Flash effect
        Color originalColor = healthText.color;
        healthText.color = damageFlashColor;
        
        // Punch scale effect
        transform.localScale = originalScale * punchScale;
        
        // Wait for flash duration
        yield return new WaitForSeconds(damageFlashDuration);
        
        // Return to normal
        healthText.color = originalColor;
        
        // Smooth scale back to original
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        
        while (elapsedTime < punchDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / punchDuration;
            transform.localScale = Vector3.Lerp(startScale, originalScale, t);
            yield return null;
        }
        
        transform.localScale = originalScale;
        isFlashing = false;
    }
    
    void ShowFloatingDamage(float damage)
    {
        // Create floating damage text
        GameObject damageText = new GameObject("FloatingDamage");
        damageText.transform.position = transform.position + Vector3.up * 0.5f;
        
        TextMeshPro damageDisplay = damageText.AddComponent<TextMeshPro>();
        damageDisplay.text = $"-{damage:F0}";
        damageDisplay.color = Color.red;
        damageDisplay.fontSize = 6;
        damageDisplay.fontStyle = FontStyles.Bold;
        damageDisplay.alignment = TextAlignmentOptions.Center;
        
        // Animate the floating damage
        StartCoroutine(AnimateFloatingDamage(damageText));
    }
    
    System.Collections.IEnumerator AnimateFloatingDamage(GameObject damageText)
    {
        Vector3 startPos = damageText.transform.position;
        Vector3 endPos = startPos + Vector3.up * 2f;
        
        TextMeshPro textComponent = damageText.GetComponent<TextMeshPro>();
        Color startColor = textComponent.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        float duration = 1.5f;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // Move up
            damageText.transform.position = Vector3.Lerp(startPos, endPos, t);
            
            // Fade out
            textComponent.color = Color.Lerp(startColor, endColor, t);
            
            yield return null;
        }
        
        Destroy(damageText);
    }
}
