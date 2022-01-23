using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatchManager : Singleton<BatchManager> , IBatchPoint
{
    [Header("BatchPoint Parent & Objects")]
    [SerializeField]
    private GameObject batchPointParent;
    private Transform[] batchPoints;

    [SerializeField]
    private List<bool> batchPointStateList = new List<bool>();

    public Player MyPlayer
    {
        get;
        set;
    }

    public bool GetEmptyPoint(int _index)
    {
        return batchPointStateList[_index];
    }

    public GameObject GetSelectBatchPoint(Vector3 _touchpos , out int _index)
    {
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(_touchpos), Vector2.zero);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, 1 << LayerMask.NameToLayer("Batch"));
        if (hit.collider != null)
        {
            // ex) BatchPoint_2 : _ 기준으로 나눈다
            string[] namesplit = hit.collider.transform.name.Split('_');
            _index = int.Parse(namesplit[1]);
            Debug.Log($"RayCast HitPoint : BatchPoint_{_index}");
            return hit.collider.gameObject;
        }
        _index = -1;
        return null;
    }

    public void SetBatchPoint(int _index , bool _isbatch)
    {
        batchPointStateList[_index] = _isbatch;
        Debug.Log($"BatchPoint_{_index} State : {_isbatch}");
    }

    private void InitBatchPointsSetting()
    {
        // BatchPoint Transform Componet Connect
        batchPoints = new Transform[batchPointParent.transform.childCount];
        for (int i=0; i< batchPointParent.transform.childCount; i++)
        {
            batchPoints[i] = batchPointParent.transform.GetChild(i);
            batchPoints[i].transform.name = $"BatchPoint_{i}";
            // All BatchPoint State false
            batchPointStateList.Add(true);
        }

        MyPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected override void OnStart()
    {
        InitBatchPointsSetting();
    }
    private void Update()
    {
        // 추후 전처리기 코드 추가
        if(Input.GetMouseButtonDown(0))
        {
            int index = -1;
            GameObject batchPoint = GetSelectBatchPoint(Input.mousePosition , out index);
            if(batchPoint != null)
            {
                Debug.Log($"Select BatchPoint : {batchPoint.transform.name} / Index : {index}");
                
                // 선택한 BatchPoint가 비어있는지 체크
                if(GetEmptyPoint(index))
                {
                    MyPlayer.TargetBatchPointMove(batchPoint, index);
                }
            }
        }
    }

    public GameObject GetSelectBatchPoint(Vector3 _touchpos)
    {
        throw new System.NotImplementedException();
    }
}
