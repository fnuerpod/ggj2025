using UnityEngine;

public class PotDropMechanic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool MechanicTriggerChecks(Collider collision)
    {
        if (collision.gameObject.tag != "Player") return false;

        GameObject playerController = GameObject.FindGameObjectWithTag("PlayerController");

        if (playerController == null)
        {
            Debug.LogWarning("No player controller exists yet the player GameObject exists?");
            return false;
        }

        if (playerController.GetComponent<InteractSystem>().HoldingIngredient == null) return false;

        return true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!MechanicTriggerChecks(collision)) return;

        Debug.Log("Player enter!!!");
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!MechanicTriggerChecks(collision)) return;

        Debug.Log("Player leave!!!");
    }
}
