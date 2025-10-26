using System.Collections.Generic;

public static class HealthPickupManager
{
    public static HashSet<string> collectedPickups = new HashSet<string>();

    public static void RegisterPickupCollected(string pickupID)
    {
        if (!string.IsNullOrEmpty(pickupID))
        {
            collectedPickups.Add(pickupID);
        }
    }

    public static bool IsPickupCollected(string pickupID)
    {
        return collectedPickups.Contains(pickupID);
    }

    public static void ResetCollectedPickups()
    {
        collectedPickups.Clear();
    }
}