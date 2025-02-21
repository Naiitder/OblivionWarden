using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider easeHealthSlider;
    float lerpSpeed = 2f;
    private float delayBeforeLerp = 0.5f;

    Coroutine easeHealthCoroutine;

    
    public override void Awake()
    {
        base.Awake();
        UpdateHealthBarsMaximums();

    }

    public void UpdateHealthBarsMaximums()
    {
        healthSlider.maxValue = maxHealth;
        easeHealthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        easeHealthSlider.value = currentHealth;
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


}
