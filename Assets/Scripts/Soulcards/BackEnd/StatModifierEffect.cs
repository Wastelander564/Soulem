using UnityEngine;

[CreateAssetMenu(fileName = "StatModifierEffect", menuName = "Cards/Effects/Stat Modifier")]
public class StatModifierEffect : CardEffect
{
    public enum AffectedStat
    {
        DamageDealt,
        MoveSpeed,
        DamageTaken,
        MaxHp
    }

    public AffectedStat targetStat;
    public float valuePerStack = 0.1f;

    public override void Apply(PlayerStats player, int stackCount)
    {
        float amount = valuePerStack * stackCount;

        switch (targetStat)
        {
            case AffectedStat.DamageDealt:
                player.damageMultiplier += amount;
                break;
            case AffectedStat.MoveSpeed:
                player.moveSpeedMultiplier += amount;
                break;
            case AffectedStat.DamageTaken:
                player.damageTakenMultiplier += amount;
                break;
            case AffectedStat.MaxHp:
                player.HpMultiplier += amount;
                break;
        }
    }

    public override void Remove(PlayerStats player, int stackCount)
    {
        float amount = valuePerStack * stackCount;

        switch (targetStat)
        {
            case AffectedStat.DamageDealt:
                player.damageMultiplier -= amount;
                break;
            case AffectedStat.MoveSpeed:
                player.moveSpeedMultiplier -= amount;
                break;
            case AffectedStat.DamageTaken:
                player.damageTakenMultiplier -= amount;
                break;
            case AffectedStat.MaxHp:
                player.HpMultiplier -= amount;
                break;
        }
    }
}
