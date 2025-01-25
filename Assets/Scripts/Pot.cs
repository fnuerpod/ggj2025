using FMODUnity;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public StudioEventEmitter StudioEventEmitter;

    public int LiquidTemperature = 0;
    public int IngredientEffectivenessMultipler = 1;
    public int SecondsBetweenTemperatureIncrease = 1;

    private long LastTemperatureIncrease;

    private float Lerp_MinHeight = 1.571f;
    private float Lerp_MaxHeight = 1.818f;



    private static long GetUnixTime()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime currentTime = DateTime.UtcNow;

        // Get the difference in milliseconds
        return (long)(currentTime - epochStart).TotalMilliseconds;
    }

    private float LerpCylinderHeight()
    {
        return Mathf.Lerp(Lerp_MinHeight, Lerp_MaxHeight, LiquidTemperature / 100f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LastTemperatureIncrease = GetUnixTime();
        Debug.Log(LastTemperatureIncrease);
    }

    private void Update()
    {
        long CurrentUnixTime = GetUnixTime();

        if (CurrentUnixTime > LastTemperatureIncrease + (SecondsBetweenTemperatureIncrease * 1000))
        {
            LiquidTemperature += 1;
            LastTemperatureIncrease = GetUnixTime();
        }

        transform.position = new Vector3(transform.position.x, LerpCylinderHeight(), transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only do checks on tagged ingredients.
        if (collision.gameObject.tag != "Ingredient") return;

        Ingredient collidedIngredient = collision.gameObject.GetComponent<Ingredient>();

        int AmountToDecreaseBy = Mathf.RoundToInt(collidedIngredient.ingredientEffectiveness * IngredientEffectivenessMultipler);

        LiquidTemperature -= AmountToDecreaseBy;

        // play gwa gwa
        StudioEventEmitter.Play();

        Destroy(collision.gameObject);
    }
}
