using UnityEngine;

public class IngredientList : MonoBehaviour
{
    public GameObject[] ingredients;

    void Start()
    {
        foreach (GameObject ingredient in ingredients)
        {
            //ingredient.GetComponent<Ingredient>().hints
        }
    }
}
