using UnityEngine;

public class Dice : MonoBehaviour
{
    public bool isGrounded { get; private set; } = true;

    private void OnCollisionEnter(Collision collision)
    {
        // Se ejecuta en el cubo cuando toca la esfera
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // Opcional: marca inmediatamente que despegó del suelo
        isGrounded = false;
    }
}
