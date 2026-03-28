using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ContinueGame()
    {
        if (!PlayerPrefs.HasKey("RoomKey"))
        {
            SceneManager.LoadScene("Room1"); // first room
            return;
        }

        int key = PlayerPrefs.GetInt("RoomKey");
        SceneManager.LoadScene(key);
    }

    //string GetSceneFromKey(int key)
    //{
      //  switch (key)
      //  {
      ///      case 1: return "Room1";
     //       case 2: return "Room2";
     //       case 3: return "Room3";
     //       default: return "Room1"; // fallback
   //     }
  //  }
}