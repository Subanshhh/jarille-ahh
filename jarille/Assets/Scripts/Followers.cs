using UnityEngine;

public class Follower : MonoBehaviour
{
    public PlayerMovement leader;
    public int followIndex = 10; 

    void Update()
    {
        if (leader.positionHistory.Count > followIndex)
        {
            transform.position = leader.positionHistory[followIndex];
        }
    }
}