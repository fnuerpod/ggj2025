using FMODUnity;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Pot : MonoBehaviour
{
    FMOD.Studio.EventInstance Sound_PotBubble;

    public int LiquidTemperature = 0;
    public int IngredientEffectivenessMultipler = 1;
    public int SecondsBetweenTemperatureIncrease = 1;

    private long LastTemperatureIncrease;

    private float Lerp_MinHeight = 0.571f;
    private float Lerp_MaxHeight = 0.818f;

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

        // initialise sounds.
        Sound_PotBubble = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/PotGame_SFX_PotBubble");
        Sound_PotBubble.start();
    }

    private void Update()
    {
        long CurrentUnixTime = GetUnixTime();

        if (CurrentUnixTime > LastTemperatureIncrease + (SecondsBetweenTemperatureIncrease * 1000) && LiquidTemperature < 100)
        {
            LiquidTemperature += 1;
            LastTemperatureIncrease = GetUnixTime();
        }

        transform.localPosition = new Vector3(0, LerpCylinderHeight(), 0);

        //Sound_PotBubble.setParameterByName("PotIntensity", (float)LiquidTemperature);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PotIntensity", (float)LiquidTemperature);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only do checks on tagged ingredients.
        if (collision.gameObject.tag != "Ingredient") return;

        Ingredient collidedIngredient = collision.gameObject.GetComponent<Ingredient>();

        // Check if ingredient will accept drop (ie. if player is holding don't gwa gwa)
        if (!collidedIngredient.acceptDrop) return;

        int AmountToDecreaseBy = Mathf.RoundToInt(collidedIngredient.ingredientEffectiveness * IngredientEffectivenessMultipler);

        LiquidTemperature -= AmountToDecreaseBy;

        // play gwa gwa
        //StudioEventEmitter_PotionInsert.Play();

        Destroy(collision.gameObject);
    }

    
}
