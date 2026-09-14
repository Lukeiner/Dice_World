using UnityEngine;
using TMPro;

public class Dice : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text valueText;
    public bool isGrounded { get; private set; } = true;

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private struct DiceFace
    {
        public Vector3 localDirection;
        public int value;

        public DiceFace(Vector3 dir, int val)
        {
            localDirection = dir;
            value = val;
        }
    }

    private DiceFace[] faces;

    private void Awake()
    {
        // Mapeo inicial de las 6 caras
        faces = new DiceFace[]
        {
            new DiceFace(Vector3.up, 1),
            new DiceFace(Vector3.down, 6),
            new DiceFace(Vector3.back, 2),
            new DiceFace(Vector3.forward, 5),
            new DiceFace(Vector3.right, 3),
            new DiceFace(Vector3.left, 4)
        };
    }
    private void Start()
    {
        UpdateTopFaceUI();
    }

    public int GetTopFaceValue()
    {
        int topValue = 1;
        float maxDot = -1f;

        foreach (var face in faces)
        {
            // Transformar la dirección local de la cara a espacio de mundo
            Vector3 worldFaceDirection = transform.TransformDirection(face.localDirection);

            // Comparar con el vector vertical global (Vector3.up)
            float dotProduct = Vector3.Dot(worldFaceDirection, Vector3.up);

            if (dotProduct > maxDot)
            {
                maxDot = dotProduct;
                topValue = face.value;
            }
        }

        return topValue;
    }

    public void UpdateTopFaceUI()
    {
        int currentValue = GetTopFaceValue();
        if (valueText != null)
        {
            valueText.text = "Cara: " + currentValue;
        }
        Debug.Log("Resultado del movimiento: " + currentValue);
    }

}
