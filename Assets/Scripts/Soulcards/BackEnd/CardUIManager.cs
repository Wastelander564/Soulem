using UnityEngine;

public class DeckUIManager : MonoBehaviour
{
    public GameObject deckCardPrefab; // Prefab must have DeckCardControls + CardDisplay
    public Transform deckUIParent;
    public SoulCardDatabase database;
    public CardCollection playerCollection;
    public CardRarityAssets rarityAssets;
    
    public DeckManager deckManager;  // <-- Add this reference

    void Start()
    {
        RefreshDeckUI();
    }

    public void RefreshDeckUI()
    {
        // Clear existing deck UI
        foreach (Transform child in deckUIParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate fresh card UI for each card
        foreach (var card in database.allSoulCards)
        {
            int owned = playerCollection.GetCardCount(card);

            GameObject cardGO = Instantiate(deckCardPrefab, deckUIParent);
            DeckCardControls controls = cardGO.GetComponent<DeckCardControls>();
            controls.Initialize(card, owned, rarityAssets, deckManager); // <-- Pass deckManager here
        }
    }
}
