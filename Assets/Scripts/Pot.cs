using FMODUnity;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Pot : MonoBehaviour
{
    FMOD.Studio.EventInstance Sound_PotBubble;
    FMOD.Studio.EventInstance BGM;

    public int LiquidTemperature = 0;
    public int IngredientEffectivenessMultipler = 1;
    public int SecondsBetweenTemperatureIncrease = 1;

    public ParticleSystem particleSystem;

    private long LastTemperatureIncrease;

    private float Lerp_MinHeight = 0.571f;
    private float Lerp_MaxHeight = 0.818f;

    private float LerpFX_minLifetime = 0.13f;
    private float LerpFX_maxLifetime = 1.2f;

    private float LerpFX_minSpeed = 0.48f;
    private float LerpFX_maxSpeed = 1.24f;

    private float LerpFX_minEmissionRate = 22.19f;
    private float LerpFX_maxEmissionRate = 200f;

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

        // initialise sounds.;
        BGM = FMODUnity.RuntimeManager.CreateInstance("event:/MUSIC/PotGame_Music_GameTheme");
        Sound_PotBubble = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/PotGame_SFX_PotBubble");
        BGM.setVolume(0.1f);

        BGM.start();
        Sound_PotBubble.start();
    }

    private void LerpParticleEffects()
    {
        // Calculate the interpolation factor based on liquidTemperature (clamped between 0 and 70)
        float t = Mathf.Clamp01(LiquidTemperature / 70f);

        // Lerp between the min and max values based on the clamped t value
        var main = particleSystem.main;
        main.startLifetime = Mathf.Lerp(LerpFX_minLifetime, LerpFX_maxLifetime, t);
        main.startSpeed = Mathf.Lerp(LerpFX_minSpeed, LerpFX_maxSpeed, t);

        // Adjust emission rate dynamically
        var emission = particleSystem.emission;
        emission.rateOverTime = Mathf.Lerp(LerpFX_minEmissionRate, LerpFX_maxEmissionRate, t);

        // If liquidTemperature is greater than 70, clamp to maximum values directly
        if (LiquidTemperature > 70)
        {
            main.startLifetime = LerpFX_maxLifetime;
            main.startSpeed = LerpFX_maxSpeed;
            emission.rateOverTime = LerpFX_maxEmissionRate;
        }
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

        LerpParticleEffects();
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
