using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    // 특정 씬으로 이동하는 메서드
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
