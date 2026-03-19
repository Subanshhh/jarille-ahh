using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoader : MonoBehaviour
{
    void Start()
    {
        Load();
    }

    public void Load()
    {
        SaveData data = SaveSystem.LoadGame();

        if (data == null) return;

        // Move player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(data.playerX, data.playerY, 0);

        // Restore NEO
        CombatManager.Instance.neo = data.neo;

        // Restore HP
        for (int i = 0; i < CombatManager.Instance.party.Count; i++)
        {
            CombatManager.Instance.party[i].currentHealth = data.partyHealth[i];
        }
    }
}