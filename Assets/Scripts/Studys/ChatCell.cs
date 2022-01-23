using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

public class ChatCell : MonoBehaviour
{
    public ChatType ChatType { private set; get; }

    public void Setup(ChatType type , Color color , string textData)
    {
        Text text = GetComponent<Text>();

        ChatType = type;
        text.color = color;
        text.text = textData;
    }
}
