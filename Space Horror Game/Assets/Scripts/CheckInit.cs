using UnityEngine;

public class CheckInit : MonoBehaviour
{
    private static bool LoadedSceneZero = false;
    private void Awake()
    {
        if (LoadedSceneZero) Destroy(gameObject);
        else if (gameObject.scene.buildIndex != 0) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        else LoadedSceneZero = true;
    }
}