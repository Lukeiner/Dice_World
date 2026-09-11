using UnityEngine;

public class WorldAndDiceController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform worldSphere;
    [SerializeField] private Rigidbody diceRigidBody;
    [SerializeField] private Dice diceScript;

    [Header("Parámetros de Swipe")]
    [SerializeField] private float minSwipeDistance = 50f;
    [SerializeField] private float rotationAngle = 45f; // Grados que rota la esfera
    [SerializeField] private float jumpForce = 5f;


    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isExecutingMove = false;
    private bool isGrounded = true;

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            ProcessSwipe();
        }
    }

    private void ProcessSwipe()
    {
        Vector2 swipeVector = endTouchPosition - startTouchPosition;

        // Le preguntamos directamente al script del dado si está en el suelo
        if (swipeVector.magnitude >= minSwipeDistance && diceScript.isGrounded)
        {
            swipeVector.Normalize();

            if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
            {
                if (swipeVector.x > 0) ExecuteMove(Vector3.down);
                else ExecuteMove(Vector3.up);
            }
            else
            {
                if (swipeVector.y > 0) ExecuteMove(Vector3.right);
                else ExecuteMove(Vector3.left);
            }
        }
    }

    private void ExecuteMove(Vector3 rotationAxis)
    {
        // 1. Impulso de salto
        diceRigidBody.linearVelocity = Vector3.zero; // O .velocity si usás versión anterior a Unity 2023
        diceRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // 2. Rotar la esfera
        worldSphere.Rotate(rotationAxis, rotationAngle, Space.World);
    }

}
