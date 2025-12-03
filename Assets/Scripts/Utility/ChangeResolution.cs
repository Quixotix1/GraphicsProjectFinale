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
    }
}

public class MaterialSwitcher : MonoBehaviour
{
    public Material switchMaterial;

    private readonly Hashtable _matList = new();

    private Renderer[] _renderers;

    private bool switched;

    /// <summary>
    /// Record all material of the gameobject and its children
    /// </summary>
    void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer _renderer in _renderers)
        {
            _matList.Add(_renderer, _renderer.material);
        }
    }

    /// <summary>
    /// just for testing
    /// </summary>
    public void Update()
    {

        if (Input.GetButtonDown("Toggle Debug"))
        {
            ToggleMaterial();
        }
    }

    /// <summary>
    /// Toggle between the switch material and the default.
    /// </summary>
    public void ToggleMaterial()
    {
        switched = !switched;
        if (switched)
        {
            SwitchMaterial();
        }
        else
        {
            ResetMaterial();
        }
    }
    /// <summary>
    /// Revert to the default material
    /// </summary>
    public void ResetMaterial()
    {
        switched = false;
        foreach (Renderer _renderer in _renderers)
        {
            _renderer.material = _matList[_renderer] as Material;
        }
    }

    /// <summary>
    /// Switch to the predefined switchMaterial
    /// </summary>
    public void SwitchMaterial()
    {
        switched = true;
        SetMaterial(switchMaterial); 
    }

    /// <summary>
    /// If you want to assign an arbitrary material
    /// </summary>
    /// <param name="mat">
    /// A <see cref="Material"/>
    /// </param>
    public void SetMaterial(Material mat)
    {
        switched = true;
        if (mat == null)
        {
            Debug.LogWarning("no Material defined to set on GameObject: " + name);
            return;
        }
        foreach (Renderer _renderer in _renderers)
        {
            _renderer.material = mat;
        }
    }

}
