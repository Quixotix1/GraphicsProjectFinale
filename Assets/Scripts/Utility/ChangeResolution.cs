using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChangeResolution : MonoBehaviour
{
    [SerializeField] RenderTexture m_FullRes, m_Pixelated;
    [SerializeField] Material debugMat;

    Dictionary<GameObject, Material[]> materialDict = new();

    RawImage m_render;
    bool isPixelated = false;


    private void Start()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
    }

    bool debug = false;
    void Update()
    {
        if (Input.GetButtonDown("Change"))
        {
            gameObject.GetComponent<Camera>().targetTexture = isPixelated ? m_FullRes : m_Pixelated;
            m_render = GameObject.FindGameObjectWithTag("Render").GetComponent<RawImage>();
            m_render.texture = isPixelated ? m_FullRes : m_Pixelated;
            isPixelated = !isPixelated;
        }
        if (Input.GetButtonDown("Toggle Debug"))
        {
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            if (!debug)
            {
                foreach (GameObject obj in allObjects)
                {
                    if (obj.CompareTag("Ignore Debug")) continue;
                    if (obj.TryGetComponent(out Renderer rend))
                    {
                        if (rend.materials != null && !obj.CompareTag("Ignore Debug"))
                        {
                            materialDict.Add(obj, rend.materials);
                            Material[] mats = rend.materials;
                            for (int i = 0; i < mats.Length; i++)
                            {
                                mats[i] = debugMat;
                            }
                            rend.materials = mats;
                        }
                    }
                }
                debug = true;
            }
            else
            {
                foreach (GameObject obj in allObjects)
                {
                    if (obj.CompareTag("Ignore Debug")) continue;
                    if (obj.TryGetComponent(out Renderer rend))
                    {
                        if (rend.materials != null && !obj.CompareTag("Ignore Debug"))
                        {
                            rend.materials = materialDict[obj];
                        }
                    }
                }
                materialDict = new();
                debug = false;
            }
        }
        if (Input.GetButtonDown("Toggle Lighting"))
        {
            Light[] allLights = FindObjectsByType<Light>(FindObjectsSortMode.None);
            foreach (Light light in allLights)
            {
                light.enabled = !light.enabled;
            }
        }
    }
}
