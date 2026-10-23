using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float m_TurnDuration;
    [SerializeField] float m_MoveDuration;
    [SerializeField] float m_BumpDuration;
    [SerializeField] float m_RaycastDistance;
    [SerializeField] float m_BumpDistance;

    [SerializeField] LayerMask m_InteractableLayers;
    [SerializeField] GameObject m_InteractKey;

    RaycastHit m_LatestCheckedInteraction;
    bool m_CanInteract;
    bool m_InMotion = false;

    private void Start()
    {
        CheckInteract();
    }

    void Update()
    {
        float lookValue = Input.GetAxisRaw("Horizontal");

        if (lookValue != 0f && !m_InMotion)
        {
            StartCoroutine(Turn(lookValue));
        }

        if (!m_InMotion)
        {
            if (Input.GetAxisRaw("Vertical") > 0f)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _, m_RaycastDistance))
                {
                    StartCoroutine(Bump());
                }
                else
                {
                    StartCoroutine(Move());
                }
            }
        }

        if (m_CanInteract && Input.GetButtonDown("Interact"))
        {
            if (m_LatestCheckedInteraction.transform.gameObject.TryGetComponent(out Interact i))
            {
                if (i.requiresKey && Inventory.Instance.keys.Contains(i.key) || !i.requiresKey)
                {
                    i.DoInteraction();
                }
                else
                {
                    i.ToggleLock();
                }
            }

        } 
    }

    IEnumerator Turn(float lookValue)
    {
        m_InteractKey.SetActive(false);
        m_InMotion = true;

        float direction = Mathf.Sign(lookValue);
        float elapsed = 0f;
        float initialRotation = transform.eulerAngles.y;
        float targetRotation = initialRotation + direction * 90f;
        while (elapsed < m_TurnDuration)
        {
            elapsed += Time.deltaTime;
            if (elapsed > m_TurnDuration) 
                elapsed = m_TurnDuration;

            Vector3 rotation = transform.eulerAngles;
            float currentRotation = Mathf.LerpAngle(initialRotation, targetRotation, elapsed / m_TurnDuration);
            rotation.y = currentRotation;
            transform.eulerAngles = rotation;

            yield return null;
        }

        m_InMotion = false;
        CheckInteract();
    }

    IEnumerator Move()
    {
        m_InteractKey.SetActive(false);
        m_InMotion = true;

        float elapsed = 0f;

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + transform.TransformDirection(Vector3.forward) * m_RaycastDistance;

        while (elapsed < m_MoveDuration)
        {
            elapsed += Time.deltaTime;
            if (elapsed > m_MoveDuration)
                elapsed = m_MoveDuration;

            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsed / m_MoveDuration);

            yield return null;
        }

        m_InMotion = false;
        CheckInteract();
    }

    IEnumerator Bump()
    {
        m_InteractKey.SetActive(false);
        m_InMotion = true;

        float elapsed = 0f;
        float halfDuration = m_BumpDuration / 2f;

        Vector3 initialPosition = transform.position;
        Vector3 maxPosition = initialPosition + transform.TransformDirection(Vector3.forward) * m_BumpDistance;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;

            transform.position = Vector3.Lerp(initialPosition, maxPosition, elapsed / halfDuration);

            yield return null;
        }

        while (elapsed < m_BumpDuration)
        {
            elapsed += Time.deltaTime;
            if (elapsed > m_BumpDuration)
                elapsed = m_BumpDuration;

            transform.position = Vector3.Lerp(maxPosition, initialPosition, (elapsed - halfDuration) / halfDuration);

            yield return null;
        }

        m_InMotion = false;
        CheckInteract();
    }

    private void CheckInteract()
    {
        m_CanInteract = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out m_LatestCheckedInteraction, m_RaycastDistance, m_InteractableLayers);
        m_InteractKey.SetActive(m_CanInteract);
    }
}
