using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    public int strength = 0;
    public int armor = 0;
    public int speed = 0;
    public int food = 0;

    public Text strengthText;
    public Text armorText;
    public Text speedText;
    public Text foodText;

    void Start()
    {
        UpdateUI();
    }

    public void IncreaseStrength()
    {
        strength++;
        UpdateUI();
    }

    public void IncreaseArmor()
    {
        armor++;
        UpdateUI();
    }

    public void IncreasePotion()
    {
        speed++;
        UpdateUI();
    }

    public void IncreaseFood()
    {
        food++;
        UpdateUI();
    }

    void UpdateUI()
    {
        strengthText.text = "Strength: " + strength;
        armorText.text = "Armor: " + armor;
        speedText.text = "speed: " + speed;
        foodText.text = "Food: " + food;
    }
}
