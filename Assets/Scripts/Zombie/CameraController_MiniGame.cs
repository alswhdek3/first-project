using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using System;
using System.Linq;

public class CameraController_MiniGame : MonoBehaviour
{
    [SerializeField]
    private Transform myCharacterTarget;

    [Range(0f, 100f)]
    [SerializeField]
    private float m_height;
    [Range(-20f, 20f)]
    [SerializeField]
    private float m_distance;
    [Range(-360f, 360f)]
    [SerializeField]
    private float m_angle; //각도
    private float m_prevAngle; //이전 각도

    private Vector3 m_prevPos; //이전 좌표
    [SerializeField]
    private float m_speed;

    [Header("Clamp MinX")]
    public float clampMinX;
    [Header("Clamp MaxX")]
    public float clampMaxX;

    public Transform MyLocalPlayerTarget
    {
        get
        {
            return myCharacterTarget;
        }
        set
        {
            myCharacterTarget = value;
        }
    }

    /*private void Start()
    {
        if(myCharacterTarget == null)
        {
            StartCoroutine(Coroutine_FindMyCharacter());
        }
    }*/  

    // Update is called once per frame
    private void Update()
    {
        if(myCharacterTarget != null)
        {
            transform.eulerAngles = new Vector3(Mathf.Lerp(m_prevAngle, m_angle, m_speed * Time.deltaTime), 0f, 0f);

            transform.position = new Vector3(
                Mathf.Lerp(m_prevPos.x, myCharacterTarget.transform.position.x, m_speed * Time.deltaTime),
                Mathf.Lerp(m_prevPos.y, myCharacterTarget.transform.position.y + m_height, m_speed * Time.deltaTime),
                Mathf.Lerp(m_prevPos.z, myCharacterTarget.transform.position.z - m_distance, m_speed * Time.deltaTime)
                );
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, clampMinX, clampMaxX), transform.position.y, transform.position.z);
    }
    private void LateUpdate()
    {
        m_prevAngle = transform.eulerAngles.x;
        m_prevPos = transform.position;
    }
}
