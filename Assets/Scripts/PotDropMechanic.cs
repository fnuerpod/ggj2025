using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class PotDropMechanic : MonoBehaviour
{
    [SerializeField] private RandomSpawning spawnCode;
    [SerializeField] private Transform PotionDropHeight;
    [SerializeField] private KeyCode interactKey;

    public StudioEventEmitter Sound_PotionDrop;

    public bool PlayerInCollisionField = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void GetInput()
    {
        //on the press of the interact key and when droppable.
        if (Input.GetKeyDown(interactKey))
        {
            GameObject playerController = GameObject.FindGameObjectWithTag("PlayerController");
            
            InteractSystem interactSystem = playerController.GetComponent<InteractSystem>();
            if (interactSystem == null)
            {
                Debug.LogWarning("Player controller exists without interaction system!");
                return;
            }

            if (interactSystem.HoldingIngredient != null && PlayerInCollisionField)
            {
                DropIntoPot(interactSystem);
            }
        }
    }

    private void DropIntoPot(InteractSystem interactSystem)
    {
        Ingredient heldIngredient = interactSystem.HoldingIngredient;
        heldIngredient.acceptDrop = true;
        interactSystem.Drop();

        heldIngredient.transform.position = PotionDropHeight.transform.position;
        Sound_PotionDrop.Play();

        StartCoroutine(WaitBeforeSpawn());
    }

    private System.Collections.IEnumerator WaitBeforeSpawn()
    {
        yield return new WaitForSeconds(1.0f);
        spawnCode.SpawnIngredients();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput(); 
    }

    private bool MechanicTriggerChecks(GameObject collision)
    {
        if (collision.tag != "Player") return false;

        GameObject playerController = GameObject.FindGameObjectWithTag("PlayerController");

        if (playerController == null)
        {
            Debug.LogWarning("No player controller exists yet the player GameObject exists?");
            return false;
        }

        return true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!MechanicTriggerChecks(collision.gameObject)) return;

        Debug.Log("Player enter!!!");

        GameObject playerController = GameObject.FindGameObjectWithTag("PlayerController");
        playerController.GetComponent<InteractSystem>().CanDrop = false;
        PlayerInCollisionField = true;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (!MechanicTriggerChecks(collision.gameObject)) return;

        Debug.Log("Player leave!!!");

        GameObject playerController = GameObject.FindGameObjectWithTag("PlayerController");
        playerController.GetComponent<InteractSystem>().CanDrop = true;
        PlayerInCollisionField = false;
    }
}
