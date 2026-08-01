using UnityEngine;
using UnityEngine.UI;

public class HealthUIDisplay : MonoBehaviour
{
    public Slider healthSlider;

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}
