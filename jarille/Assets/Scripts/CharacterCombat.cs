using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCombat : MonoBehaviour
{
    public string characterName;
    public int maxHealth = 50;
    public int currentHealth;

    public GameObject damageNumberPrefab;
    public Transform damageSpawnPoint;

    [Header("UI")]
    public Slider healthBar;
    public GameObject highlight;

    [Header("Damage Settings")]
    public int normalMinDamage = 4;
    public int normalMaxDamage = 7;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void NormalAttack()
    {
        Debug.Log("NormalAttack pressed for " + characterName);

        int damage = Random.Range(normalMinDamage, normalMaxDamage + 1);

        CombatManager.Instance.enemy.TakeDamage(damage);
        CombatManager.Instance.GainNeo(1);

        CombatManager.Instance.CharacterFinishedTurn();
    }

    public void ElementAttack()
    {
        if (CombatManager.Instance.neo < 2)
        {
            Debug.Log("Not enough NEO!");
            return;
        }

        
        CombatManager.Instance.GainNeo(-2);

        int damage;

        
        if (Random.value <= 0.9f)
        {
            damage = normalMaxDamage;
        }
        else
        {
            damage = 1;
        }

        CombatManager.Instance.enemy.TakeDamage(damage);

        CombatManager.Instance.CharacterFinishedTurn();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();
        Debug.Log(characterName + " HP: " + currentHealth);

        UIShake.Instance.Shake(18f, 0.15f);

        if (damageNumberPrefab != null && damageSpawnPoint != null)
        {
            GameObject dmg = Instantiate(damageNumberPrefab, damageSpawnPoint.position, Quaternion.identity, Object.FindFirstObjectByType<Canvas>().transform);
            dmg.GetComponent<DamageNumber>().SetDamage(damage);
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }

    
    public void SetHighlight(bool isActive)
    {
        if (highlight != null)
        {
            if (!IsAlive())
            {
                
                highlight.SetActive(true);
                var img = highlight.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                    img.color = Color.red; // 
            }
            else
            {
                highlight.SetActive(isActive);
                var img = highlight.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                    img.color = Color.yellow; 
            }
        }
    }
    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}