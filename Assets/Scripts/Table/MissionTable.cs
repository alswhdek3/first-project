using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionData
{
    public enum MissionDataHead
    {
        None=-1,
        Index,Conditions1,Conditions2,Conditions3,FaceImageIndex,TextUI,Jamamount,ExpAmount,
        Max
    }

    public int index;
    public string[] conditions;
    public int faceImageIndex;
    public string textUI;
    public int jamamount;
    public int expamount;
}
[CreateAssetMenu(fileName = "MissionTable",menuName = "Table/MissionTable")]
public class MissionTable : BaseTable
{
    [SerializeField]
    private List<MissionData> missionList;
    protected override void InitTableSetting()
    {
        path = "Data/MissionExcelData";
        base.InitTableSetting();
        missionList = new List<MissionData>();

        // 액셀 데이터 세팅
        string data = dataTextAsset.text.Substring(0, dataTextAsset.text.Length);
        string[] rowdata = data.Split('\n');
        int rowcount = rowdata.Length;
        for(int i=0; i<rowcount; i++)
        {
            string[] coldata = rowdata[i].Split('\t');

            MissionData missiondata = new MissionData();
            missiondata.conditions = new string[(int)MissionData.MissionDataHead.Conditions3];
            for (int j=0; j<coldata.Length; j++)
            {               
                switch ((MissionData.MissionDataHead)j)
                {
                    case MissionData.MissionDataHead.Index:
                        missiondata.index = int.Parse(coldata[j]);
                        break;
                    case MissionData.MissionDataHead.Conditions1:
                        missiondata.conditions[(int)MissionData.MissionDataHead.Conditions1-1] = coldata[j];
                        break;
                    case MissionData.MissionDataHead.Conditions2:
                        missiondata.conditions[(int)MissionData.MissionDataHead.Conditions2-1] = coldata[j];
                        break;
                    case MissionData.MissionDataHead.Conditions3:
                        missiondata.conditions[(int)MissionData.MissionDataHead.Conditions3-1] = coldata[j];
                        break;
                    case MissionData.MissionDataHead.FaceImageIndex:
                        missiondata.faceImageIndex = int.Parse(coldata[j]);
                        break;
                    case MissionData.MissionDataHead.TextUI:
                        missiondata.textUI = coldata[j];
                        break;
                    case MissionData.MissionDataHead.Jamamount:
                        missiondata.jamamount = int.Parse(coldata[j]);
                        break;
                    case MissionData.MissionDataHead.ExpAmount:
                        missiondata.expamount = int.Parse(coldata[j]);
                        break;
                }
            }
            missionList.Add(missiondata);
        }
    }
    protected override void Awake()
    {
        base.Awake();
    }
}
