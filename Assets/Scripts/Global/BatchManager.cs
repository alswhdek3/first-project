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
            Transform batchpoint = UtilManager<Transform>.GetRayCastTarget(Input.mousePosition, "Batch");
            string[] namesplit = batchpoint.transform.name.Split('_'); // ex) Batch_2
            int index = int.Parse(namesplit[1]);
            Debug.Log($"Select BatchPoint : {batchpoint.transform.name} / Index : {index}");

            // 선택한 BatchPoint가 비어있는지 체크
            if (GetEmptyPoint(index))
            {
                MyPlayer.TargetBatchPointMove(batchpoint.gameObject, index);
            }
        }
    }

    public GameObject GetSelectBatchPoint(Vector3 _touchpos)
    {
        throw new System.NotImplementedException();
    }
}
