using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public StudioEventEmitter StudioEventEmitter;

    public int LiquidTemperature = 50;
    public int IngredientEffectivenessMultipler = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only do checks on tagged ingredients.
        if (collision.gameObject.tag != "Ingredient") return;

        Ingredient collidedIngredient = collision.gameObject.GetComponent<Ingredient>();

        Debug.Log("The collided ingredient has a material index of: No");

        // play gwa gwa
        StudioEventEmitter.Play();

        Destroy(collision.gameObject);
    }
}
