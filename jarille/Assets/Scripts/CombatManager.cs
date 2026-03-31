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

    public EnemyAI currentEnemy; 

    public bool isInCombat = false;


    void Awake()
    {
        Instance = this;
    }

    
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
    
    public void CharacterFinishedTurn()
    {
       
        if (currentCharacter < party.Count)
            party[currentCharacter].SetHighlight(false);

        
        do
        {
            currentCharacter++;
        } while (currentCharacter < party.Count && !party[currentCharacter].IsAlive());

        if (currentCharacter >= party.Count)
        {
            
            StartCoroutine(EnemyTurn());
        }
        else
        {
            
            party[currentCharacter].SetHighlight(true);
            Debug.Log("Next character: " + GetCurrentCharacter()?.characterName);
        }
    }

    IEnumerator EnemyTurn()
    {
        
        foreach (var c in party)
            c.SetHighlight(false);

        
        bool anyAlive = false;
        foreach (var c in party)
            if (c.IsAlive()) anyAlive = true;

        if (!anyAlive)
        {
            Debug.Log("All characters dead.");

            StartCoroutine(HandleDeath());

            yield break;
        }

        
        enemy.Attack(party);

        yield return new WaitForSeconds(1f);

        
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

        yield return new WaitForSeconds(2f); 

        SceneManager.LoadScene(firstSceneName);
    }
    public void GainNeo(int amount)
    {
        neo += amount;
        neo = Mathf.Clamp(neo, 0, maxNeo);

        UpdateNeoUI();

        Debug.Log("NEO: " + neo);
    }

    
    public void StartCombat(EnemyAI enemyRef, EnemyCombat enemyData)
    {
        if (isInCombat) return;

        isInCombat = true;

        currentEnemy = enemyRef;

        
        enemy = enemyData;

       
        if (enemy != null)
        {
            enemy.ResetEnemy(); 
        }

        AudioManager.Instance.PlayCombat();

        neo = 0;
        UpdateNeoUI();
        combatPanel.SetActive(true);
        currentCharacter = 0;

        foreach (var c in party)
            c.SetHighlight(false);

        if (party.Count > 0)
            party[0].SetHighlight(true);

        Debug.Log("Combat Started with " + enemyRef.name);
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

        combatPanel.SetActive(false);
        currentCharacter = 0;
        neo = 0;

        
        if (currentEnemy != null)
        {
            currentEnemy.gameObject.SetActive(false);
        }

        
        currentEnemy = null;
        isInCombat = false;

        Debug.Log("Combat Ended");
    }
}