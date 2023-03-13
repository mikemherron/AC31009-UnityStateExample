using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Now the HealthBar component only relies on a health
    // controller rather than a Playercontroller. This means
    // we can use the same HealthBar component on any game
    // object that has a HealthController component!
    public HealthController healthController;

    public Slider slider;
    
    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = healthController.maxHealth;
        slider.value = healthController.health;
        healthController.onHealthUpdated.AddListener(UpdateHealth);
    }

    void UpdateHealth(int health)
    {
        slider.value = health;
    }
}
