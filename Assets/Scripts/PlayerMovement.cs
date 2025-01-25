using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements.Experimental;
using static UnityEditorInternal.VersionControl.ListControl;
using Microsoft.Win32.SafeHandles;

//require rigidbody
[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement:MonoBehaviour
{
    #region Variables
    [Header("References")]
    public Transform orientation;
    public Transform faceDirection;
    public Transform grabPoint;
    public Transform playerCam;
    private Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float groundDrag;
    Coroutine SmoothlyLerpMoveSpeedCo;
    public float maxYSpeed;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool canJump;

    [Header("Slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    bool exitingSlope;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;

    float xInput;
    float yInput;

    Vector3 moveDirection;

    [Header("Movement State")]
    public MovementState state;
    public enum MovementState
    {
        walking,
        air
    }
    #endregion

    private void Start()
    {
        //assign variables
        walkSpeed = 6;
        moveSpeed = walkSpeed;
        playerHeight = 2;
        groundDrag = 5f;

        //jump
        jumpForce = 5f;
        jumpCooldown = 0.25f;
        airMultiplier = 0.4f;
        canJump = true;

        //slope
        maxSlopeAngle = 40f;
        exitingSlope = false;

        //rb
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MyInput();

        SpeedControl();

        StateHandler();

        HandleDrag();

        FaceDirection();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    #region Input
    private void MyInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //jump
        if (Input.GetKey(jumpKey) && canJump && grounded)
        {
            Jump();
        }
    }
    #endregion

    //code to rotate the face of the main character towards where the last move command was
    private void FaceDirection()
    {
        //rotate the face direction
        if (moveDirection != Vector3.zero)
        {
            faceDirection.transform.rotation = Quaternion.LookRotation(moveDirection);
            grabPoint.transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    private void StateHandler()
    {
        
        //Mode - walking
        if (grounded)
        {
            state = MovementState.walking;
        }

        //Mode - air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        //calculate move direction
        moveDirection = orientation.forward * yInput + orientation.right * xInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        //on ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        //in the air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        //turn off gravity while on slope to prevent sliding
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        //limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            //limit velocity if needed
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }

        //limit y velocity
        if (maxYSpeed != 0 && rb.linearVelocity.y > maxYSpeed)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxYSpeed, rb.linearVelocity.z);
    }

    #region Drag
    //drag
    private void HandleDrag()
    {
        if (state == MovementState.walking)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, ground))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        else
        {
            return false;
        }
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    #endregion

    #region Jump
    private void Jump()
    {
        canJump = false;

        //reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump()
    {
        canJump = true;
    }
    #endregion
}
