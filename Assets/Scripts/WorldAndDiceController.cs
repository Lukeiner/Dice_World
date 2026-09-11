using UnityEngine;
using System.Collections;

public class WorldAndDiceController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform worldSphere;
    [SerializeField] private Rigidbody diceRigidBody;
    [SerializeField] private Dice diceScript;
    [SerializeField] private DiceRotation diceRotationScript;

    [Header("Parámetros de Swipe")]
    [SerializeField] private float minSwipeDistance = 50f;
    [SerializeField] private float rotationAngle = 45f; // Grados que rota la esfera
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotationSpeed = 10f;


    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isRotating = false;
    //private bool isExecutingMove = false;
    //private bool isGrounded = true;

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

        if (swipeVector.magnitude >= minSwipeDistance && diceScript.isGrounded && !isRotating)
        {
            swipeVector.Normalize();

            if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
            {
                // SWIPE HORIZONTAL
                if (swipeVector.x > 0)
                {
                    // Swipe Derecha -> Esfera gira en Y, Dado rueda a la Derecha (Eje Z negativo)
                    ExecuteMove(Vector3.up, Vector3.back);
                }
                else
                {
                    // Swipe Izquierda -> Esfera gira en -Y, Dado rueda a la Izquierda (Eje Z positivo)
                    ExecuteMove(Vector3.down, Vector3.forward);
                }
            }
            else
            {
                // SWIPE VERTICAL
                if (swipeVector.y > 0)
                {
                    // Swipe Arriba -> Esfera gira en X, Dado rueda hacia Adelante (Eje X positivo)
                    ExecuteMove(Vector3.right, Vector3.right);
                }
                else
                {
                    // Swipe Abajo -> Esfera gira en -X, Dado rueda hacia Atrás (Eje X negativo)
                    ExecuteMove(Vector3.left, Vector3.left);
                }
            }
        }
    }

    private void ExecuteMove(Vector3 rotationAxis, Vector3 diceRollAxis)
    {
        // 1. Impulso de salto
        diceRigidBody.linearVelocity = Vector3.zero;
        diceRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        diceRotationScript.Roll(diceRollAxis);

        // 2. Usar Quaternion.Euler en espacio global para evitar desalineaciones diagonales
        Quaternion deltaRotation = Quaternion.AngleAxis(rotationAngle, rotationAxis);
        Quaternion targetRotation = deltaRotation * worldSphere.rotation;

        StartCoroutine(RotateSphereRoutine(targetRotation));
    }

    private IEnumerator RotateSphereRoutine(Quaternion targetRotation)
    {
        isRotating = true;

        while (Quaternion.Angle(worldSphere.rotation, targetRotation) > 0.1f)
        {
            worldSphere.rotation = Quaternion.Slerp(worldSphere.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        worldSphere.rotation = targetRotation; // Ajuste final exacto
        isRotating = false;
    }
}
