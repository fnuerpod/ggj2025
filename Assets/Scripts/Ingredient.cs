using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public int ingredientEffectiveness = 1;
    [SerializeField] private int materialIndex;
    [SerializeField] private List<string> hints;
    private Color[] colours =
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        Color.white,
        Color.black,
        Color.grey,
        Color.clear
    };
    private float[] sizes =
    {
        0.75f,
        1.0f,
        1.25f
    };

    void Start()
    {
        // Name
        hints.Insert(0, "Name: " + gameObject.name); // Add the name hint to the list of hints
        // Colours
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>(); // Get the MeshRenderer component
        meshRenderer.materials[materialIndex] = new Material(Shader.Find("Universal Render Pipeline/Lit")); // Create a new material for the ingredient randomisation
        meshRenderer.materials[materialIndex].color = colours[Random.Range(0, colours.Length)]; // Change the ingredient's colour to one of the random colours
        string[] colourNames = { "Red", "Blue", "Green", "Yellow", "Magenta", "Cyan", "White", "Black", "Grey", "Clear" }; // Update the colour hint
        hints.Insert(1, "Colour: " + colourNames[System.Array.IndexOf(colours, meshRenderer.materials[materialIndex].color)]); // Add the colour hint to the list of hints
        // Sizes
        transform.localScale *= sizes[Random.Range(0, sizes.Length)]; // Set the size of the ingredient to one of the random sizes
        string[] sizeNames = { "Small", "Medium", "Large" }; // Update the size hint
        hints.Insert(2, "Size: " + sizeNames[System.Array.IndexOf(sizes, transform.localScale)]); // Add the size hint to the list of hints
    }
}
