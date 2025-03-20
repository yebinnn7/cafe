using UnityEngine;

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
