using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Rarity { Common, Rare, VeryRare, Epic, Mythical }

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card")]
public class SoulCards : ScriptableObject
{
    public int ID;
    public string cardName;
    [TextArea] public string description;
    public Rarity rarity;
    
    public List<CardEffect> effects;
    public float effectValue; // You can extend this to be more flexible.
}
