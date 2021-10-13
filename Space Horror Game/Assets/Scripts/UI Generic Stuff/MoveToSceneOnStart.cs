using UnityEngine;
public class MoveToSceneOnStart : MonoBehaviour
{
    public Tymski.SceneReference scene;
    void Start() => UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ScenePath);
}