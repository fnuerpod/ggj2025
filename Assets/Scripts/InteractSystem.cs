using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class InteractSystem : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform faceDirection;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private float pickupDistance;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private TextMeshProUGUI ingredientPickupHint;

    public PotDropMechanic dropMechanic;

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

    private void CheckIfGrabbableNearby()
    {
        if (grabbableObject != null)
        {
            // determine the text to be used for pickup/drop
            string ActionHintText = "fault";

            if (HoldingIngredient != null)
            {
                // holding ingredient check proximity to pot.
                if (dropMechanic.PlayerInCollisionField)
                {
                    ActionHintText = "drop into pot";
                }
                else
                {
                    ActionHintText = "drop";
                }
            }
            else
            {
                ActionHintText = "pick up";
            }

            ingredientPickupHint.text = HoldingIngredient.hints[0] + "\r\nPress <color=#00ff00>E</color> to " + ActionHintText;
            ingredientPickupHint.enabled = true;

            return;
        }

        if (Physics.Raycast(faceDirection.position, faceDirection.forward, out RaycastHit raycastHit, pickupDistance, pickupLayerMask))
        {
            // check if it is a grabbable object
            GrabbableObject possiblyGrabbable = null;
            Ingredient possiblyGrabbableIngredient = null;

            if (raycastHit.transform.TryGetComponent(out possiblyGrabbable) && raycastHit.transform.TryGetComponent(out possiblyGrabbableIngredient)) { 
    
                    Debug.Log("We have a grabbable object in our horizons. nomnomnomom.");

                // determine the text to be used for pickup/drop
                string ActionHintText = "fault";

                if (HoldingIngredient != null)
                {
                    // holding ingredient check proximity to pot.
                    if (dropMechanic.PlayerInCollisionField)
                    {
                        ActionHintText = "drop into pot";
                    } else
                    {
                        ActionHintText = "drop";
                    }
                } else
                {
                    ActionHintText = "pick up";
                }

                ingredientPickupHint.text = possiblyGrabbableIngredient.hints[0] + "\r\nPress <color=#00ff00>E</color> to " + ActionHintText;
                ingredientPickupHint.enabled = true;
            } else {
                ingredientPickupHint.enabled = false;
            }
        } else
        {
            ingredientPickupHint.enabled = false;
        }
    }

    private void Update()
    {
        GetInput();
        CheckIfGrabbableNearby();
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

                    //set animator 
                    animator.SetBool("holdItem", true);
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
            //set animator 
            animator.SetBool("holdItem", false);
            grabbableObject.Drop();
            grabbableObject = null;
            HoldingIngredient = null;

        }
    }
}
