using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string portalTextObjectName = "PortalText";       // The name of your text object in the scene
    [SerializeField] private string portalWithSaveObjectName = "PortalWithSave"; // The name of the "continue available" UI object
    [SerializeField] private string roundCountKey = "RoundCount";
    [SerializeField] private string arenaSceneName = "Arena";

    private TextMeshProUGUI messageText;
    private GameObject portalWithSave;
    private bool isTrigger = false;

    private void Start()
    {
        // Find the objects at runtime
        GameObject textObj = GameObject.Find(portalTextObjectName);
        if (textObj != null)
            messageText = textObj.GetComponent<TextMeshProUGUI>();

        portalWithSave = GameObject.Find(portalWithSaveObjectName);

        // Hide both at the start
        if (messageText != null)
            messageText.gameObject.SetActive(false);

        if (portalWithSave != null)
            portalWithSave.SetActive(false);
    }

    private void Update()
    {
        if (isTrigger && Input.GetKeyDown(KeyCode.Return) && !PlayerPrefs.HasKey(roundCountKey))
        {
            Switch();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTrigger = true;

            if (messageText != null)
                messageText.gameObject.SetActive(true);

            if (PlayerPrefs.HasKey(roundCountKey))
            {
                int savedRound = PlayerPrefs.GetInt(roundCountKey);
                Debug.Log("Player entered portal with saved round count: " + savedRound);

                if (portalWithSave != null)
                    portalWithSave.SetActive(true);
            }
            else
            {
                Debug.Log("Player entered portal but no round count saved.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTrigger = false;

            if (messageText != null)
                messageText.gameObject.SetActive(false);

            if (portalWithSave != null)
                portalWithSave.SetActive(false);
        }
    }

    private void Switch()
    {
        SceneManager.LoadScene(arenaSceneName);
    }

    public void NewRounds()
    {
        PlayerPrefs.DeleteKey(roundCountKey);
        Switch();
    }

    public void Continue()
    {
        Switch();
    }
}
