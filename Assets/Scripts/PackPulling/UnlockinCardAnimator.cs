using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockingCardAnimator : MonoBehaviour
{
    [Header("References")]
    public CardDisplay cardDisplayPrefab;   // Card UI prefab with nameText and effectText
    public Transform cardParent;            // Parent inside the panel on canvas
    public Canvas unlockingCanvas;          // Canvas GameObject (set inactive in inspector)
    public Button closeButton;              // Close button on the canvas
    public CardRarityAssets rarityAssets;   // Rarity assets ScriptableObject

    private CardDisplay currentCardDisplay;

    void Start()
    {
        unlockingCanvas.gameObject.SetActive(false);
    }

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseUnlockingCanvas);

        if (unlockingCanvas != null && unlockingCanvas.gameObject.scene.name != null)
        {
            DontDestroyOnLoad(unlockingCanvas.gameObject);
            Debug.Log($"{unlockingCanvas.gameObject.name} is now persistent.");
        }

        unlockingCanvas.gameObject.SetActive(false);
    }


    public void ShowUnlockingAnimation(SoulCards card)
    {
        // Pause game time
        Time.timeScale = 0f;

        unlockingCanvas.gameObject.SetActive(true);

        if (currentCardDisplay != null)
        {
            Destroy(currentCardDisplay.gameObject);
        }

        currentCardDisplay = Instantiate(cardDisplayPrefab, cardParent);

        // Set up card UI with 0 owned count to trigger silhouette mode
        currentCardDisplay.Setup(card, 0, rarityAssets);

        // Set text colors to white for dramatic reveal
        currentCardDisplay.nameText.color = Color.white;
        currentCardDisplay.effectText.color = Color.white;

        // Make silhouette overlay black
        currentCardDisplay.silhouetteOverlay.SetActive(true);
        Image silhouetteImage = currentCardDisplay.silhouetteOverlay.GetComponent<Image>();
        if (silhouetteImage != null)
        {
            silhouetteImage.color = Color.black;
        }

        // Hide close button until animation finishes
        closeButton.gameObject.SetActive(false);

        StartCoroutine(UnlockAnimationCoroutine(card));
    }

    private IEnumerator UnlockAnimationCoroutine(SoulCards card)
    {
        yield return new WaitForSecondsRealtime(2f);

        Color rarityColor = rarityAssets.GetColorForRarity(card.rarity);

        Image silhouetteImage = currentCardDisplay.silhouetteOverlay.GetComponent<Image>();

        // Fade out silhouette overlay
        float fadeDuration = 1.5f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            if (silhouetteImage != null)
            {
                Color c = silhouetteImage.color;
                c.a = alpha;
                silhouetteImage.color = c;
            }
            yield return null;
        }

        // Hide silhouette completely
        currentCardDisplay.silhouetteOverlay.SetActive(false);

        // 🟢 Reveal the card's real name and description
        currentCardDisplay.Reveal();

        // Show close button
        closeButton.gameObject.SetActive(true);

        // Resume time for interaction
        Time.timeScale = 1f;
    }

    private void CloseUnlockingCanvas()
    {
        unlockingCanvas.gameObject.SetActive(false);

        // Resume time in case user skips early
        Time.timeScale = 1f;
    }
}
