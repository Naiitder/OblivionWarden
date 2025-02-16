using System;
using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]  protected int maxHealth, currentHealth;
    bool isDead;
    
    [SerializeField] int dmg;

    public int MaxHealth { get {return maxHealth; } set {maxHealth = value; } }
    public int CurrentHealth { get {return currentHealth; } set {currentHealth = value; } }
    public int Dmg { get {return dmg; } set {dmg = value; } }
    public bool IsDead { get {return isDead; } set {isDead = value; } }

    public virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public Action OnDeath { get; set; }

    public virtual void TakeDamage(int dmg)
    {
        CurrentHealth -= dmg;

       
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();
        }

    }

   
}
