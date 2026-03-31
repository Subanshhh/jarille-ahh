using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyCombat : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth = 50;

    public GameObject damageNumberPrefab;
    public Transform damageSpawnPoint;

    [Header("UI")]
    public Slider healthBar; // assign this in the inspector

    void Awake()
    {
        currentHP = maxHP;
    }

    // Attack all alive characters with random damage
    public void Attack(List<CharacterCombat> party)
    {
        foreach (var character in party)
        {
            if (character.IsAlive())
            {
                int damage = Random.Range(1, 26); // 1-15
                character.TakeDamage(damage);
                Debug.Log(name + " attacked " + character.characterName + " for " + damage + " damage");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(name + " HP: " + currentHealth);

        UpdateHealthUI(); // update slider whenever damaged

        if (damageNumberPrefab != null && damageSpawnPoint != null)
        {
            GameObject dmg = Instantiate(damageNumberPrefab, damageSpawnPoint.position, Quaternion.identity, Object.FindFirstObjectByType<Canvas>().transform);
            dmg.GetComponent<DamageNumber>().SetDamage(damage);
        }

        UIShake.Instance.Shake(05f, 0.15f);

        if (currentHealth <= 0)
        {
            CombatManager.Instance.EndCombat();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }
    public int maxHP = 20;
    private int currentHP;

    

    public void ResetEnemy()
    {
        currentHP = maxHP;
    }
}