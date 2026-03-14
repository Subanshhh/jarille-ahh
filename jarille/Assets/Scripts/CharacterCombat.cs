using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCombat : MonoBehaviour
{
    public string characterName;
    public int maxHealth = 50;
    public int currentHealth;
    

    [Header("UI")]
    public Slider healthBar;
    public GameObject highlight;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void NormalAttack()
    {
        Debug.Log("NormalAttack pressed for " + characterName);
        CombatManager.Instance.enemy.TakeDamage(5);
        CombatManager.Instance.GainNeo(1);
        CombatManager.Instance.CharacterFinishedTurn();
    }

    public void ElementAttack()
    {
        // Check if enough NEO
        if (CombatManager.Instance.neo < 2)
        {
            Debug.Log("Not enough NEO!");
            return;
        }

        // Spend NEO
        CombatManager.Instance.GainNeo(-2);

        // Deal element damage (example)
        CombatManager.Instance.enemy.TakeDamage(8);

        // End turn
        CombatManager.Instance.CharacterFinishedTurn();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();
        Debug.Log(characterName + " HP: " + currentHealth);
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }

    // Turn highlight
    public void SetHighlight(bool isActive)
    {
        if (highlight != null)
        {
            if (!IsAlive())
            {
                // Max out the highlight color (e.g., red overlay) to show dead
                highlight.SetActive(true);
                var img = highlight.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                    img.color = Color.red; // fully red when dead
            }
            else
            {
                highlight.SetActive(isActive);
                var img = highlight.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                    img.color = Color.yellow; // normal turn highlight color
            }
        }
    }
    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}