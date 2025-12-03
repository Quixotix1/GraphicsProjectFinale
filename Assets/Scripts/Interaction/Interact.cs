using System.Collections;
using UnityEngine;

public abstract class Interact : MonoBehaviour
{
    public bool requiresKey;
    public int key;
    [SerializeField] private GameObject m_Lock;

    public abstract void DoInteraction();

    public void ToggleLock()
    {
        StartCoroutine(ShowLock());
    }

    IEnumerator ShowLock()
    {
        m_Lock.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        m_Lock.SetActive(false);
    }
}
