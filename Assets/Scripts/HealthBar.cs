using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    
  public HealthController healthController;
  public Slider slider;

  void Start()
  {
    slider.minValue = 0;
    slider.maxValue = healthController.maxHealth;
    slider.value = healthController.maxHealth;
    healthController.onHealthUpdated.AddListener(UpdateHealth);
  }

  void UpdateHealth(int health)
  {
    slider.value = health;
  }
}
