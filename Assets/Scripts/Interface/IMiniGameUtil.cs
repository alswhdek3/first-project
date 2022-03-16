using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMiniGameUtil
{
    CameraController_MiniGame TargetCamera { get; set; }

    void ShowTargetCameraInfo(int _actionNumber);
}
