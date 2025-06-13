using UnityEngine;
using System.Collections.Generic;

public class PersistentCanvas : MonoBehaviour
{
    [Tooltip("Unique identifier for this canvas type (e.g. MainMenu, HUD)")]
    public string canvasID;

    private static Dictionary<string, GameObject> registeredCanvases = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (string.IsNullOrEmpty(canvasID))
        {
            Debug.LogWarning($"[PersistentCanvas] '{gameObject.name}' has no canvasID assigned!");
            return;
        }

        if (registeredCanvases.TryGetValue(canvasID, out GameObject existingCanvas))
        {
            // If this is a duplicate (not the same GameObject), destroy it
            if (existingCanvas != gameObject)
            {
                Debug.Log($"[PersistentCanvas] Duplicate '{canvasID}' found. Destroying new instance.");
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            registeredCanvases[canvasID] = gameObject;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"[PersistentCanvas] Registered persistent canvas: {canvasID}");
        }
    }

    void OnDestroy()
    {
        if (registeredCanvases.TryGetValue(canvasID, out GameObject canvas) && canvas == gameObject)
        {
            registeredCanvases.Remove(canvasID);
            Debug.Log($"[PersistentCanvas] Unregistered canvas: {canvasID}");
        }
    }

    public static void ClearRegistry()
    {
        registeredCanvases.Clear();
        Debug.Log("[PersistentCanvas] Registry cleared.");
    }

    public static bool IsRegistered(string id)
    {
        return registeredCanvases.ContainsKey(id);
    }

    public static GameObject GetRegisteredCanvas(string id)
    {
        return registeredCanvases.ContainsKey(id) ? registeredCanvases[id] : null;
    }
}
