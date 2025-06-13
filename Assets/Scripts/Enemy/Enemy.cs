using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHP = 100f;
    private float currentHP;

    public float damageAmount = 10f;
    public float damageAmountModifier = 1.00f;
    public int souldrop = 1;

    [Header("UI")]
    //[SerializeField] private Slider healthSlider;

    [Header("Other")]
    private PlayerStats stats;

    private bool canDamage = false;

    private System.Collections.IEnumerator Start()
    {
        currentHP = maxHP;
        //UpdateHealthUI();
        damageAmount *= damageAmountModifier;
        stats = FindObjectOfType<PlayerStats>();

        // Wait before enemy can damage player to avoid instant hits on spawn
        yield return new WaitForSeconds(1f);
        canDamage = true;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        //UpdateHealthUI();

        Debug.Log($"{gameObject.name} took {damage} damage, current HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    /*private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHP / maxHP;
        }
    }*/

    private void Die()
    {
        Debug.Log(gameObject.name + " has been defeated!");

        if (stats != null)
        {
            stats.TryAddSouls(souldrop);
        }

        /*if (healthSlider != null)
        {
            Destroy(healthSlider.gameObject);
        }*/

        Destroy(gameObject);
    }

    public float GetDamageAmount()
    {
        return damageAmount;
    }

    public bool GetCanDamage()
    {
        return canDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
