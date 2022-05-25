using UnityEngine;
using UnityEngine.UI;

public class InputFrame : MonoBehaviour
{
    InputField inputField;

    void Start()
    {
        inputField = GetComponent<InputField>();

        // 监听输入事件
        inputField.onValueChanged.AddListener((string integer) =>
        { 
            if (int.Parse(integer) < 0)
                inputField.text = "0";
            else if (int.Parse(integer) > 29)
                inputField.text = "29";
        });
    }
}