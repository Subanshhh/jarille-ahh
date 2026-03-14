using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyCombat : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth = 50;

    [Header("UI")]
    public Slider healthBar; // assign this in the inspector

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
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
}