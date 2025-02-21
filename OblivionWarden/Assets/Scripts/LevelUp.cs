using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UpgradeUIManager.instance.ShowUpgradeOptions();
            GameController.instance.PlayAudioClip(GameController.instance.levelUpSound);
            Destroy(gameObject);
        }
    }

}
