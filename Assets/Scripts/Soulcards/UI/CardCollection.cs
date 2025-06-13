using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardDataEntry
{
    public string cardName;
    public int count;
}

[System.Serializable]
public class CardSaveWrapper
{
    public List<CardDataEntry> savedCards = new List<CardDataEntry>();
}

public class CardCollection : MonoBehaviour
{
    public SoulCardDatabase cardDatabase;

    private Dictionary<SoulCards, int> ownedCards = new Dictionary<SoulCards, int>();
    private const string SaveKey = "SoulCardCollection";

    private void Start()
    {
        LoadCollection();
    }

    public int GetCardCount(SoulCards card)
    {
        return ownedCards.TryGetValue(card, out int count) ? count : 0;
    }

    public void AddCard(SoulCards card)
    {
        if (ownedCards.ContainsKey(card))
        {
            ownedCards[card]++;
        }
        else
        {
            ownedCards[card] = 1;
        }
        // Removed SaveCollection() here
    }

    public void RemoveCard(SoulCards card)
    {
        if (ownedCards.ContainsKey(card))
        {
            ownedCards[card]--;
            if (ownedCards[card] <= 0)
                ownedCards.Remove(card);
        }
        // Removed SaveCollection() here
    }

    // **Call this method from a Save button**
    public void SaveCollection()
    {
        CardSaveWrapper wrapper = new CardSaveWrapper();

        foreach (var pair in ownedCards)
        {
            wrapper.savedCards.Add(new CardDataEntry
            {
                cardName = pair.Key.cardName,
                count = pair.Value
            });
        }

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();

        Debug.Log("Card collection saved.");
    }

    public void LoadCollection()
    {
        ownedCards.Clear();

        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            CardSaveWrapper wrapper = JsonUtility.FromJson<CardSaveWrapper>(json);

            foreach (CardDataEntry entry in wrapper.savedCards)
            {
                SoulCards card = cardDatabase.allSoulCards.Find(c => c.cardName == entry.cardName);
                if (card != null)
                {
                    ownedCards[card] = entry.count;
                }
                else
                {
                    Debug.LogWarning($"Card not found in database: {entry.cardName}");
                }
            }

            Debug.Log("Card collection loaded.");
        }
    }

    public void ClearCollection()
    {
        ownedCards.Clear();
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();

        Debug.Log("Card collection cleared.");
    }

    public Dictionary<SoulCards, int> GetOwnedCards()
    {
        return ownedCards;
    }
}
