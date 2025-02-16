using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider easeHealthSlider;
    float lerpSpeed = 2f;
    private float delayBeforeLerp = 0.5f;

    SkinnedMeshRenderer meshRenderer;

    Coroutine easeHealthCoroutine;
    private Coroutine damageFlashCoroutine;
    
    public override void Awake()
    {
        base.Awake();  
        healthSlider.maxValue = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        easeHealthSlider.value = currentHealth;
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        healthSlider.value = currentHealth;
        if (easeHealthCoroutine != null)
        {
            StopCoroutine(easeHealthCoroutine);
        }

        easeHealthCoroutine = StartCoroutine(nameof(UpdateEaseHealth));


        if (damageFlashCoroutine != null)
            StopCoroutine(nameof(DamageFlashRoutine));

        damageFlashCoroutine = StartCoroutine(nameof(DamageFlashRoutine));
    }

    IEnumerator UpdateEaseHealth()
    {

        yield return new WaitForSeconds(delayBeforeLerp);

        float elapsedTime = 0f;
        float duration = 0.5f; 

        float startValue = easeHealthSlider.value;
        float targetValue = CurrentHealth;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * lerpSpeed;
            easeHealthSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        easeHealthSlider.value = targetValue; 
    }

    private IEnumerator DamageFlashRoutine()
    {
        meshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(1f);

        meshRenderer.material.color = Color.white;

        damageFlashCoroutine = null;
    }
}
