using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void SaveGame()
    {
        SaveData data = new SaveData();

        // Player position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        data.playerX = player.transform.position.x;
        data.playerY = player.transform.position.y;

        // Scene
        data.sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // NEO
        data.neo = CombatManager.Instance.neo;

        // Party HP
        var party = CombatManager.Instance.party;
        data.partyHealth = new int[party.Count];

        for (int i = 0; i < party.Count; i++)
        {
            data.partyHealth[i] = party[i].currentHealth;
        }

        // Story (simple for now)
        data.storyProgress = 0;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Game Saved");
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(path))
        {
            Debug.Log("No save file found");
            return null;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        return data;
    }
}