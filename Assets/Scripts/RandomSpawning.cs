using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomSpawning : MonoBehaviour
{
    [SerializeField] private IngredientList ingredientList;
    public GameObject[] ingredients;
    public Transform[] spawnPoints;
    public float time;

    void Start()
    {
        SpawnIngredients(); // Spawn ingredients for the first time
    }

    private void Update()
    {
        time += Time.deltaTime; // Increment the time every second
        PlayerPrefs.SetFloat("Time", time); // Save the time to PlayerPrefs
    }

    public void SpawnIngredients()
    {
        foreach (GameObject ingredient in GameObject.FindGameObjectsWithTag("Ingredient")) // For each ingredient
            Destroy(ingredient); // Destroy the ingredient
        List<GameObject> spawnedIngredients = new List<GameObject>(); // Create a list for the spawned ingredients

        for (int i = 0; i < spawnPoints.Length; i++) // For each spawn point
        {
            GameObject ingredientToSpawn = null; // Create a game object variable
            do // Do the following
                ingredientToSpawn = ingredients[Random.Range(0, ingredients.Length)]; // Pick a random ingredient from the ingredients array
            while (spawnedIngredients.Exists(ingredientInList => ingredientInList.name == ingredientToSpawn.name)); // While the ingredient is already in the list (to prevent duplicates)
            spawnedIngredients.Add(ingredientToSpawn); // Add the ingredient to the list of spawned ingredients (for checking the next time)
            Instantiate(ingredientToSpawn, spawnPoints[i].position, Quaternion.identity); // Pick a random ingredient and spawn in a random spawn location
        }
        ingredientList.ingredientsList.Clear(); // Clear the ingredient list
        ingredientList.StartCoroutine(ingredientList.FindIngredients()); // Find ingredients for the list to re-randomise
    }
}
