using UnityEngine;

public class PluginTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyPlugin.ID = 10;
        Debug.Log(MyPlugin.ID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
