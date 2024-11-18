using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSelector2D : MonoBehaviour
{

    private void OnMouseDown()
    {
        PlacementController.instance.isFollowingMouse = true; // 마우스 따라다니기 활성화
        Debug.Log("작동하냐?");
    }
}
