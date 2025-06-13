using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoulCardDatabase", menuName = "SoulCards/Database")]
public class SoulCardDatabase : ScriptableObject
{
    public List<SoulCards> allSoulCards;
}

