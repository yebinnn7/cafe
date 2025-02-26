using UnityEngine;
using UnityEditor;

public class PlayerDataContainerDebugger : MonoBehaviour
{
    [SerializeField] public PlayerDataModel playerDataModel;

    void Awake()
    {
        playerDataModel = PlayerDataContainer.I.PlayerData;
    }

    void Start()
    {
        Debug.Log("PlayerDataContainerDebugger Start()");
    }
}

[CustomEditor(typeof(PlayerDataContainerDebugger))]
public class PlayerDataContainerDebuggerCustomEditor : Editor
{
    PlayerDataContainerDebugger debuggerObject;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        debuggerObject = (PlayerDataContainerDebugger)target;
        if (GUILayout.Button("Pull from container"))
        {
            debuggerObject.playerDataModel = PlayerDataContainer.I.PlayerData;
        }
        if (GUILayout.Button("Push to container"))
        {
            PlayerDataContainer.I.PlayerData = debuggerObject.playerDataModel;
        }
        if (GUILayout.Button("PlayerDataContainer.I.PushDataToLocal()"))
        {
            PlayerDataContainer.I.PushDataToLocal();
        }
        if (GUILayout.Button("PlayerDataContainer.I.PullDataFromLocal()"))
        {
            PlayerDataContainer.I.PullDataFromLocal();
        }
        if (GUILayout.Button("Pull from container after load"))
        {
            PlayerDataContainer.I.PullDataFromLocal();
            debuggerObject.playerDataModel = PlayerDataContainer.I.PlayerData;
        }
        if (GUILayout.Button("Push to container and save"))
        {
            PlayerDataContainer.I.PlayerData = debuggerObject.playerDataModel;
            PlayerDataContainer.I.PushDataToLocal();
        }
        if (GUILayout.Button("Dump GameManager and sync"))
        {
            GameManager.instance.PushAndSavePlayerData();
            debuggerObject.playerDataModel = PlayerDataContainer.I.PlayerData;
        }
        if (GUILayout.Button("Reload GameManager and sync"))
        {
            GameManager.instance.LoadAndPullPlayerData();
            debuggerObject.playerDataModel = PlayerDataContainer.I.PlayerData;
        }
        if (GUILayout.Button("Apply to GameManager"))
        {
            PlayerDataContainer.I.PlayerData = debuggerObject.playerDataModel;
            GameManager.instance.LoadAndPullPlayerData();
        }
    }
}
