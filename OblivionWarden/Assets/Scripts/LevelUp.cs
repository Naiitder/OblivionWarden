using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UpgradeUIManager.instance.ShowUpgradeOptions();
            Destroy(gameObject);
        }
    }

}
