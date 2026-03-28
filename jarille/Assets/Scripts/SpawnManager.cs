using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;

    void Start()
    {
        int key = GameManager.Instance.currentRoomKey;

        if (key < spawnPoints.Length)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoints[key].position;
        }
    }
}