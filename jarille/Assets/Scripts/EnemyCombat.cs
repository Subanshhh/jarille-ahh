using UnityEngine;
using System.Collections.Generic;

public class EnemyCombat : MonoBehaviour
{
    public int health = 40;
    public int attackPower = 4;

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Enemy HP: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Attack(List<CharacterCombat> party)
    {
        int target = Random.Range(0, party.Count);

        Debug.Log("Enemy attacks " + party[target].characterName);
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");
    }
}