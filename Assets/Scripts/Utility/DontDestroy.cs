using UnityEngine;
using UnityEngine.Device;

public class DontDestroy : MonoBehaviour
{
    [SerializeField] private string m_TagName;

    private void Awake()
    {

        if (GameObject.FindGameObjectWithTag(m_TagName) != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}
