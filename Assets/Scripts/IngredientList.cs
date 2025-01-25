using System.Collections;
using TMPro;
using UnityEngine;

public class IngredientList : MonoBehaviour
{
    private GameObject[] ingredients;
    private int page;
    private bool once;
    [SerializeField] private TextMeshProUGUI[] hints;

    void Start()
    {
        StartCoroutine(FindIngredients());
    }

    IEnumerator FindIngredients()
    {
        yield return new WaitForSeconds(1);
        ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
        once = true;
        DisplayPage();
    }

    void DisplayPage()
    {
        if (page < ingredients.Length)
        {
            var ingredient = ingredients[page];
            var ingredientHints = ingredient.GetComponent<Ingredient>().hints;

            if (once)
            {
                for (int i = 0; i < hints.Length; i++)
                {
                    if (i != 0 || Random.value > 0.5f)
                    {
                        hints[i].text = "";
                        ingredient.GetComponent<Ingredient>().ingredientEffectiveness++;
                    }
                }
            }
            once = false;
            for (int i = 0; i < hints.Length; i++)
            {
                if (hints[i].text != "")
                {
                    hints[i].text = ingredientHints[i];
                }
            }
        }
    }

    public void NextPage()
    {
        if (page < ingredients.Length - 1)
        {
            page++;
            DisplayPage();
        }
    }

    public void PreviousPage()
    {
        if (page > 0)
        {
            page--;
            DisplayPage();
        }
    }
}
