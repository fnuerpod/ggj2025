using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public int materialIndex;
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

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>(); // Get the MeshRenderer component
        meshRenderer.materials[materialIndex] = new Material(Shader.Find("Universal Render Pipeline/Lit")); // Create a new material for the ingredient randomisation
        meshRenderer.materials[materialIndex].color = colours[Random.Range(0, colours.Length)]; // Change the ingredient's colour to one of the random colours
    }
}
