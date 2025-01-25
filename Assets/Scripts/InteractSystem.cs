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
    #endregion

    private void Start()
    {
        pickupDistance = 2f;
        interactKey = KeyCode.E;
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

                if (raycastHit.transform.TryGetComponent(out grabbableObject))
                {
                    grabbableObject.Grab(objectGrabPointTransform);
                }
            }
        }
        else
        {
            //if grabbed -> drop
            grabbableObject.Drop();
            grabbableObject = null;
        }
    }
}
