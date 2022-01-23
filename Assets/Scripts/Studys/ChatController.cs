using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;
using System;

public enum ChatType { Normal = 0 , Party , Guild , Whisper , System , Count}
public class ChatController : MonoBehaviour
{
    [SerializeField]
    private GameObject textChatPrefab; // 대화를 출력하는 Text UI 프리팹
    [SerializeField]
    private Transform parentContent; // 대화가 출력되는 ScrollView의 Content
    [SerializeField]
    private InputField inputField; // 대화 입력창

    [SerializeField]
    private Sprite[] spriteChatInputType; // 대화 입력 속성 버튼에 적용할 이미지 에셋
    [SerializeField]
    private Image imageChatInputType; // 대화 입력 속성 버튼의 이미지
    [SerializeField]
    private Text textInput; // 대화 입력 속성에 따라 대화 입력창에 작성되는 텍스트 색상 변경

    private ChatType currentInputType; // 현재 대화 입력 속성
    private Color currentTextColor;

    private List<ChatCell> chatList; // 대화창에 출력되는 모든 대화를 보관하는 리스트
    private ChatType currentViewType; // 현재 대화 보기 속성(Noral , Party , Guild , Whisper , System)

    private string ID = "DoctorKO"; // 본인 아이디(임시)

    private void Awake()
    {
        chatList = new List<ChatCell>();

        currentInputType = ChatType.Normal;
        currentTextColor = Color.white;
    }

    private void Update()
    {
        // 대화 입력창이 포커스 되어있지 않을 때 Enter키를 누르면
        if(Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
        {
            // 대화 입력창의 포커스를 활성화
            inputField.ActivateInputField();
        }
    }

    /// <summary>
    /// 입력이 끝난상태에서 Enter키를 누르면
    /// </summary>
    public void OnEndEditEventMethod()
    {
        // Enter키를 누르면 대화 입력창에 입력된 내용을 대화창에 출력
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdateChat();
        }
    }

    public void UpdateChat()
    {
        // InputField가 비어있으면 종료
        if (inputField.text.Equals("")) return;

        // 대화 내용을 출력을 위해 Text UI 생성
        GameObject clone = Instantiate(textChatPrefab, parentContent);
        ChatCell cell = clone.GetComponent<ChatCell>();

        // 대화 입력창에 있는 내용을 대화창에 출력 (ID : 내용)
        //clone.GetComponent<Text>().text = $"{ID} : {inputField.text}";
        cell.Setup(currentInputType, currentTextColor, $"{ID} : {inputField.text}");

        // 대화 입력창에 있는 내용 초기화
        inputField.text = "";
        // 대화창에 출력한 대화를 리스트에 저장
        chatList.Add(cell);
    }

    private Color ChatTypeToColor(ChatType type)
    {
        // 대화 속성에 따라 색상 값 변환(일반 , 파티 , 길드 , 귓말 , 시스템)
        Color[] colors = new Color[(int)ChatType.Count]
        {
            Color.white , Color.blue , Color.green,Color.magenta,Color.yellow
        };

        return colors[(int)type];
    }

    public void SetCurrentInputType()
    {
        // 현재 대화 속성을 한 단계씩 변화 (귓말, 시스템은 입력 속성에 없기 때문에 제외)
        currentInputType = (int)currentInputType < (int)ChatType.Count - 3 ? currentInputType + 1 : 0;
        //현재 대화 속성에 따른 이미지 변경
        imageChatInputType.sprite = spriteChatInputType[(int)currentInputType];

        currentTextColor = ChatTypeToColor(currentInputType);

        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor;
    }

    public void SetCurrentViewType(int newType)
    {
        // Button UI의 OnClick 이벤트에 열거형은 매개변수로 처리가 안되서 int로 받아온다.

        // 현재 대화 보기 속성이 일반이면
        if(currentInputType == ChatType.Normal)
        {
            // 모든 대화 목록 활성화
            for(int i=0; i<chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(true);
            }
        }
        // 현재 대화 보기 속성이 일반이 아니면
        else
        {
            for(int i=0; i<chatList.Count; ++i)
            {
                // 현재 대화 보기 속성에 해당하는 대화만 활성화하고, 나머지는 비활성화
                chatList[i].gameObject.SetActive(chatList[i].ChatType == currentViewType);
            }
        }
    }
}
