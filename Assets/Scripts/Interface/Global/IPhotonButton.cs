using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 여러 미니게임에 대한 버튼들을 공통으로 쓰기위한 인터페이스
/// </summary>
public interface IPhotonButton
{
    void OnClickStart();
    void OnClickGameCancel();
    void OnClickExit();
}
