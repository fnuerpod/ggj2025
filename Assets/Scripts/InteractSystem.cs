using UnityEngine;
using UnityEngine.Timeline;

public class InteractSystem : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform faceDirection;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private float pickupDistance;
    [SerializeField] private LayerMask pickupLayerMask;
    private GrabbableObject grabbableObject;

    FMOD.Studio.EventInstance Sound_ItemGrab;
    #endregion

    public Ingredient HoldingIngredient;
    public bool CanDrop = true;

    private void Start()
    {
        pickupDistance = 2f;
        interactKey = KeyCode.E;

        Sound_ItemGrab = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/PotGame_SFX_ItemPickup");
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        //on the press of the interact key
        if (Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    private void Interact()
    {
        //check if an item is grabbed already
        if (grabbableObject == null)
        {
            //if not grabbed -> try to grab
            //raycast from the player in the face direction
            if (Physics.Raycast(faceDirection.position, faceDirection.forward, out RaycastHit raycastHit, pickupDistance, pickupLayerMask))
            {
                // check if it is a grabbable object
                if (raycastHit.transform.TryGetComponent(out grabbableObject) && raycastHit.transform.TryGetComponent(out HoldingIngredient))
                {
                    Sound_ItemGrab.start();
                    grabbableObject.Grab(objectGrabPointTransform);
                }
            }
        }
        else
        {
            if (!CanDrop) return;

            //if grabbed -> drop
            grabbableObject.Drop();
            grabbableObject = null;
            HoldingIngredient = null;
        }
    }

    public void Drop()
    {
        if (grabbableObject != null)
        {
            grabbableObject.Drop();
            grabbableObject = null;
            HoldingIngredient = null;
        }
    }
}
