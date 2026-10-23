using System.Collections;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float m_MinY, m_MaxY;
    [SerializeField] private float m_Speed;

    private float yPos;

    private void Start()
    {
        yPos = m_MaxY;
    }

    void Update()
    {
        yPos = (m_MinY - m_MaxY) / 2f * Mathf.Cos(Mathf.PI * (Time.time * m_Speed)) + (m_MinY + m_MaxY) / 2f;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
