using System.Collections.Generic;
using UnityEngine;

public class RandomSpawning : MonoBehaviour
{
    public GameObject[] ingredients;
    public Transform[] spawnPoints;

    void Start()
    {
        List<GameObject> spawnedIngredients = new List<GameObject>();

        for (int i = 0; i < spawnPoints.Length; i++) // For each spawn point
        {
            GameObject ingredientToSpawn = null; // Create a game object variable
            do // Do the following
                ingredientToSpawn = ingredients[Random.Range(0, ingredients.Length)]; // Pick a random ingredient from the ingredients array
            while (spawnedIngredients.Exists(ingredient => ingredient.name == ingredientToSpawn.name)); // While the ingredient is already in the list (to prevent duplicates)
            spawnedIngredients.Add(ingredientToSpawn); // Add the ingredient to the list of spawned ingredients (for checking the next time)
            Instantiate(ingredientToSpawn, spawnPoints[i].position, Quaternion.identity); // Pick a random ingredient and spawn in a random spawn location
        }
    }
}
