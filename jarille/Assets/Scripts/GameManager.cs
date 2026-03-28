using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentRoomKey = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetRoomKey(int key)
    {
        if (key > currentRoomKey)
        {
            currentRoomKey = key;
            PlayerPrefs.SetInt("RoomKey", currentRoomKey);
            PlayerPrefs.Save();

            Debug.Log("Saved Room Key: " + currentRoomKey);
        }
    }

    void LoadProgress()
    {
        currentRoomKey = PlayerPrefs.GetInt("RoomKey", 0);
        Debug.Log("Loaded Room Key: " + currentRoomKey);
    }
}