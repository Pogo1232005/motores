using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    static string path = Application.persistentDataPath + "/save.json";

    [System.Serializable] class SaveData
    {
        public Vector3 playerPosition;
        public float playerSpeedMultiplier;
        public float enemySpeedMultiplier;
        public bool playerSpeedCollected;
        public bool enemySlowCollected;
        public Vector3[] enemyposition;
    }
    public static void Save(GameData datasave, EnemyController[] enemies)
    {
        SaveData data = new SaveData();
        data.playerPosition = datasave.playerPosition;
        data.playerSpeedMultiplier = datasave.playerSpeedMultiplier;
        data.enemySpeedMultiplier = datasave.enemySpeedMultiplier;
        data.playerSpeedCollected = datasave.playerSpeedCollected;
        data.enemySlowCollected = datasave.enemySlowCollected;
        data.enemyposition = new Vector3[enemies.Length];

        for(int i = 0; i < enemies.Length; i++)
        {
            data.enemyposition[i] = enemies[i].transform.position;
        }

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);
    }

    public static void Load(GameData dataSave, GameObject player, EnemyController[] enemies)
    {
        if (!File.Exists(path))
        {
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        dataSave.playerPosition = data.playerPosition;
        dataSave.playerSpeedMultiplier = data.playerSpeedMultiplier;
        dataSave.enemySpeedMultiplier = data.enemySpeedMultiplier;
        dataSave.playerSpeedCollected= data.playerSpeedCollected;
        dataSave.enemySlowCollected= data.enemySlowCollected;

        player.transform.position = data.playerPosition;

        for(int i = 0;i < enemies.Length; i++)
        {
            if(i < data.enemyposition.Length)
            {
                enemies[i].transform.position = data.enemyposition[i];
            }
        }

        Debug.Log("Partida Cargada");
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save eliminado");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
