using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControlls : MonoBehaviour
{
    public Vector3 targetDir;
    public GameObject player;
    public Transform playerObject;
    public Rigidbody rb;
    public Transform orientation;
    public float rotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotationSpeed = 10f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the camera
        targetDir = player.transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        //rotate the orientation
        Vector3 viewDir = player.transform.position - new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //rotate the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, rotationSpeed * Time.deltaTime);
        }
    }
}
