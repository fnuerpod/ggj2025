using FMODUnity;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public StudioEventEmitter StudioEventEmitter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StudioEventEmitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
