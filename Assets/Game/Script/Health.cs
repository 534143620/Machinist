using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth;

    public int CurrentHealth;

    public float CurrentHealthPercentage{
        get{
            return (float)CurrentHealth  / (float) MaxHealth;
        }
    }

    private Character _cc;
    private void Awake() {
        CurrentHealth = MaxHealth;
        _cc = GetComponent<Character>();
    }

    public void ApplyDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log(gameObject.name + "took damage" + damage);
        Debug.Log(gameObject.name + "currentHealth" + CurrentHealth);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(CurrentHealth <= 0)
           _cc.SwitchStateTo(Character.CharacterState.Dead);
    }

    public void AddHealth(int health)
    {
        CurrentHealth += health;
        if(CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }
}
