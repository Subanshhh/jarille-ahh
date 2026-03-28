using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ContinueGame()
    {
        int key = PlayerPrefs.GetInt("RoomKey", 0);

        string sceneToLoad = GetSceneFromKey(key);

        SceneManager.LoadScene(sceneToLoad);
    }

    string GetSceneFromKey(int key)
    {
        switch (key)
        {
            case 1: return "Room1";
            case 2: return "Room2";
            case 3: return "Room3";
            default: return "Room1"; // fallback
        }
    }
}