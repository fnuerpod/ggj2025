using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public StudioEventEmitter StudioEventEmitter;
    public BoxCollider Colldier;

    private bool SoundStartedOnLastUpdateCycle = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "Pot")
            {
                if (Input.GetMouseButton(0))
                {
                    if (SoundStartedOnLastUpdateCycle) return;
                    StudioEventEmitter.Play();
                    SoundStartedOnLastUpdateCycle = true;
                }
                else
                {
                    SoundStartedOnLastUpdateCycle = false;
                }
            }
            else
            {
                SoundStartedOnLastUpdateCycle = false;
            }
        } else {
            SoundStartedOnLastUpdateCycle = false;
        }
    }
}
