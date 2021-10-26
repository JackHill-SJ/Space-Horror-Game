using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonChangeCanvas : MonoBehaviour
{
    public Canvas To;
    public Canvas From;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangeCanvas);
    }
    private void ChangeCanvas()
    {
        To.gameObject.SetActive(true);
        From.gameObject.SetActive(false);
    }
}