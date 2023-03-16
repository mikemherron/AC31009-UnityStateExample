using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HealthUpdatedEvent : UnityEvent<int> { }

public class HealthController : MonoBehaviour
{
    
    public int maxHealth;
    public int health;
    public HealthUpdatedEvent onHealthUpdated;

    void Start()
    {
        health = maxHealth;
        if (onHealthUpdated == null) {
            onHealthUpdated = new HealthUpdatedEvent();
        }    
    }

    public void TakeDamage() {
        health--;
        onHealthUpdated.Invoke(health);
    }
}
