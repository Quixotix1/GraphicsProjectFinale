using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance { get; private set; }
    [SerializeField] private Material m_Screen;
    [SerializeField] private float m_TransitionTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        m_Screen.color = new Color(0, 0, 0, 0);
    }

    public void SwitchScene(int scene, Vector3 exitPos)
    {
        StartCoroutine(TransitionToScene(scene, exitPos));
    }

    IEnumerator TransitionToScene(int scene, Vector3 exitPos)
    {
        float elapsed = 0f;
        while (elapsed < m_TransitionTime)
        {
            elapsed += Time.deltaTime;
            if (elapsed > m_TransitionTime) elapsed = m_TransitionTime;

            m_Screen.color = new Color(0, 0, 0, elapsed / m_TransitionTime);

            yield return null;
        }

        SceneManager.LoadScene(scene);

        yield return new WaitForNextFrameUnit();
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = exitPos;

        elapsed = 0f;
        while (elapsed < m_TransitionTime)
        {
            elapsed += Time.deltaTime;
            if (elapsed > m_TransitionTime) elapsed = m_TransitionTime;

            m_Screen.color = new Color(0, 0, 0, 1 - elapsed / m_TransitionTime);

            yield return null;
        }
    }
}
