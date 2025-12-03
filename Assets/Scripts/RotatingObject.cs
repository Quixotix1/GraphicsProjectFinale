using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    private float yRot = 0f;

    void Update()
    {
        yRot += m_Speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRot, transform.rotation.eulerAngles.z);
    }
}
