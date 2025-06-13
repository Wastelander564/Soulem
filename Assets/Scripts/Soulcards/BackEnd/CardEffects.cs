using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Apply(PlayerStats player, int stackCount);
    public abstract void Remove(PlayerStats player, int stackCount);
}

