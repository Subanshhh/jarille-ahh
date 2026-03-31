using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Party")]
    public List<CharacterCombat> party;

    [Header("Enemy")]
    public EnemyCombat enemy;

    [Header("Combat Panel UI")]
    public GameObject combatPanel;

    [Header("Turn System")]
    public int currentCharacter = 0;

    [Header("NEO Resource")]
    public int neo = 0;
    public int maxNeo = 10;
    public TMP_Text neoText;

    [Header("Death UI")]
    public GameObject youDiedPanel;
    public string firstSceneName = "TestScene";

    public EnemyAI currentEnemy; // 👈 add this



    void Awake()
    {
        Instance = this;
    }

    // ✅ This is the function the buttons call
    public CharacterCombat GetCurrentCharacter()
    {
        if (party == null || party.Count == 0)
            return null;

        if (currentCharacter < 0 || currentCharacter >= party.Count)
            return null;

        return party[currentCharacter];
    }

    void UpdateNeoUI()
    {
        if (neoText != null)
            neoText.text = "NEO: " + neo + " / " + maxNeo;
    }
    // Called by CharacterCombat after each turn
    public void CharacterFinishedTurn()
    {
        // Turn off previous highlight if in range
        if (currentCharacter < party.Count)
            party[currentCharacter].SetHighlight(false);

        // Move to next alive character
        do
        {
            currentCharacter++;
        } while (currentCharacter < party.Count && !party[currentCharacter].IsAlive());

        if (currentCharacter >= party.Count)
        {
            // All characters done, start enemy turn
            StartCoroutine(EnemyTurn());
        }
        else
        {
            // Turn on next character highlight
            party[currentCharacter].SetHighlight(true);
            Debug.Log("Next character: " + GetCurrentCharacter()?.characterName);
        }
    }

    IEnumerator EnemyTurn()
    {
        // Turn off all highlights
        foreach (var c in party)
            c.SetHighlight(false);

        // Check if all characters are dead
        bool anyAlive = false;
        foreach (var c in party)
            if (c.IsAlive()) anyAlive = true;

        if (!anyAlive)
        {
            Debug.Log("All characters dead.");

            StartCoroutine(HandleDeath());

            yield break;
        }

        // Enemy attacks
        enemy.Attack(party);

        yield return new WaitForSeconds(1f);

        // Reset to first alive character
        currentCharacter = 0;
        while (currentCharacter < party.Count && !party[currentCharacter].IsAlive())
            currentCharacter++;

        if (currentCharacter < party.Count)
        {
            party[currentCharacter].SetHighlight(true);
            Debug.Log("New Player Turn: " + GetCurrentCharacter()?.characterName);
        }
        else
        {
            // Everyone dead after enemy attack
            Debug.Log("All characters died during enemy attack");
            StartCoroutine(HandleDeath());

            yield break;
        }
    }

    IEnumerator HandleDeath()
    {
        combatPanel.SetActive(false);

        if (youDiedPanel != null)
            youDiedPanel.SetActive(true);

        yield return new WaitForSeconds(2f); // dramatic pause 😤

        SceneManager.LoadScene(firstSceneName);
    }
    public void GainNeo(int amount)
    {
        neo += amount;
        neo = Mathf.Clamp(neo, 0, maxNeo);

        UpdateNeoUI();

        Debug.Log("NEO: " + neo);
    }

    // Called when player collides with enemy
    public void StartCombat(EnemyAI enemyRef)
    {
        currentEnemy = enemyRef; // 👈 store WHO started combat

        AudioManager.Instance.PlayCombat();

        neo = 0;
        UpdateNeoUI();
        combatPanel.SetActive(true);
        currentCharacter = 0;

        foreach (var c in party)
            c.SetHighlight(false);

        if (party.Count > 0)
            party[0].SetHighlight(true);

        Debug.Log("Combat Started");
    }
    public void CurrentCharacterNormalAttack()
    {
        CharacterCombat c = GetCurrentCharacter();
        if (c != null)
            c.NormalAttack();
    }

    public void CurrentCharacterElementAttack()
    {
        CharacterCombat c = GetCurrentCharacter();
        if (c != null)
            c.ElementAttack();
    }
    public void EndCombat()
    {
        AudioManager.Instance.PlayRoom();
        combatPanel.SetActive(false); // hide the combat UI
        currentCharacter = 0;          // reset turn
        neo = 0;                       // reset NEO or keep it depending on your design

        // If you want the enemy to respawn in the overworld later, re-enable it:
        if (currentEnemy != null)
        {
            currentEnemy.gameObject.SetActive(false); // only THIS enemy dies
        }

        Debug.Log("Combat Ended");
    }
}