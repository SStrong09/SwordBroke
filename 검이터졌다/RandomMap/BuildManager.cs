using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using UnityEditor;

public class BuildManager : MonoBehaviour
{
    #region Singleton
    private static BuildManager instance;
    public static BuildManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<BuildManager>();
            return instance;
        }
    }
    #endregion

    public ParseMapInfo PMI;
    public LevelBuilder[] LB;
    public GameObject Player;
    private int StageLevel;

    void Start()
    {
        StageLevel = 0;
        PMI = new ParseMapInfo();
        foreach(LevelBuilder lb in LB)
        {
            lb.Level = StageLevel;
            lb.BuildRandomMap(transform.position);
            lb.BuildRandomWay();
            SetPlayer(lb.GetSetPlayerPos());
            lb.DrawMiniMap();
            lb.MakeMonster();
            lb.Portalinit();
            StageLevel++;
        }
        
    }

    public void SetPlayer(Vector3 PlayerLocation)
    {
        Player.transform.Translate(PlayerLocation);
    }

    public void TakeDamagePlayer()
    {
        LB[StageLevel-1].SpreadSwordPiece();
    }

    public void SetSwordMaxCount()
    {
        LB[StageLevel - 1].SwordMaxCount = Player.transform.GetComponentInChildren<PlayerStatConroller>().MaxPiece_Count+1;
    }

    public void SpreadObejct()
    {
        LB[StageLevel-1].InitSpreadObject();
    }
}



public class ParseMapInfo
{
    string[] NodeInfo;
    List<string[]> NodeInfoList;
    public ParseMapInfo()
    {
        parseText();
    }

    //텍스트에서 정보를 한줄씩 가져와서 NodeInfo에 저장
    public void parseText()
    {
        NodeInfoList = new List<string[]>();
        TextAsset MapAsset = Resources.Load("TextMapInfo") as TextAsset;
        string textValued = MapAsset.text;
        string[] textValue = textValued.Split('\n');
        //string[] textValue = System.IO.File.ReadAllLines(path);
        NodeInfo = new string[(textValue.Length) * (textValue.Length)];
        for (int i = 0; i < textValue.Length; i++)
        {
            NodeInfo = textValue[i].Split(' ');
            NodeInfoList.Add(NodeInfo);
        }
    }

    //TextInfoList에는 맵 정보가 1줄씩 264줄이 들어가있음
    //맵 하나당 필요한 정보는 6줄임 그래서 0~5줄 6~11 이런식으로 가져와서 
    //Map에 필요한 5줄만 저장하고 Map를 리턴함
    public List<string[]> GetRandomMap(int RandomNumber)
    {
        List<string[]> Map = new List<string[]>();
        for (int i = 0; i < 6; i++)
        {
            Map.Add(NodeInfoList[i + (RandomNumber * 5 + RandomNumber)]);
        }
        return Map;
    }


    //맵 나선환 ~ 
    public List<string[]> RotateMap(List<string[]> _Map)
    {
        List<string[]> TempList = new List<string[]>();
        for (int i = 0; i < 6; i++)
        {
            TempList.Add(new string[6]);
        }
        for (int y = 0; y < _Map.Count; y++)
        {
            for (int x = 0; x < _Map[y].Length; x++)
            {
                TempList[x][y] = _Map[6 - 1 - y][x];

            }
        }
        for (int j = 0; j < TempList.Count; j++)
        {
            for (int k = 0; k < TempList[j].Length; k++)
            {
                _Map[j][k] = TempList[j][k];
            }
        }
        return _Map;
    }
}