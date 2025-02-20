using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UpgradeOptionUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button upgradeButton;

    private UpgradeOption currentUpgrade;
    private UpgradeUIManager upgradeUIManager;

    private void Awake()
    {
        upgradeUIManager = GetComponentInParent<UpgradeUIManager>();
        upgradeButton.onClick.AddListener(OnUpgradeSelected);
    }

    public void SetupUpgrade(UpgradeOption upgrade)
    {
        currentUpgrade = upgrade;
        iconImage.sprite = upgrade.Icon;
        titleText.text = upgrade.Title;
        descriptionText.text = upgrade.Description;
    }

    private void OnUpgradeSelected()
    {
        if (currentUpgrade != null)
        {
            upgradeUIManager.SelectUpgrade(currentUpgrade);
        }
    }
}