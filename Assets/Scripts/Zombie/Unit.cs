using System;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;

public enum UnitState
{
    None=-1,
    Idle,Run,Attack,
    Max
}

public abstract class Unit :  MonoBehaviourPun, IUnit , IAnimation,  IPunObservable
{
    protected Rigidbody rb;
    protected Animator animator;

    protected PhotonView pv;

    protected MovePadController movePadController;

    protected Vector3 currentDir;
    protected Quaternion currentRot;

    protected Dictionary<UnitState, IState> stateTable = new Dictionary<UnitState, IState>();
    protected UnitState currentState;

    // 아이템 버프 이벤트
    public event EventHandler<ZombieGameItem> ItemBuffEvent;

    #region 프로퍼티
    public int ActorNumber { get; set; }
    public float Speed { get; set; }
    public UnitState CurrentState { get { return currentState; } }
    public Dictionary<string, float> AnimationLengthTable { get; set; }
    public Animator UnitAnimator { get { return animator; } }
    #endregion

    public abstract void SetUnit<T>(int _actornumber, float _speed , T _manager) where T : class;

    public abstract void InitEventAdd();

    public float GetAnimationLength(string _animationclipname)
    {
        if(!AnimationLengthTable.ContainsKey(_animationclipname))
        {
            Debug.LogError($"AnimationTable에 {_animationclipname}키가 존재하지않습니다 !!");
            return -1f;
        }

        return AnimationLengthTable[_animationclipname];
    }

    public bool GetIsLocalPlayer()
    {
        return pv.IsMine;
    }

    public void SetMovePadController(MovePadController _movePadController)
    {
        movePadController = _movePadController;
    }

    #region RPC
    [PunRPC]
    protected void AnimationShareRPC(string _run, bool _isDrag)
    {
        animator.SetBool(_run, _isDrag);
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            currentDir = (Vector3)stream.ReceiveNext();
            currentRot = (Quaternion)stream.ReceiveNext();
        }
    }

    #region 재정의 메서드
    public virtual void SetState(UnitState _state)
    {
        if (_state == currentState)
            return;

        // 현재상태 FSM 종료
        stateTable[currentState].OperatorExit();
        currentState = UnitState.None;
        // 현재상태 변경
        currentState = _state;
        stateTable[currentState].OperatorEnter(); // 이벤트 전달
    }

    public virtual void ResetState()
    {
        float length = GetAnimationLength(UnitState.Attack.ToString());
        StartCoroutine(nameof(Coroutine_ResetState));

        IEnumerator Coroutine_ResetState()
        {
            yield return new WaitForSeconds(length);
            animator.SetBool(UnitState.Run.ToString(), false);
        }
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        pv = photonView;
    }

    protected virtual void Start()
    {
        // 생성된 애니메이션 재생시간 테이블 추가
        AnimationLengthTable = new Dictionary<string, float>();
        int animationcliplength = animator.runtimeAnimatorController.animationClips.Length;
        for(int i=0; i< animationcliplength; i++)
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];

            // Key : Animationclipname , Value : AnimationLength
            if (!AnimationLengthTable.ContainsKey(clip.name))
                AnimationLengthTable.Add(clip.name, clip.length);
        }
    }

    protected virtual void Update()
    {
        // LocalPlayer가 아닌 대상들의 transform값을 가져와서 적용
        if(!pv.IsMine)
        {
            // distance값이 N값 이상이면 순간이동
            if ((transform.position - currentDir).sqrMagnitude >= 10f * 10f)
            {
                transform.position = currentDir;
                transform.rotation = currentRot;
            }
            //보관 기법으로 부드럽게 이동
            else
            {
                transform.position = Vector3.Lerp(transform.position, currentDir, Time.deltaTime * 10f);
                transform.rotation = Quaternion.Slerp(transform.rotation, currentRot, Time.deltaTime * 10f);
            }
        }
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // 아이템 버프 이벤트
        if(other.gameObject.CompareTag("Item"))
            ItemBuffEvent?.Invoke(this, other.GetComponent<ZombieGameItem>());
    }
    #endregion
}
