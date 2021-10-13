using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonMoveToScene : MonoBehaviour
{
    public Tymski.SceneReference scene;
    Button b;
    void Start()
    {
        b = GetComponent<Button>();
        b.onClick.AddListener(MoveToScene);
    }
    void MoveToScene() => SceneManager.LoadScene(scene.ScenePath);
}