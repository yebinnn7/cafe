using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Default values for PlayerDataModel
/// (Migrated from database.json and DataManager.cs)
/// </summary>

public class PlayerDataModelDefaults
{
    public const int JELATIN = 0;
    public const int GOLD = 0;
    public static readonly bool[] JELLY_UNLOCKS = new bool[] { true, true, true, false, false, false, false, false, false, false, false, false };
    public static readonly Data[] JELLY = {};
    public const int NUM_LEVEL = 1;
    public const int CLICK_LEVEL = 1;
    public const float VOLUME_MIN = 0.0f;
    public const float VOLUME_MAX = 1.0f;
    public const float BGM_VOLUME = 0.5f;
    public const float SFX_VOLUME = 0.5f;
    public const int MACHINE_LENGTH = 5;
    public static readonly int[] MACHINE_LEVEL = new int[5] { 0, 0, 0, 0, 0 };
    public const int CUSTOMER_LENGTH = 15;
    public static readonly string[] CUSTOMER_UNLOCKED = new string[15] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
    public const int CUSTOMER_UNLOCKED_COUNT = 0;
}
