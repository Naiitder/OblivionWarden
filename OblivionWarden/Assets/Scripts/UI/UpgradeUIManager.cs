using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UpgradeUIManager : MonoBehaviour
{
    public static UpgradeUIManager instance;

    [Header("UI References")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private UpgradeOptionUI[] upgradeOptions;

    [Header("Dependencies")]
    [SerializeField] private PlayerUpgradeManager upgradeManager;

    [Header("Upgrade Icons")]
    [SerializeField] private Sprite damageIcon;
    [SerializeField] private Sprite healthIcon;
    [SerializeField] private Sprite attackSpeedIcon;
    [SerializeField] private Sprite projectileIcon;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        upgradePanel.SetActive(false);
    }

    public void ShowUpgradeOptions()
    {

        List<UpgradeOption> availableUpgrades = GenerateAvailableUpgrades();

        if (availableUpgrades.Count == 0)
        {
            return;
        }

        upgradePanel.SetActive(true);
        Time.timeScale = 0; 

        List<UpgradeOption> selectedUpgrades = SelectRandomUpgrades(availableUpgrades,
            Mathf.Min(3, availableUpgrades.Count));

        for (int i = 0; i < upgradeOptions.Length; i++)
        {
            if (i < selectedUpgrades.Count)
            {
                upgradeOptions[i].gameObject.SetActive(true);
                upgradeOptions[i].SetupUpgrade(selectedUpgrades[i]);
            }
            else
            {
                upgradeOptions[i].gameObject.SetActive(false);
            }
        }
    }

    private List<UpgradeOption> GenerateAvailableUpgrades()
    {
        List<UpgradeOption> availableUpgrades = new List<UpgradeOption>();

        if (upgradeManager.CanUpgrade(UpgradeType.Damage))
        {
            availableUpgrades.Add(new UpgradeOption
            {
                Type = UpgradeType.Damage,
                Title = "Daño",
                Description = "Aumenta el daño base en 5",
                Icon = damageIcon

            });

        }

        if (upgradeManager.CanUpgrade(UpgradeType.Health))
        {
            availableUpgrades.Add(new UpgradeOption
            {
                Type = UpgradeType.Health,
                Title = "Vida",
                Description = "Aumenta la vida máxima en 20",
                Icon = healthIcon
            });
        }

        if (upgradeManager.CanUpgrade(UpgradeType.AttackSpeed))
        {
            availableUpgrades.Add(new UpgradeOption
            {
                Type = UpgradeType.AttackSpeed,
                Title = "Cadencia",
                Description = "Reduce el tiempo entre ataques en 0.2s",
                Icon = attackSpeedIcon
            });
        }

        if (upgradeManager.CanUpgrade(UpgradeType.ProjectileCount))
        {
            availableUpgrades.Add(new UpgradeOption
            {
                Type = UpgradeType.ProjectileCount,
                Title = "Proyectiles",
                Description = "Añade un proyectil adicional",
                Icon = projectileIcon
            });
        }

        return availableUpgrades;
    }

    private List<UpgradeOption> SelectRandomUpgrades(List<UpgradeOption> availableUpgrades, int count)
    {
        return availableUpgrades.OrderBy(x => Random.value).Take(count).ToList();
    }

    public void SelectUpgrade(UpgradeOption upgrade)
    {
        upgradeManager.ApplyUpgrade(upgrade.Type);
        CloseUpgradePanel();
    }

    public void CloseUpgradePanel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1; 
    }
}

[System.Serializable]
public class UpgradeOption
{
    public UpgradeType Type;
    public string Title;
    public string Description;
    public Sprite Icon;
}
