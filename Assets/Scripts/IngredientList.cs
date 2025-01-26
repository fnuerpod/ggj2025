using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class IngredientList : MonoBehaviour
{
    private int page;
    public List<GameObject> ingredientsList = new List<GameObject>();
    private List<PageHints> pageHintsList = new List<PageHints>();

    [SerializeField] private GameObject ingredientDisplay;
    [SerializeField] private TextMeshProUGUI[] hintText;
    [SerializeField] private int maxIngredients;

    private class PageHints // class for storing hints for each ingredient on each page
    {
        public string IngredientName { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public bool OnList { get; set; }
        public bool ShowSize { get; set; }
        public bool ShowColor { get; set; }
    }

    void Start()
    {
        StartCoroutine(FindIngredients()); // Find ingredients
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && ingredientDisplay.activeSelf) // If the player presses Q
            ingredientDisplay.SetActive(false); // Hide the ingredient list
        else if (Input.GetKeyDown(KeyCode.Q) && !ingredientDisplay.activeSelf) // If the player presses Q
            ingredientDisplay.SetActive(true); // Show the ingredient list
    }

    public IEnumerator FindIngredients()
    {
        yield return new WaitForSeconds(0.1f); // Wait for ingredients to be created
        GameObject[] ingredientsArray = GameObject.FindGameObjectsWithTag("Ingredient"); // Find objects with the "Ingredient" tag and assign them to the ingredients array

        for (int i = 0; i < maxIngredients; i++) // While i is less than the maximum number of ingredients
        {
            if (ingredientsList.Count <= 3)
            {
                GameObject ingredient = null; // Pick a random ingredient from the ingredients array
                do // Do the following
                    ingredient = ingredientsArray[Random.Range(0, ingredientsArray.Length)]; // Pick a random ingredient from the ingredients array
                while (ingredientsList.Exists(ingredientInList => ingredientInList.name == ingredient.name)); // While the ingredient is already in the list (to prevent duplicates)
                ingredientsList.Add(ingredient); // Add the ingredient to the ingredient list
            }
        }
        RandomiseHints(); // Randomise hints
        UpdatePage(); // Update the page
    }

    void RandomiseHints()
    {
        pageHintsList.Clear(); // Clear the current page hints list
        foreach (var ingredient in ingredientsList) // For each ingredient in the list
        {
            PageHints pageHints = new PageHints // Create a new page hints object
            {
                IngredientName = ingredient.GetComponent<Ingredient>().hints[0], // Set the ingredient name hint
                OnList = true,
                ShowColor = Random.value > 0.5f, // Randomly decide whether to show the colour hint
                ShowSize = Random.value > 0.5f // Randomly decide whether to show the size hint
            };

            if (pageHints.OnList) // If the ingredient is on the list
                ingredient.GetComponent<Ingredient>().ingredientEffectiveness = 1; // Set the ingredient effectiveness to 1
            else // If not
                ingredient.GetComponent<Ingredient>().ingredientEffectiveness = 0; // Set the ingredient effectiveness to 0

            if (pageHints.ShowColor) // If the colour is to be shown
                pageHints.Color = ingredient.GetComponent<Ingredient>().hints[1]; // Set the colour hint
            else // If not
                ingredient.GetComponent<Ingredient>().ingredientEffectiveness += 2; // Increase the ingredient effectiveness

            if (pageHints.ShowSize) // If the size is to be shown
                pageHints.Size = ingredient.GetComponent<Ingredient>().hints[2]; // Set the size hint
            else // If not
                ingredient.GetComponent<Ingredient>().ingredientEffectiveness += 2; // Increase the ingredient effectiveness

            pageHintsList.Add(pageHints); // Add these hints to the list of page hints
        }
    }

    void UpdatePage()
    {
        PageHints currentPageHints = pageHintsList[page]; // Get the hints for the current page
        hintText[0].text = currentPageHints.IngredientName; // Set the ingredient name hint
        hintText[1].gameObject.SetActive(currentPageHints.ShowColor); // Set the colour hint active or inactive
        hintText[2].gameObject.SetActive(currentPageHints.ShowSize); // Set the size hint active or inactive

        if (currentPageHints.ShowColor) // If the colour is to be shown
        {
            hintText[1].text = currentPageHints.Color; // Set the colour hint text
        }

        if (currentPageHints.ShowSize) // If the size is to be shown
        {
            hintText[2].text = currentPageHints.Size; // Set the size hint text
        }
    }

    public void NextPage()
    {
        if (page < maxIngredients - 1) // If the page is less than the maximum number of ingredients
        {
            page++; // Show the next page
            UpdatePage(); // Update the page
        }
    }

    public void PreviousPage() 
    {
        if (page > 0) // If the page is greater than 0
        {
            page--; // Show the previous page
            UpdatePage(); // Update the page
        }
    }
}
