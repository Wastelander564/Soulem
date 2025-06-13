using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGiver : MonoBehaviour
{
    public CardCollection playerCollection;
    public SoulCards cardToGive;

    void Start()
    {
        // Give the player this card once at the beginning
        playerCollection.AddCard(cardToGive);
    }
}

