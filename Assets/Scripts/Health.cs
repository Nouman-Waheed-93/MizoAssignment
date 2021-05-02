using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent onDamageTaken;
    private float totalHealth = 100;
    private float currHealth;

    public float HealthRatio { get { return currHealth / totalHealth; } }

    private void Awake()
    {
        currHealth = totalHealth;
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)
        {
            Destroy(gameObject);   
        }
        else
        {
            onDamageTaken?.Invoke();
        }
    }
}
