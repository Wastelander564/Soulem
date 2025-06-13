using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SavedDeck
{
    public List<int> cardIDs = new();
}

public class DeckManager : MonoBehaviour
{
    [SerializeField] private int maxDeckSize = 10;
    [SerializeField] private List<SoulCards> currentDeck = new();
    [SerializeField] private List<DeckSlot> deckSlots = new();
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private SoulCardDatabase cardDatabase;

    private const string SaveKey = "SavedDeck";

    private void Start()
    {
        LoadDeck();
        UpdateDeckUI();
        ApplyAllCardEffects();
    }

    public bool AddCardToDeck(SoulCards card)
    {
        if (currentDeck.Count >= maxDeckSize)
        {
            Debug.LogWarning("Deck is full. Cannot add more cards.");
            return false;
        }

        currentDeck.Add(card);
        ApplyCardEffects(card);
        UpdateDeckUI();
        // Removed SaveDeck() here
        return true;
    }

    public void RemoveCardFromDeck(SoulCards card)
    {
        if (currentDeck.Remove(card))
        {
            RemoveCardEffects(card);
            UpdateDeckUI();
            // Removed SaveDeck() here
        }
        else
        {
            Debug.LogWarning("Card to remove was not found in deck.");
        }
    }

    public int CountCardInDeck(SoulCards card)
    {
        return currentDeck.Count(c => c == card);
    }

    private void UpdateDeckUI()
    {
        for (int i = 0; i < deckSlots.Count; i++)
        {
            if (i < currentDeck.Count)
                deckSlots[i].SetCard(currentDeck[i]);
            else
                deckSlots[i].ClearSlot();
        }
    }

    private void ApplyCardEffects(SoulCards card)
    {
        foreach (var effect in card.effects)
        {
            effect.Apply(playerStats, 1);
        }
    }

    private void RemoveCardEffects(SoulCards card)
    {
        foreach (var effect in card.effects)
        {
            effect.Remove(playerStats, 1);
        }
    }

    private void ApplyAllCardEffects()
    {
        foreach (var card in currentDeck)
            ApplyCardEffects(card);
    }

    // **This is the method you call on your Save button**
    public void SaveDeck()
    {
        SavedDeck saveData = new SavedDeck
        {
            cardIDs = currentDeck.Select(c => c.ID).ToList()
        };

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Deck saved.");
    }

    public void LoadDeck()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            Debug.Log("No saved deck found.");
            return;
        }

        // Remove old effects before clearing deck
        foreach (var card in currentDeck)
            RemoveCardEffects(card);

        currentDeck.Clear();

        string json = PlayerPrefs.GetString(SaveKey);
        SavedDeck loadedData = JsonUtility.FromJson<SavedDeck>(json);

        foreach (int cardID in loadedData.cardIDs)
        {
            SoulCards card = cardDatabase.allSoulCards.Find(c => c.ID == cardID);
            if (card != null)
                currentDeck.Add(card);
            else
                Debug.LogWarning($"Card with ID '{cardID}' not found in database.");
        }

        UpdateDeckUI();
        ApplyAllCardEffects();
        Debug.Log("Deck loaded.");
    }

    public void ClearDeck()
    {
        foreach (var card in currentDeck)
            RemoveCardEffects(card);

        currentDeck.Clear();
        UpdateDeckUI();
        // Removed SaveDeck() here (save must be manual)
    }

    public List<SoulCards> GetCurrentDeck()
    {
        return new List<SoulCards>(currentDeck);
    }
}
