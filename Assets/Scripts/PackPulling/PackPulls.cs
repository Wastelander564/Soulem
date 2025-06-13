using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PackPulls : MonoBehaviour
{
    [Header("References")]
    public SoulCardDatabase allCards;
    public CardCollection playerCollection;
    public CardCatalogUI[] catalogUI;
    public DeckUIManager deckUIManager;
    public UnlockingCardAnimator unlockingAnimator;

    private PlayerStats stats;

    private readonly Dictionary<Rarity, float> rarityWeights = new Dictionary<Rarity, float>
    {
        { Rarity.Common, 60f },
        { Rarity.Rare, 25f },
        { Rarity.VeryRare, 10f },
        { Rarity.Epic, 4f },
        { Rarity.Mythical, 1f }
    };

    void Start()
    {
        stats = FindComponentInScene<PlayerStats>();
    }

    public bool CanRefine()
    {
        return stats != null && stats.Souls >= 100;
    }

    public void RefineSouls()
    {
        if (!CanRefine())
        {
            Debug.Log("Cannot refine: not enough souls.");
            return;
        }

        stats.Souls -= 100;
        Debug.Log("Refining souls...");

        SoulCards newCard = DrawWeightedCard();
        if (newCard != null)
        {
            playerCollection.AddCard(newCard);
            Debug.Log($"Refined souls into: {newCard.cardName} ({newCard.rarity})");

            foreach (CardCatalogUI ui in catalogUI)
            {
                ui.RefreshCatalog();
            }

            if (deckUIManager != null)
            {
                deckUIManager.RefreshDeckUI();
            }

            if (unlockingAnimator != null)
            {
                unlockingAnimator.ShowUnlockingAnimation(newCard);
            }
            else
            {
                Debug.LogWarning("Unlocking animator is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning("No card could be drawn.");
        }
    }

    private SoulCards DrawWeightedCard()
    {
        Rarity chosenRarity = GetRandomRarity();
        List<SoulCards> pool = allCards.allSoulCards.Where(c => c.rarity == chosenRarity).ToList();

        if (pool.Count == 0)
        {
            Debug.LogWarning($"No cards available for rarity: {chosenRarity}");
            return null;
        }

        return pool[Random.Range(0, pool.Count)];
    }

    private Rarity GetRandomRarity()
    {
        float totalWeight = rarityWeights.Values.Sum();
        float rand = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var pair in rarityWeights)
        {
            cumulative += pair.Value;
            if (rand <= cumulative)
            {
                return pair.Key;
            }
        }

        return Rarity.Common;
    }

    // Utility to find components in scene
    private T FindComponentInScene<T>() where T : MonoBehaviour
    {
        return FindObjectOfType<T>();
    }
}
