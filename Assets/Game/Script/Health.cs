using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth;

    public int CurrentHealth;

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
    }
}
