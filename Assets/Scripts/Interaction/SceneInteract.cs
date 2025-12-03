using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInteract : Interact
{
    [Header("Scene Info")]
    [Tooltip("Scene integer according to build order.")] [SerializeField] private int m_Scene; 
    [Tooltip("Scene integer according to build order.")] [SerializeField] private Vector3 m_ExitPos; 

    public override void DoInteraction()
    {
        SceneSwitcher.Instance.SwitchScene(m_Scene, m_ExitPos);
    }
}
