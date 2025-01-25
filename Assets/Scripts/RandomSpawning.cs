using UnityEngine;

public class RandomSpawning : MonoBehaviour
{
    public GameObject[] ingredients;
    public Transform[] spawnPoints;

    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++) // For each spawn point
            Instantiate(ingredients[Random.Range(0, ingredients.Length)], spawnPoints[i].position, Quaternion.identity); // Pick a random ingredient and spawn in a random spawn location
    }
}
