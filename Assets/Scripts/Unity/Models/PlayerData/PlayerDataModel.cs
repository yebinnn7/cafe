using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using Unity.VisualScripting;

/**
 * ACTION REQUIRED: Remodeling below data model is required.
 */
[System.Serializable]
[ProtoContract]
public class PlayerDataModel
{
    [ProtoMember(1)]
    public int jelatin;
    [ProtoMember(2)]
    public int gold;
    [ProtoMember(3, OverwriteList=true)]
    public bool[] jellyUnlocks;
    [ProtoMember(4, OverwriteList=true)]
    public List<DataMigrated> jelly;
    [ProtoMember(5)]
    public int numLevel;
    [ProtoMember(6)]
    public int clickLevel;
    [UnityEngine.Range(PlayerDataModelDefaults.VOLUME_MIN, PlayerDataModelDefaults.VOLUME_MAX)]
    [ProtoMember(7)]
    public float bgmVolume;
    [UnityEngine.Range(PlayerDataModelDefaults.VOLUME_MIN, PlayerDataModelDefaults.VOLUME_MAX)]
    [ProtoMember(8)]
    public float sfxVolume;
    [ProtoMember(9, OverwriteList=true)]
    public int[] machine_level = PlayerDataModelDefaults.MACHINE_LEVEL;
    [ProtoMember(10, OverwriteList=true)]
    public string[] customerUnlocked = PlayerDataModelDefaults.CUSTOMER_UNLOCKED;
    [ProtoMember(11)]
    public int customerUnlockedCount;

    public PlayerDataModel
    (
        int jelatin,
        int gold,
        bool[] jellyUnlocks,
        IEnumerable<Data> jelly,
        int numLevel,
        int clickLevel,
        float bgmVolume,
        float sfxVolume,
        int[] machine_level,
        string[] customerUnlocked,
        int customerUnlockedCount
    )
    {
        this.jelatin = jelatin;
        this.gold = gold;
        this.jellyUnlocks = (bool[])jellyUnlocks.Clone();
        this.jelly = jelly.Select(each => (DataMigrated)each).ToList();
        this.numLevel = numLevel;
        this.clickLevel = clickLevel;
        this.bgmVolume = bgmVolume;
        this.sfxVolume = sfxVolume;
        this.machine_level = machine_level;
        for (int i = 0; i < Math.Min(PlayerDataModelDefaults.MACHINE_LENGTH, machine_level.Length); i++)
        {
            this.machine_level[i] = machine_level[i];
        }
        for (int i = 0; i < Math.Min(PlayerDataModelDefaults.CUSTOMER_LENGTH, customerUnlocked.Length); i++)
        {
            this.customerUnlocked[i] = customerUnlocked[i];
        }
        this.customerUnlockedCount = customerUnlocked.Length;
    }

    public PlayerDataModel()
    {
        this.jelatin = PlayerDataModelDefaults.JELATIN;
        this.gold = PlayerDataModelDefaults.GOLD;
        this.jellyUnlocks = (bool[])PlayerDataModelDefaults.JELLY_UNLOCKS.Clone();
        this.jelly = PlayerDataModelDefaults.JELLY.Select(data => (DataMigrated)data).ToList();
        this.numLevel = PlayerDataModelDefaults.NUM_LEVEL;
        this.clickLevel = PlayerDataModelDefaults.CLICK_LEVEL;
        this.bgmVolume = PlayerDataModelDefaults.BGM_VOLUME;
        this.sfxVolume = PlayerDataModelDefaults.SFX_VOLUME;
        this.machine_level = PlayerDataModelDefaults.MACHINE_LEVEL;
        this.customerUnlocked = PlayerDataModelDefaults.CUSTOMER_UNLOCKED;
        this.customerUnlockedCount = PlayerDataModelDefaults.CUSTOMER_UNLOCKED_COUNT;
    }
}
