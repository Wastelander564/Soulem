using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCatalogUI : MonoBehaviour
{
    public CardDisplay cardDisplayPrefab;
    public Transform container;
    public CardRarityAssets rarityAssets;
    public CardCollection playerCollection;
    public SoulCardDatabase allCards;

    void Start()
    {
        // Clear existing UI elements (optional if needed)
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // Instantiate and set up each card in the catalog
        foreach (var card in allCards.allSoulCards)
        {
            var display = Instantiate(cardDisplayPrefab, container);
            int owned = playerCollection.GetCardCount(card);  // Get owned count from player collection
            display.Setup(card, owned, rarityAssets);  // Set up card display
        }
    }
    public void RefreshCatalog()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        foreach (var card in allCards.allSoulCards)
        {
            var display = Instantiate(cardDisplayPrefab, container);
            int owned = playerCollection.GetCardCount(card);
            display.Setup(card, owned, rarityAssets);
        }
    }

}
