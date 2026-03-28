using UnityEngine;

public class RoomKeyTrigger : MonoBehaviour
{
    public int roomKey;

    void Start()
    {
        GameManager.Instance.SetRoomKey(roomKey);
    }
}