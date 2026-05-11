using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
public class GameData : ScriptableObject
{
    [Header("Player")]
    public Vector3 playerPosition;
    public float playerSpeedMultiplier = 1f;
    [Header("Enemies")]
    public float enemySpeedMultiplier = 1f;
    [Header("Collectibles")]
    public bool playerSpeedCollected;
    public bool enemySlowCollected;

    public void resetData()
    {
        playerPosition = Vector3.zero;
        enemySpeedMultiplier = 1f;

        playerSpeedCollected = false;
        enemySlowCollected = false;
    }
}
