using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;
using System;

public enum ChatType { Normal = 0 , Party , Guild , Whisper , System , Count}
public class ChatController : MonoBehaviour
{
    [SerializeField]
    private GameObject textChatPrefab; // ��ȭ�� ����ϴ� Text UI ������
    [SerializeField]
    private Transform parentContent; // ��ȭ�� ��µǴ� ScrollView�� Content
    [SerializeField]
    private InputField inputField; // ��ȭ �Է�â

    [SerializeField]
    private Sprite[] spriteChatInputType; // ��ȭ �Է� �Ӽ� ��ư�� ������ �̹��� ����
    [SerializeField]
    private Image imageChatInputType; // ��ȭ �Է� �Ӽ� ��ư�� �̹���
    [SerializeField]
    private Text textInput; // ��ȭ �Է� �Ӽ��� ���� ��ȭ �Է�â�� �ۼ��Ǵ� �ؽ�Ʈ ���� ����

    private ChatType currentInputType; // ���� ��ȭ �Է� �Ӽ�
    private Color currentTextColor;

    private List<ChatCell> chatList; // ��ȭâ�� ��µǴ� ��� ��ȭ�� �����ϴ� ����Ʈ
    private ChatType currentViewType; // ���� ��ȭ ���� �Ӽ�(Noral , Party , Guild , Whisper , System)

    private string ID = "DoctorKO"; // ���� ���̵�(�ӽ�)

    private void Awake()
    {
        chatList = new List<ChatCell>();

        currentInputType = ChatType.Normal;
        currentTextColor = Color.white;
    }

    private void Update()
    {
        // ��ȭ �Է�â�� ��Ŀ�� �Ǿ����� ���� �� EnterŰ�� ������
        if(Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
        {
            // ��ȭ �Է�â�� ��Ŀ���� Ȱ��ȭ
            inputField.ActivateInputField();
        }
    }

    /// <summary>
    /// �Է��� �������¿��� EnterŰ�� ������
    /// </summary>
    public void OnEndEditEventMethod()
    {
        // EnterŰ�� ������ ��ȭ �Է�â�� �Էµ� ������ ��ȭâ�� ���
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdateChat();
        }
    }

    public void UpdateChat()
    {
        // InputField�� ��������� ����
        if (inputField.text.Equals("")) return;

        // ��ȭ ������ ����� ���� Text UI ����
        GameObject clone = Instantiate(textChatPrefab, parentContent);
        ChatCell cell = clone.GetComponent<ChatCell>();

        // ��ȭ �Է�â�� �ִ� ������ ��ȭâ�� ��� (ID : ����)
        //clone.GetComponent<Text>().text = $"{ID} : {inputField.text}";
        cell.Setup(currentInputType, currentTextColor, $"{ID} : {inputField.text}");

        // ��ȭ �Է�â�� �ִ� ���� �ʱ�ȭ
        inputField.text = "";
        // ��ȭâ�� ����� ��ȭ�� ����Ʈ�� ����
        chatList.Add(cell);
    }

    private Color ChatTypeToColor(ChatType type)
    {
        // ��ȭ �Ӽ��� ���� ���� �� ��ȯ(�Ϲ� , ��Ƽ , ��� , �Ӹ� , �ý���)
        Color[] colors = new Color[(int)ChatType.Count]
        {
            Color.white , Color.blue , Color.green,Color.magenta,Color.yellow
        };

        return colors[(int)type];
    }

    public void SetCurrentInputType()
    {
        // ���� ��ȭ �Ӽ��� �� �ܰ辿 ��ȭ (�Ӹ�, �ý����� �Է� �Ӽ��� ���� ������ ����)
        currentInputType = (int)currentInputType < (int)ChatType.Count - 3 ? currentInputType + 1 : 0;
        //���� ��ȭ �Ӽ��� ���� �̹��� ����
        imageChatInputType.sprite = spriteChatInputType[(int)currentInputType];

        currentTextColor = ChatTypeToColor(currentInputType);

        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor;
    }

    public void SetCurrentViewType(int newType)
    {
        // Button UI�� OnClick �̺�Ʈ�� �������� �Ű������� ó���� �ȵǼ� int�� �޾ƿ´�.

        // ���� ��ȭ ���� �Ӽ��� �Ϲ��̸�
        if(currentInputType == ChatType.Normal)
        {
            // ��� ��ȭ ��� Ȱ��ȭ
            for(int i=0; i<chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(true);
            }
        }
        // ���� ��ȭ ���� �Ӽ��� �Ϲ��� �ƴϸ�
        else
        {
            for(int i=0; i<chatList.Count; ++i)
            {
                // ���� ��ȭ ���� �Ӽ��� �ش��ϴ� ��ȭ�� Ȱ��ȭ�ϰ�, �������� ��Ȱ��ȭ
                chatList[i].gameObject.SetActive(chatList[i].ChatType == currentViewType);
            }
        }
    }
}
