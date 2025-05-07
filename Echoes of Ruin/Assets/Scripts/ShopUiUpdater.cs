using UnityEngine;

public class ShopQuantityManager : MonoBehaviour
{
    // Singleton instance for easy access.
    public static ShopQuantityManager Instance { get; private set; }

    private ItemTrack track;

    // Constants for item IDs (ensure these match other scripts).
    private const int BALL_ID = 1;
    private const int BISCUIT_ID = 2;
    private const int BRUSH_ID = 3;

    // Called once when the script instance is being loaded.
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        track = GetComponent<ItemTrack>();
    }

    // Retrieves the current quantity for a given item ID.
    public int GetItemQuantity(int itemID)
    {
        if (track == null)
        {
            return 0;
        }

        switch (itemID)
        {
            case BALL_ID:
                return track.ball;
            case BISCUIT_ID:
                return track.bisc;
            case BRUSH_ID:
                return track.brush;
            default:
                return 0;
        }
    }
}