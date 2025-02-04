using System.IO;
using UnityEngine;
using ProtoBuf;

public class PlayerDataContainer
{
    [UnityEngine.SerializeField] private PlayerDataModel playerDataModel;
    
    private static PlayerDataContainer instance;

    private PlayerDataContainer()
    {
        this.playerDataModel = new PlayerDataModel();
    }

    public static PlayerDataContainer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerDataContainer();
                instance.PullDataFromLocal();
            }
            return instance;
        }
    }
    public static PlayerDataContainer I => Instance;

    /**
     * Methods associated with Serialization
     * 
     * NOTE: These methods has a string argument `path`.
     *       But do not use as much as possible. It is for generalization and testing purposes.
     *       If you want to change the data path, modify the `SerializationSupportDefaults.PLAYER_DATA_FILE_NAME`.
     */

    public static string _GetSerializationPath(string path = "")
    {
        return string.IsNullOrEmpty(path) ?
            Path.Combine(Application.dataPath, SerializationSupportDefaults.PLAYER_DATA_FILE_NAME) :
            Path.Combine(Application.dataPath, path);
    }

    public PlayerDataModel PlayerData
    {
        get => playerDataModel;
        set => playerDataModel = value;
    }

    public void PushDataToLocal(string path = "")
    {
        path = _GetSerializationPath(path);

        using (FileStream file = File.Create(path))
        {
            Serializer.Serialize(file, playerDataModel);
        }
    }

    public void PullDataFromLocal(string path = "")
    {
        path = _GetSerializationPath(path);

        PlayerDataModel _new;
        try
        {
            using (FileStream file = File.OpenRead(path))
            {
                _new = (PlayerDataModel)Serializer.Deserialize(playerDataModel.GetType(), file);
            }
        }
        catch (FileNotFoundException)
        {
            PushDataToLocal(path);
            return;
        }

        this.playerDataModel.jelatin = _new.jelatin;
        this.playerDataModel.gold = _new.gold;
        this.playerDataModel.jellyUnlocks = _new.jellyUnlocks;
        this.playerDataModel.jelly = _new.jelly;
        this.playerDataModel.numLevel = _new.numLevel;
        this.playerDataModel.clickLevel = _new.clickLevel;
        this.playerDataModel.bgmVolume = _new.bgmVolume;
        this.playerDataModel.sfxVolume = _new.sfxVolume;
    }
}
