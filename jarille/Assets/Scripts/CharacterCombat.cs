using UnityEngine;
using System.Collections;

public class CharacterCombat : MonoBehaviour
{
    public string characterName;
    public int attackPower = 5;

    public void NormalAttack()
    {
        StartCoroutine(AttackRoutine(false));
    }

    public void ElementAttack()
    {
        StartCoroutine(AttackRoutine(true));
    }

    IEnumerator AttackRoutine(bool elemental)
    {
        Debug.Log(characterName + " attacks!");

        yield return new WaitForSeconds(0.3f);

        if (elemental)
        {
            CombatManager.Instance.enemy.TakeDamage(attackPower + 2);
        }
        else
        {
            CombatManager.Instance.enemy.TakeDamage(attackPower);
            CombatManager.Instance.GainNeo(1);
        }

        yield return new WaitForSeconds(0.2f);

        CombatManager.Instance.CharacterFinishedTurn();
    }
}