using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    void Awake()
    {
        Instance = this;
    }

    // ✅ This is the function the buttons call
    public CharacterCombat GetCurrentCharacter()
    {
        if (party.Count == 0) return null;
        return party[currentCharacter];
    }

    // Called by CharacterCombat after each turn
    public void CharacterFinishedTurn()
    {
        currentCharacter++;

        if (currentCharacter >= party.Count)
        {
            StartCoroutine(EnemyTurn());
        }
        else
        {
            Debug.Log("Next character: " + GetCurrentCharacter().characterName);
        }
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");

        yield return new WaitForSeconds(0.5f);

        enemy.Attack(party);

        yield return new WaitForSeconds(1f);

        currentCharacter = 0;

        Debug.Log("New Player Turn");
    }

    public void GainNeo(int amount)
    {
        neo += amount;
        neo = Mathf.Clamp(neo, 0, maxNeo);

        Debug.Log("NEO: " + neo);
    }

    // Called when player collides with enemy
    public void StartCombat()
    {
        combatPanel.SetActive(true);
        currentCharacter = 0;

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
}