using UnityEngine;
using System.Collections;

public class DiceRotation : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 16f;
    private Coroutine currentRollCoroutine;
    private Dice diceScript;


    private void Awake()
    {
        diceScript = GetComponent<Dice>();
    }
    public void Roll(Vector3 rollAxis)
    {
        if (currentRollCoroutine != null)
        {
            StopCoroutine(currentRollCoroutine);
        }

        // Calcular los 90 grados aplicando la rotación sobre los ejes locales del cubo
        Quaternion targetRotation = Quaternion.AngleAxis(90f, rollAxis) * transform.rotation;

        currentRollCoroutine = StartCoroutine(RollRoutine(targetRotation));
    }

    private IEnumerator RollRoutine(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            yield return null;
        }

        // Asignar la rotación objetivo final de forma limpia sin pasar por Euler
        transform.rotation = targetRotation;

        if (diceScript != null)
        {
            diceScript.UpdateTopFaceUI();
        }
    }
}
