using UnityEngine;
using UnityEngine.UI;

public class DeckCardControls : MonoBehaviour
{
    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private CardDisplay cardDisplay;

    private SoulCards card;
    private DeckManager deckManager;
    private int ownedCount;

    public void Initialize(SoulCards assignedCard, int owned, CardRarityAssets rarityAssets, DeckManager manager)
    {
        card = assignedCard;
        ownedCount = owned;
        deckManager = manager;

        cardDisplay.Setup(card, ownedCount, rarityAssets);
        UpdateButtonInteractivity();

        addButton.onClick.RemoveAllListeners();
        addButton.onClick.AddListener(() =>
        {
            if (deckManager.CountCardInDeck(card) < ownedCount)
            {
                deckManager.AddCardToDeck(card);
                UpdateButtonInteractivity();
            }
        });

        removeButton.onClick.RemoveAllListeners();
        removeButton.onClick.AddListener(() =>
        {
            if (deckManager.CountCardInDeck(card) > 0)
            {
                deckManager.RemoveCardFromDeck(card);
                UpdateButtonInteractivity();
            }
        });
    }

    private void UpdateButtonInteractivity()
    {
        int inDeck = deckManager.CountCardInDeck(card);
        addButton.interactable = inDeck < ownedCount;
        removeButton.interactable = inDeck > 0;
    }
}