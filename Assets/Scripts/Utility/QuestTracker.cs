using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    bool onLaterQuest = false;
    void Update()
    {
        if (!onLaterQuest)
        {
            if (Inventory.Instance.keys.Count > 0)
            {
                transform.position = new Vector3(8f, 2.5f, -13f);
                onLaterQuest = true;
            }
        }
    }
}
