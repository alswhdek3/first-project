using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class MovePadController : MonoBehaviour
{
    [Header("MovePad Controller")]    
    [SerializeField]
    private GameObject padController;
    [SerializeField]
    private GameObject padCenter;

    [Header("MoveController Player")]
    [SerializeField]
    private Transform myplayer;
    private float speed;

    public float maxDir = 150f;
    private float maxDirPow;

    private Vector3 touchpoint;
    private Vector3 touchposition;

    private bool isDrag;
    public bool IsDrag { get { return isDrag; } }

    private Vector3 GetTouchPadPosition(Vector3 _touchPosition)
    {
        Ray2D ray = new Ray2D(_touchPosition, Vector2.zero);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, 1 << LayerMask.NameToLayer("UI"));

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("MovePad"))
            {
                return hit.point;
            }
        }
        return Vector3.zero;
    }

    private void Move()
    {
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if(Input.touchCount > 0)
            {
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    touchpoint = GetTouchPadPosition(Input.GetTouch(0).position);
                    if (touchpoint != Vector3.zero)
                        isDrag = true;
                }
                if(isDrag)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        touchposition = Input.GetTouch(0).position;
                    }
                }
                if(Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    isDrag = false;
                    padController.transform.position = padCenter.transform.position;
                    touchpoint = Vector3.zero;
                }
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                touchpoint = GetTouchPadPosition(Input.mousePosition);
                if (touchpoint != Vector3.zero)
                    isDrag = true;
            }
            if(Input.GetMouseButton(0))
            {
                if(isDrag)
                    touchposition = Input.mousePosition;
            }
            if(Input.GetMouseButtonUp(0))
            {
                isDrag = false;
                padController.transform.position = padCenter.transform.position;
                touchposition = Vector3.zero;
            }    
        }

        if(isDrag)
        {
            Vector3 distance = touchposition - padCenter.transform.position;
            // 경계범위 내에서 컨트롤러를 움직임
            if (Mathf.Approximately(distance.sqrMagnitude, maxDirPow) || distance.sqrMagnitude < maxDirPow)
            {
                padController.transform.position = padCenter.transform.position + distance;
            }
            else
            {
                padController.transform.position = padCenter.transform.position + distance.normalized * maxDir;
            }
            // Player Move Controller
            myplayer.GetComponent<Rigidbody>().velocity = new Vector3(distance.x, myplayer.transform.position.y, distance.y).normalized * speed;
            myplayer.transform.forward = new Vector3(distance.x, myplayer.transform.position.y, distance.y).normalized;
        }
        else
            myplayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void SetMyPlayer(Transform _myplayer , float _speed)
    {
        myplayer = _myplayer;
        speed = _speed;
    }

    private void Awake()
    {
        maxDirPow = Mathf.Pow(maxDir, 2f);
        isDrag = false;
    }

    private void Update()
    {
        if(myplayer != null)
        {
            if(!myplayer.GetComponent<ZombiePlayer>().IsZombie)
                Move();
        }       
    }
}
