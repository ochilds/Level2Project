using System;
using System.Collections;
using System.Collections.Concurrent;
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
    public float moveForce;
    public string state;
    public GameObject inventoryUI;
    public GameObject selectedSlot;
    public float jumpTime;
    public float distanceTraveled;
    public float amountLookedAround;
    public int objectsHit;
    // Start is called before the first frame update
    void Start() {
        // Enable control scheme
        controller = new Controls();
        controller.Gameplay.Enable();
        // Reset internal cube rotation
        cubeRotation = new Vector2(0, 0);
        // Initiliase state
        state = "Grounded";
        // Hide and centre cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Subscribe Punch method for when left click is pressed
        controller.Gameplay.Punch.performed += Punch;
        // Subscribe Sprint method to sprint key
        controller.Gameplay.Sprint.started += StartSprint;
        controller.Gameplay.Sprint.canceled += EndSprint;
        // Subscribe inventory hotkeys
        controller.Gameplay.Hotkey1.started += Hotkey1;
        controller.Gameplay.Hotkey2.started += Hotkey2;
        controller.Gameplay.Hotkey3.started += Hotkey3;
        controller.Gameplay.Hotkey4.started += Hotkey4;
        controller.Gameplay.Hotkey5.started += Hotkey5;
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
        // Jump if nessercary
        if (controller.Gameplay.Jump.IsPressed()) {
            Jump();
        }
        // Update State
        UpdateState();
        // Update Gravity
        UpdateGravity();
    }

    public void StartSprint(InputAction.CallbackContext context) {
        moveForce *= 1.75f;
    }

    public void EndSprint(InputAction.CallbackContext context) {
        // This is intentional as a easter egg
        moveForce /= 1.7499f;
    }

    public void UpdateGravity() {
        if (myRigidBody.velocity.y < -1) {
            myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, 
                                               myRigidBody.velocity.y * Time.deltaTime * 52, 
                                               myRigidBody.velocity.z);
        }
    }

    private void UpdateState() {
        // If raycast pointing down collides player if grounded
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f) ||
            Physics.Raycast(transform.position + (Vector3.forward / 3), Vector3.down, 1.1f) ||
            Physics.Raycast(transform.position + (Vector3.back / 3), Vector3.down, 1.1f) ||
            Physics.Raycast(transform.position + (Vector3.right / 3), Vector3.down, 1.1f) ||
            Physics.Raycast(transform.position + (Vector3.left / 3), Vector3.down, 1.1f)) {
            if (state != "Jumping") {
                state = "Grounded";
                myRigidBody.useGravity = false;
            }
        }
        // Else player is airborne
        else {
            state = "Airborne";
            myRigidBody.useGravity = true;
        }
    }

    private void MovePlayer(Vector2 MoveAmount) {
        // Rotate the move vector to face to correct way
        MoveAmount = RotateVector2(MoveAmount, -cubeRotation.y);
        // Add the force to rigid body
        myRigidBody.AddForce(new Vector3(moveForce * MoveAmount.x, 0, moveForce * MoveAmount.y));
        // Update distance traveled
        distanceTraveled += MathF.Sqrt(MathF.Pow(MoveAmount.x, 2) + MathF.Pow(MoveAmount.y, 2));
    }

    private void RotateCamera(Vector2 RotateAmount) {
        RotateAmount *= Time.deltaTime * 50;
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
        // Update amount looked
        amountLookedAround += MathF.Sqrt(MathF.Pow(RotateAmount.x, 2) + MathF.Pow(RotateAmount.y, 2));
    }

    public void Jump() {
        // If player grounded
        if (state == "Grounded") {
            // Add up force with jump force to rigidbody
            myRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            // Update state
            state = "Jumping";
            // Initiliase jumpTime
            jumpTime = 0;
        }
        if (state == "Jumping" && jumpTime < 0.2) {
            // Add up force with jump force to rigidbody
            myRigidBody.AddForce(Vector3.up * jumpForce / 5, ForceMode.Acceleration);
            // Add time
            jumpTime += Time.deltaTime;
        }
    }

    public void Punch(InputAction.CallbackContext context) {
        // Raycast forward
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 3f)) {
            // If raycast hits object that can break call break on that object
            if (hit.transform.gameObject.TryGetComponent<ItemBreakingLogic>(out ItemBreakingLogic logic)) {
                logic.Break(hit.point, playerCamera.transform.forward);
                // Update objects hit
                objectsHit += 1;
            }
        }
    }

    public void OpenInventory(InputAction.CallbackContext context) {
        // Make inventory UI visible
        inventoryUI.SetActive(true);
        // Switch control map to inventory control
        controller.Gameplay.Disable();
        controller.Inventory.Enable();
        // Make cursor visible
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseInventory(InputAction.CallbackContext context) {
        // Make inventory UI invisible
        inventoryUI.SetActive(false);
        // Switch control map in gameplay
        controller.Inventory.Disable();
        controller.Gameplay.Enable();
        // Hide and centre mouse
        Cursor.lockState = CursorLockMode.Locked;
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

    public void Hotkey1(InputAction.CallbackContext context) {
        selectedSlot.transform.localPosition = new Vector3(-3.97f, 6.75f, 0);
    }

    public void Hotkey2(InputAction.CallbackContext context) {
        selectedSlot.transform.localPosition = new Vector3(-1.97f, 6.75f, 0);
    }

    public void Hotkey3(InputAction.CallbackContext context) {
        selectedSlot.transform.localPosition = new Vector3(0.03f, 6.75f, 0);
    }

    public void Hotkey4(InputAction.CallbackContext context) {
        selectedSlot.transform.localPosition = new Vector3(2.03f, 6.75f, 0);
    }

    public void Hotkey5(InputAction.CallbackContext context) {
        selectedSlot.transform.localPosition = new Vector3(4.03f, 6.75f, 0);
    }
}
