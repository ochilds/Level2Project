using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Controls controller;
    public Rigidbody myRigidBody;
    public Camera playerCamera;
    Vector2 cubeRotation;
    public float jumpForce;
    public string state;
    // Start is called before the first frame update
    void Start() {
        // Enable control scheme
        controller = new Controls();
        controller.Gameplay.Enable();
        // Reset internal cube rotation
        cubeRotation = new Vector2(0, 0);
        // Subscribe Jump method for when jump button gets pressed
        controller.Gameplay.Jump.performed += Jump;
        state = "Grounded";
        // Hide and centre cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Subscribe Punch method for when left click is pressed
        controller.Gameplay.Punch.performed += Punch;
    }

    private void Update() {
        // Snap camera to player
        // Do this in Update instead of FixedUpdate to avoid jitters
        playerCamera.transform.position = transform.position + new Vector3(0, 0.45f, 0);
    }

    private void FixedUpdate() {
        // Get move vector and call MovePlayer with that vector
        Vector2 moveInputVector = controller.Gameplay.Move.ReadValue<Vector2>();
        MovePlayer(moveInputVector);
        // Get camera rotate vector and call RotateCamera with that vector
        Vector2 inputRotateVector = controller.Gameplay.MoveCamera.ReadValue<Vector2>();
        RotateCamera(inputRotateVector);
        // Update State
        UpdateState();
    }

    private void UpdateState() {
        // If raycast pointing down collides player if grounded
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f)) {
            state = "Grounded";
        }
        // Else player is airborne
        else {
            state = "Airborne";
        }
    }

    private void MovePlayer(Vector2 MoveAmount) {
        // Set force multiplier
        float force = 75f;
        // Rotate the move vector to face to correct way
        MoveAmount = RotateVector2(MoveAmount, -cubeRotation.y);
        // Add the force to rigid body
        myRigidBody.AddForce(new Vector3(force * MoveAmount.x, 0, force * MoveAmount.y));
    }

    private void RotateCamera(Vector2 RotateAmount) {
        // Check so camera doesn't move outside limits
        if (!(cubeRotation.x <= 90 && cubeRotation.x - RotateAmount.y > 90) && !(cubeRotation.x >= -90 && cubeRotation.x - RotateAmount.y < -90))
        {
            // Rotate both x and y (Done in this order and way to stop weird rotation problems)
            playerCamera.transform.Rotate(new Vector3(-RotateAmount.y, 0, 0), Space.Self);
            playerCamera.transform.Rotate(new Vector3(0, RotateAmount.x, 0), Space.World);
            // Rotate player around the y axis for movement purposes
            transform.Rotate(new Vector3(0, RotateAmount.x, 0), Space.Self);
            // Update internal cube rotation
            cubeRotation += new Vector2(-RotateAmount.y, RotateAmount.x);
        }
    }

    public void Jump(InputAction.CallbackContext context) {
        // If player grounded
        if (state == "Grounded") {
            // Add up force with jump force to rigidbody
            myRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
        }
    }

    public void Punch(InputAction.CallbackContext context) {
        // Raycast forward
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3f)) {
            // if raycast hits object that can break call break on that object
            if (hit.transform.gameObject.TryGetComponent<ItemBreakingLogic>(out ItemBreakingLogic logic)) {
                logic.Break();
            }
        }
    }

    public Vector2 RotateVector2(Vector2 v, float delta) {
        // Convert the angle into radians
        delta *= Mathf.Deg2Rad;
        // Use matrix multiplication to rotate the vector the specified amount
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
}
