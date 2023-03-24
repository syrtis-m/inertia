using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewSceneButton : MonoBehaviour
{
    private Button button;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ToNewScene(index));
    }

    void ToNewScene(int index)
    {
        Mind.instance.StartGame(index);
    }
}
