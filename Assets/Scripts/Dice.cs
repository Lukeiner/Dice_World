using UnityEngine;

public class Dice : MonoBehaviour
{
    public bool isGrounded { get; private set; } = true;

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
