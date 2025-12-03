using UnityEngine;

public class KeyInteract : Interact
{
    [Header("Scene Info")]
    [SerializeField] private int keyData;

    public override void DoInteraction()
    {
        Inventory.Instance.keys.Add(keyData);
        Destroy(gameObject);
    }
}
