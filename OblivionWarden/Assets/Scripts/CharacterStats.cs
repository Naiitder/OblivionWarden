using System;
using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]  protected int maxHealth, currentHealth;
    bool isDead;
    
    [SerializeField] int dmg;

    private Coroutine damageFlashCoroutine;
    SkinnedMeshRenderer meshRenderer;

    public int MaxHealth { get {return maxHealth; } set {maxHealth = value; } }
    public int CurrentHealth { get {return currentHealth; } set {currentHealth = value; } }
    public int Dmg { get {return dmg; } set {dmg = value; } }
    public bool IsDead { get {return isDead; } set {isDead = value; } }

    public virtual void Awake()
    {
        currentHealth = maxHealth;
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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

        if (damageFlashCoroutine != null)
            StopCoroutine(nameof(DamageFlashRoutine));

        damageFlashCoroutine = StartCoroutine(nameof(DamageFlashRoutine));

    }

    private IEnumerator DamageFlashRoutine()
    {
        meshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(1f);

        meshRenderer.material.color = Color.white;

        damageFlashCoroutine = null;
    }


}
