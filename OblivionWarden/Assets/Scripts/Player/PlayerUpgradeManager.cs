using UnityEngine;
using System;

[System.Serializable]
public class UpgradeStats
{
    public int damageLevel;
    public int healthLevel;
    public int attackSpeedLevel;
    public int projectileCountLevel;

    public const int MAX_UPGRADE_LEVEL = 5;
}

public class PlayerUpgradeManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public PlayerStats playerStats;
    public UpgradeStats upgradeStats;

    [Header("Upgrade Multipliers")]
    [SerializeField] private float damageIncreasePerLevel = 5f;
    [SerializeField] private float healthIncreasePerLevel = 20f;
    [SerializeField] private float attackSpeedDecreasePerLevel = 0.2f;
    [SerializeField] private int projectilesIncreasePerLevel = 1;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        upgradeStats = new UpgradeStats();
    }

    public bool CanUpgrade(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Damage:
                return upgradeStats.damageLevel < UpgradeStats.MAX_UPGRADE_LEVEL;
            case UpgradeType.Health:
                return upgradeStats.healthLevel < UpgradeStats.MAX_UPGRADE_LEVEL;
            case UpgradeType.AttackSpeed:
                return upgradeStats.attackSpeedLevel < UpgradeStats.MAX_UPGRADE_LEVEL;
            case UpgradeType.ProjectileCount:
                return upgradeStats.projectileCountLevel < UpgradeStats.MAX_UPGRADE_LEVEL;
            default:
                return false;
        }
    }

    public void ApplyUpgrade(UpgradeType upgradeType)
    {
        if (!CanUpgrade(upgradeType)) return;

        switch (upgradeType)
        {
            case UpgradeType.Damage:
                upgradeStats.damageLevel++;
                playerStats.Dmg += Mathf.RoundToInt(damageIncreasePerLevel);
                break;

            case UpgradeType.Health:
                upgradeStats.healthLevel++;
                playerStats.MaxHealth += Mathf.RoundToInt(healthIncreasePerLevel);
                playerStats.CurrentHealth += Mathf.RoundToInt(healthIncreasePerLevel);
                break;

            case UpgradeType.AttackSpeed:
                upgradeStats.attackSpeedLevel++;
                playerManager.UpdateAttackInterval(-attackSpeedDecreasePerLevel);
                break;

            case UpgradeType.ProjectileCount:
                upgradeStats.projectileCountLevel++;
                playerManager.UpdateProjectileCount(projectilesIncreasePerLevel);
                break;
        }

        OnUpgradeApplied?.Invoke(upgradeType);
    }

    public event Action<UpgradeType> OnUpgradeApplied;
}

public enum UpgradeType
{
    Damage,
    Health,
    AttackSpeed,
    ProjectileCount
}