using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Damage Taken")]
    public float damageTakenMultiplier = 1f;

    [Header("Movement")]
    public float baseMoveSpeedValue = 5f;
    public float moveSpeedMultiplier = 1f;

    [Header("Damage Dealt")]
    public float baseDamageToEnemiesValue = 10f;
    public float damageMultiplier = 1f;

    [Header("Health")]
    public float baseMaxHp = 100f;
    public float HpMultiplier = 1f;

    [Header("Souls")]
    public float Souls;
    public float SoulGainMultiplier = 1f;
    public float SoulDropChance = 100f;

    public float MoveSpeed => baseMoveSpeedValue * moveSpeedMultiplier;
    public float DamageToEnemies => baseDamageToEnemiesValue * damageMultiplier;
    public float MaxHp => baseMaxHp * HpMultiplier;
    public float SoulGain => Souls * SoulGainMultiplier;

    private const string SoulsKey = "PlayerSouls";
    private Dictionary<CardEffect, int> appliedEffects = new();

    private void Start()
    {
        Souls = PlayerPrefs.GetFloat(SoulsKey, 0f);
        Debug.Log($"Loaded Souls: {Souls}");
    }

    public void ApplyCard(SoulCards card)
    {
        foreach (var effect in card.effects)
        {
            if (!appliedEffects.ContainsKey(effect))
                appliedEffects[effect] = 0;

            appliedEffects[effect]++;
            effect.Apply(this, appliedEffects[effect]);
        }
    }

    public void RemoveCard(SoulCards card)
    {
        foreach (var effect in card.effects)
        {
            if (!appliedEffects.ContainsKey(effect)) continue;

            effect.Remove(this, appliedEffects[effect]);
            appliedEffects[effect]--;

            if (appliedEffects[effect] <= 0)
                appliedEffects.Remove(effect);
        }
    }

    public void TryAddSouls(int baseAmount)
    {
        float roll = Random.Range(0f, 100f);
        if (roll <= SoulDropChance)
        {
            int gained = Mathf.RoundToInt(baseAmount * SoulGainMultiplier);
            Souls += gained;

            PlayerPrefs.SetFloat(SoulsKey, Souls);
            PlayerPrefs.Save();

            Debug.Log($"Gained {gained} soul(s)! Total: {Souls}");
        }
        else
        {
            Debug.Log("No souls gained. Failed drop chance.");
        }
    }

    public void ResetSouls()
    {
        Souls = 0f;
        PlayerPrefs.DeleteKey(SoulsKey);
        PlayerPrefs.Save();
        Debug.Log("Souls have been reset.");
    }
}
