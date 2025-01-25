using TMPro;
using UnityEngine;

public class IngredientList : MonoBehaviour
{
    [SerializeField] private GameObject[] ingredients;
    [SerializeField] private TextMeshProUGUI[] hints;

    void Start()
    {
        foreach (GameObject ingredient in ingredients)
        {
            foreach(string hint in ingredient.GetComponent<Ingredient>().hints)
            {
                
            }
        }
    }
}
