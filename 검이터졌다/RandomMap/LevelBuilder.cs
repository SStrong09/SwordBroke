
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}


[System.Serializable]
//이 스크립터블 오브젝트는 유니티 에디터 상에서 생성 가능합니다.
//경로는 "New Object/DataBase System/Stage/Map"
[CreateAssetMenu(fileName = "New Level", menuName = "DataBase System/Stage/Level")]
public class LevelBuilder : ScriptableObject
{

    [Header("방")]
    //맵을 가져온다. 
    public MapInfo NormalRoom;
    public int NormalRoomCount;
    public GameObject[] SpecialRooms;
    public GameObject[] BossRooms;

    //만들어질 방 NavMesh정보를 갖고있다.
    public GameObject NodeRoom;
    public GameObject CrossRoom;

    [Header("몬스터")]
    public int MonsterCount;
    public GameObject[] SmallMonsters;
    public GameObject[] MediumMonsters;

    [Header("오브젝트")]
    public GameObject Treasurebox;
    public GameObject SwordPiece;
    private List<RoomInfo> CanSpreadSwordRooms;


    private List<MapMatrix> MapPosMatrix;
    private List<GameObject> MainRoom;

    private List<List<RoomInfo>> Island;
    private List<int> TempRoom;
    private List<int> haveRoomList;
    private List<int[]> haveRoomMatrix;
    private int Spacing = 300;
    private int Size;

    public int Level;
    public int SwordMaxCount;

    // 길찾기 알고리즘
    public List<Node> FinalNodeList;
    public List<List<Node>> Route;
    Node[,] NodeArray;
    Node  CurNode;
    List<Node> OpenList, ClosedList;
    List<Node> StartNodeList, TargetNodeList;


    int Max;
    int MaxindexNumber;

    #region  방만들기 ~ 

    public void BuildRandomMap(Vector3 BuildPosition)
    {
        Size = (int)Mathf.Sqrt(NormalRoomCount + SpecialRooms.Length)+1;
        MapPosMatrix = new List<MapMatrix>();
        MainRoom = new List<GameObject>();
        TempRoom = new List<int>();
        
        for (int i = 0; i <Mathf.Pow( Size,2); i++)
        {
            MapPosMatrix.Add(new MapMatrix());
        }
        
        int SpacingX = 0;
        int SpacingY = 0;
        for (int i = 0; i < MapPosMatrix.Count; i++)
        {
            MapPosMatrix[i].RoomPos = new Vector3(BuildPosition.x + SpacingX, 0, BuildPosition.y + SpacingY);
            SpacingX += Spacing;
            if (i != 0)
            {
                if (i % Size == Size-1)
                {
                    SpacingX = 0;
                    SpacingY += Spacing;
                }
            }
        }
        BuildMainRoom(MapPosMatrix.Count);
        BuildNormalRoom();
        BuildSpacialRoom();
    }

    public void BuildMainRoom(int Count)
    {
        for(int i=0; i < Count; i++)
        {
            GameObject RoomGo = Instantiate(NodeRoom, MapPosMatrix[i].RoomPos, Quaternion.identity) as GameObject;
            RoomGo.GetComponent<RoomInfo>().RoomID = i;
            RoomGo.GetComponent<RoomInfo>().FindPath();
            TempRoom.Add(i);
            MainRoom.Add(RoomGo);
        }
    }

    public void BuildNormalRoom()
    {
        for (int i = 0; i < NormalRoomCount; i++)
        {
            int RandNumber = GetRandomMainRoom();
            NormalRoom.PalletteSwap();
            NormalRoom.BuildRandomTile(MainRoom[RandNumber].transform.position, Random.Range(0, 21), MainRoom[RandNumber]);
        }
    }

    public void SetPosBossRoom()
    {
        int FirstRandomDirect = Random.Range(0, 4);
        switch (FirstRandomDirect)
        {
            case 0:
                int SecondRandomDirect = Random.Range(0, 2);
                switch (SecondRandomDirect)
                {
                    //시계 방향
                    case 0:
                        // 방향 1 맨 왼쪽아래에서 위로 ↑
                        for (int i = 0; i < Size; i++)
                        {
                            if (haveRoomMatrix[i][0] == 1)
                            {
                                Vector3 BossRoomPos = new Vector3(MapPosMatrix[i + i * (Size - 1)].RoomPos.x - Spacing, 0, MapPosMatrix[i + i * (Size - 1)].RoomPos.z);
                                BuildBossRoom(BossRoomPos);
                                return;
                            }
                        }
                        // 방향 2 맨 왼쪽위에서 오른쪽으로 →
                        for(int i=0; i<Size; i++)
                        {
                            if (haveRoomMatrix[Size - 1][i] == 1)
                            {
                                Vector3 BossRoomPos = new Vector3(MapPosMatrix[Size - 1 + i+ ((Size - 1) * (Size - 1))].RoomPos.x, 0, MapPosMatrix[Size - 1 + i + ((Size - 1) * (Size - 1))].RoomPos.z + Spacing);
                                BuildBossRoom(BossRoomPos);
                                return;
                            }
                        }
                        // 방향 3 맨 오른쪽위에서 아래쪽으로 ↓
                        for (int i = Size; i > 0; i--)
                        {
                            if (haveRoomMatrix[i - 1][Size - 1] == 1)
                            {
                                Vector3 BossRoomPos = new Vector3(MapPosMatrix[Size - 1 + i - 1 + ((i - 1) * (Size - 1))].RoomPos.x + Spacing, 0, MapPosMatrix[Size - 1 + i - 1 + ((i - 1) * (Size - 1))].RoomPos.z);
                                BuildBossRoom(BossRoomPos);
                                return;
                            }
                        }
                        //방향 4 맨 오른쪽아래에서 왼쪽으로 ←
                        for(int i=Size; i>0; i--)
                        {
                            if (haveRoomMatrix[0][i-1] == 1)
                            {
                                Vector3 BossRoomPos = new Vector3(MapPosMatrix[i-1].RoomPos.x , 0, MapPosMatrix[i-1].RoomPos.z -Spacing);
                                BuildBossRoom(BossRoomPos);
                                return;
                            }
                        }
                        break;

                    //반시계 방향
                    case 1:
                        // 방향 5 맨 왼쪽아래에서 오른쪽으로  →
                        for (int j = 0; j < Size; j++)
                        {
                            if (haveRoomMatrix[0][j] == 1)
                            {
                                Vector3 BossRoomPos = new Vector3(MapPosMatrix[j].RoomPos.x, 0, MapPosMatrix[j].RoomPos.z - Spacing);
                                BuildBossRoom(BossRoomPos);
                                return;
                            }
                        }
                        // 방향 6 맨 오른쪽아래에서 위쪽으로 ↑
                        for (int i=0; i< Size; i++)
                        {
                            if (haveRoomMatrix[i][Size - 1] == 1)
                            {
                                Vector3 BossRoomPos = new Vector3(MapPosMatrix[((Size-1)*(1+i))+i].RoomPos.x - Spacing, 0, MapPosMatrix[((Size - 1) * (1 + i)) + i].RoomPos.z);
                                BuildBossRoom(BossRoomPos);
                                return;
                            }
                        }
                        // 방향 7 맨 오른쪽위에서 왼쪽으로 ←
                        for (int i = Size; i>0; i--)
                        {
                            if (haveRoomMatrix[Size - 1][i] == 1)
                            {
                                Vector3 BossRoomPos = new Vector3(MapPosMatrix[Size - 1 + i + ((Size - 1) * (Size - 1))].RoomPos.x - Spacing, 0, MapPosMatrix[Size - 1 + i + ((Size - 1) * (Size - 1))].RoomPos.z);
                                BuildBossRoom(BossRoomPos);
                                return;
                            }
                        }
                        // 방향 8 맨 왼쪽위에서 아래쪽으로 ↓
                        break;
                }
                
                
                // 방향 3 맨 오른쪽 위쪽에서 왼쪽으로 ←
                for (int j = Size; j > 0; j--)
                {
                    if (haveRoomMatrix[Size - 1][j - 1] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[j - 1 + Size - 1 + (Size - 1) * (Size - 1)].RoomPos.x, 0, MapPosMatrix[j - 1 + Size - 1 + (Size - 1) * (Size - 1)].RoomPos.z + Spacing);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                
                break;
            case 1:
                // 방향 2 맨 왼쪽아래에서 오른쪽으로  →
                for (int j = 0; j < Size; j++)
                {
                    if (haveRoomMatrix[0][j] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[j].RoomPos.x, 0, MapPosMatrix[j].RoomPos.z - Spacing);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                
                
                // 방향 3 맨 오른쪽 위쪽에서 왼쪽으로 ←
                for (int j = Size; j > 0; j--)
                {
                    if (haveRoomMatrix[Size - 1][j - 1] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[j - 1 + Size - 1 + (Size - 1) * (Size - 1)].RoomPos.x, 0, MapPosMatrix[j - 1 + Size - 1 + (Size - 1) * (Size - 1)].RoomPos.z + Spacing);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                // 방향 4 맨 오른쪽 위쪽에서 아래쪽으로 ↓
                for (int i = Size; i > 0; i--)
                {
                    if (haveRoomMatrix[i - 1][Size - 1] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[Size - 1 + i - 1 + ((i - 1) * (Size - 1))].RoomPos.x + Spacing, 0, MapPosMatrix[Size - 1 + i - 1 + ((i - 1) * (Size - 1))].RoomPos.z);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                // 방향 1 맨 왼쪽아래에서 위로 ↑
                for (int i = 0; i < Size; i++)
                {
                    if (haveRoomMatrix[i][0] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[i + i * (Size - 1)].RoomPos.x - Spacing, 0, MapPosMatrix[i + i * (Size - 1)].RoomPos.z);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                break;
            case 2:
                // 방향 3 맨 오른쪽 위쪽에서 왼쪽으로 ←
                for (int j = Size; j > 0; j--)
                {
                    if (haveRoomMatrix[Size - 1][j - 1] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[j - 1 + Size - 1 + (Size - 1) * (Size - 1)].RoomPos.x, 0, MapPosMatrix[j - 1 + Size - 1 + (Size - 1) * (Size - 1)].RoomPos.z + Spacing);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                // 방향 4 맨 오른쪽 위쪽에서 아래쪽으로 ↓
                for (int i = Size; i > 0; i--)
                {
                    if (haveRoomMatrix[i - 1][Size - 1] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[Size - 1 + i - 1 + ((i - 1) * (Size - 1))].RoomPos.x + Spacing, 0, MapPosMatrix[Size - 1 + i - 1 + ((i - 1) * (Size - 1))].RoomPos.z);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                // 방향 1 맨 왼쪽아래에서 위로 ↑
                for (int i = 0; i < Size; i++)
                {
                    if (haveRoomMatrix[i][0] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[i + i * (Size - 1)].RoomPos.x - Spacing, 0, MapPosMatrix[i + i * (Size - 1)].RoomPos.z);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                // 방향 2 맨 왼쪽아래에서 오른쪽으로  →
                for (int j = 0; j < Size; j++)
                {
                    if (haveRoomMatrix[0][j] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[j].RoomPos.x, 0, MapPosMatrix[j].RoomPos.z - Spacing);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                break;
            case 3:
                // 방향 4 맨 오른쪽 위쪽에서 아래쪽으로 ↓
                for (int i = Size; i > 0; i--)
                {
                    if (haveRoomMatrix[i - 1][Size - 1] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[Size - 1 + i - 1 + ((i - 1) * (Size - 1))].RoomPos.x + Spacing, 0, MapPosMatrix[Size - 1 + i - 1 + ((i - 1) * (Size - 1))].RoomPos.z);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                // 방향 1 맨 왼쪽아래에서 위로 ↑
                for (int i = 0; i < Size; i++)
                {
                    if (haveRoomMatrix[i][0] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[i + i * (Size - 1)].RoomPos.x - Spacing, 0, MapPosMatrix[i + i * (Size - 1)].RoomPos.z);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                // 방향 2 맨 왼쪽아래에서 오른쪽으로  →
                for (int j = 0; j < Size; j++)
                {
                    if (haveRoomMatrix[0][j] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[j].RoomPos.x, 0, MapPosMatrix[j].RoomPos.z - Spacing);
                        BuildBossRoom(BossRoomPos);
                        return;
                    }
                }
                // 방향 3 맨 오른쪽 위쪽에서 왼쪽으로 ←
                for (int j = Size; j > 0; j--)
                {
                    if (haveRoomMatrix[Size - 1][j - 1] == 1)
                    {
                        Vector3 BossRoomPos = new Vector3(MapPosMatrix[j - 1 + Size - 1 + (Size - 1) * (Size - 1)].RoomPos.x, 0, MapPosMatrix[j - 1 + Size - 1 + (Size - 1) * (Size - 1)].RoomPos.z + Spacing);
                        BuildBossRoom(BossRoomPos);
                    }
                }
                break;
        }
        
    }

    public void BuildBossRoom(Vector3 BossRoomPos)
    {
        GameObject RoomGo = Instantiate(NodeRoom, BossRoomPos, Quaternion.identity) as GameObject;
        GameObject BoosRoomGo = Instantiate(BossRooms[Random.Range(0, BossRooms.Length)], RoomGo.transform.position, Quaternion.identity) as GameObject;
        BoosRoomGo.transform.parent = RoomGo.transform;
        RoomGo.GetComponent<RoomInfo>().IsSpecialRoom = true;
        RoomGo.GetComponent<RoomInfo>().haveRoom = true;
        RoomGo.GetComponent<RoomInfo>().IsBossRoom = true;
        RoomGo.GetComponent<RoomInfo>().RoomID = 100;

        foreach (RoomInfo room in GameObject.FindObjectsOfType<RoomInfo>())
        {
            room.BuildWay();
        }
        return;
    }

    public void BuildSpacialRoom()
    {
        for(int i =0; i < SpecialRooms.Length; i++)
        {
            int RandNumber = GetRandomMainRoom();
            MainRoom[RandNumber].GetComponent<RoomInfo>().IsSpecialRoom = true;
            MainRoom[RandNumber].GetComponent<RoomInfo>().IsShopRoom = true;
            MainRoom[RandNumber].GetComponent<RoomInfo>().haveRoom = true;
            GameObject SpacialRoomGo = Instantiate(SpecialRooms[i], MainRoom[RandNumber].transform.position, Quaternion.identity) as GameObject;
            SpacialRoomGo.transform.parent = MainRoom[RandNumber].transform;
        }
    }
    public int GetRandomMainRoom()
    {
        int Random = UnityEngine.Random.Range(0, TempRoom.Count);
        int RoomNumber = TempRoom[Random];
        TempRoom.RemoveAt(Random);
        return RoomNumber;
    }


    #endregion

    #region  길 만들기 ~
    public void BuildRandomWay()
    {
        
        foreach (RoomInfo room in GameObject.FindObjectsOfType<RoomInfo>())
        {
            room.BuildWay();
        }
        
        GethaveRoomList();
        haveRoomMatrix = new List<int[]>();
        GethaveRoomMatrix();

        //섬을 찾는 함수
        Island = new List<List<RoomInfo>>();   
        DetectNoWayRoom();
        DetectIsland();

        //다리를 만드는 함수
        FindBuildBridge();

        GetIslandList();
        GethaveRoomMatrix();
        
        FindPath();
        FindShortestPath();

        GetIslandList();
        GethaveRoomMatrix();
        FindPath();
        FindShortestPath();

        GetIslandList();
        GethaveRoomMatrix();
        SetPosBossRoom();
        CloseDoor();
        DestroyEmptyRoom();
        BuildNavmesh();
        SetRoomDiretType();

    }
    public void GethaveRoomList()
    {
        haveRoomList = new List<int>();
        for (int i = 0; i < MainRoom.Count; i++)
        {
            haveRoomList.Add(MainRoom[i].GetComponent<RoomInfo>().haveRoom ? 1 : 0);
        }
    }
    public void GethaveRoomMatrix()
    {
        haveRoomMatrix.Clear();
        for (int i = 0; i < (int)Mathf.Sqrt(haveRoomList.Count); i++)
        {
            haveRoomMatrix.Add(new int[(int)Mathf.Sqrt(haveRoomList.Count)]);
            for (int j = 0; j < (int)Mathf.Sqrt(haveRoomList.Count); j++)
            {
                haveRoomMatrix[i][j] = haveRoomList[j + i + i * ((int)Mathf.Sqrt(haveRoomList.Count) - 1)];
            }
        }
    }

    public void DetectNoWayRoom()
    {
        Island = new List<List<RoomInfo>>();
        foreach (RoomInfo room in GameObject.FindObjectsOfType<RoomInfo>())
        {
            room.FindPath();
            if (room.haveRoom == true)
            {
                if (room.IsWayNull == true)
                {
                    List<RoomInfo> TempList = new List<RoomInfo>();
                    TempList.Add(room);
                    Island.Add(TempList);
                }
            }
        }
    }

    public void DetectIsland()
    {
        List<List<RoomInfo>> RoomIsland;
        List<RoomInfo> linkedRoom;
        RoomIsland = new List<List<RoomInfo>>();
        for (int i = 0; i < haveRoomMatrix.Count; i++)
        {
            for (int j = 0; j < haveRoomMatrix[i].Length; j++)
            {
                if (haveRoomMatrix[i][j] == 1)
                {
                    linkedRoom = new List<RoomInfo>();
                    RoomIsland.Add(linkedRoom);
                    DestroyConnected(i, j, linkedRoom);
                }
            }
        }
        Max = 0;
        MaxindexNumber = 0;
        for (int i = 0; i < RoomIsland.Count; i++)
        {
            if (Max < RoomIsland[i].Count)
            {
                Max = RoomIsland[i].Count;
                MaxindexNumber = i;
            }
        }
        for (int i = 0; i < RoomIsland[MaxindexNumber].Count; i++)
        {
            if (RoomIsland[MaxindexNumber][i].IsWayNull != true)
            {
                RoomIsland[MaxindexNumber][i].IsMainland = true;
            }
        }
        RoomIsland.RemoveAt(MaxindexNumber);
        for (int i = 0; i < RoomIsland.Count; i++)
        {
            Island.Add(RoomIsland[i]);
        }
    }

    public void DestroyConnected(int i, int j, List<RoomInfo> _linkedRoom)
    {
        haveRoomMatrix[i][j] = 0;
        _linkedRoom.Add(MainRoom[i * (int)Mathf.Sqrt(MainRoom.Count) + j].GetComponent<RoomInfo>());
        if (i > 0)
        {
            if (haveRoomMatrix[i - 1][j] == 1)
            {
                DestroyConnected(i - 1, j, _linkedRoom);
            }
        }
        if (j > 0)
        {
            if (haveRoomMatrix[i][j - 1] == 1)
            {
                DestroyConnected(i, j - 1, _linkedRoom);
            }
        }
        if (i < haveRoomMatrix.Count - 1)
        {
            if (haveRoomMatrix[i + 1][j] == 1)
            {
                DestroyConnected(i + 1, j, _linkedRoom);
            }
        }
        if (j < haveRoomMatrix[i].Length - 1)
        {
            if (haveRoomMatrix[i][j + 1] == 1)
            {
                DestroyConnected(i, j + 1, _linkedRoom);
            }
        }
        return;
    }

    public void FindBuildBridge()
    {
        foreach(RoomInfo room in GameObject.FindObjectsOfType<RoomInfo>())
        {
            room.GetCanBuildArea();
        }
    }

    public void GetIslandList()
    {
        haveRoomList.Clear();
        for(int i =0; i< MainRoom.Count; i++)
        {
            haveRoomList.Add(MainRoom[i].GetComponent<RoomInfo>().haveRoom ?
               MainRoom[i].GetComponent<RoomInfo>().IsMainland ? 1:2 : 
               MainRoom[i].GetComponent<RoomInfo>().IsCanBuild ? 0:3);
        }
    }

    public void FindPath()
    {
        TargetNodeList = new List<Node>();
        StartNodeList = new List<Node>();
        Route = new List<List<Node>>();
        NodeArray = new Node[haveRoomMatrix.Count, haveRoomMatrix.Count];
        for (int i = 0; i < haveRoomMatrix.Count; i++)
        {
            for (int j = 0; j < haveRoomMatrix[i].Length; j++)
            {
                bool isWall = false;
                if (haveRoomMatrix[i][j] == 1)
                {
                    isWall = true;
                    NodeArray[i, j] = new Node(isWall, i, j);
                    TargetNodeList.Add(NodeArray[i, j]);

                }
                else if (haveRoomMatrix[i][j] == 2 )
                {
                    isWall = true;
                    NodeArray[i, j] = new Node(isWall, i, j);
                    StartNodeList.Add(NodeArray[i, j]);
                }
                else if(haveRoomMatrix[i][j] == 3)
                {
                    isWall = true;
                    NodeArray[i, j] = new Node(isWall, i, j);
                }
                else
                {
                    isWall = false;
                    NodeArray[i, j] = new Node(isWall, i, j);
                }
            }
        }

        for (int i = 0; i < StartNodeList.Count; i++)
        {
            for (int j = 0; j < TargetNodeList.Count; j++)
            {
                OpenList = new List<Node>() { StartNodeList[i] };
                StartNodeList[i].isWall = false;
                TargetNodeList[j].isWall = false;
                StartFind(OpenList,Route ,StartNodeList[i], TargetNodeList[j]);
                StartNodeList[i].isWall = true;
                TargetNodeList[j].isWall = true;
            }
        }

       
    }

    public void StartFind(List<Node> OpenList,List<List<Node>> Route, Node StartNode, Node TargetNode)
    {

        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                if (FinalNodeList.Count > 0)
                {
                    Route.Add(FinalNodeList);
                }
                return;
            }
            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1,ClosedList);
            OpenListAdd(CurNode.x + 1, CurNode.y,ClosedList);
            OpenListAdd(CurNode.x, CurNode.y - 1,ClosedList);
            OpenListAdd(CurNode.x - 1, CurNode.y,ClosedList);
        }
        
    }

    void OpenListAdd(int checkX, int checkY,List<Node> ClosedList)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= 0 
            && checkX < haveRoomMatrix.Count 
            && checkY >= 0 
            && checkY < haveRoomMatrix.Count 
            && !ClosedList.Contains(NodeArray[checkX, checkY]))
        {
            if (!NodeArray[checkX, checkY].isWall )

            {
                // 이동 중에 수직수평 장애물이 있으면 안됨
                if (NodeArray[CurNode.x, checkY].isWall || NodeArray[checkX, CurNode.y].isWall) return;

                // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
                Node NeighborNode = NodeArray[checkX, checkY];
                int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


                // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
                if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.G = MoveCost;
                    NeighborNode.H = (Mathf.Abs(NeighborNode.x) + Mathf.Abs(NeighborNode.y)) * 10;
                    NeighborNode.ParentNode = CurNode;
                    OpenList.Add(NeighborNode);
                }
            }
        }
    }

    public void FindShortestPath()
    {
        RoomInfo[] rooms = GameObject.FindObjectsOfType<RoomInfo>();
        List<List<Node>> SortedRoute = Route.OrderBy(x => x.Count).ToList();
        for (int i = 0; i < SortedRoute.Count; i++)
        {
            for (int j = 0; j < SortedRoute[i].Count; j++)
            {
                if (SortedRoute[i][j].isWall == false)
                {
                    
                    foreach (RoomInfo room in rooms)
                    {
                        room.GetChildDoor();
                        room.BuildWay();
                        room.FindPath();
                        if (room.RoomID == (int)((SortedRoute[i][j].x) * haveRoomMatrix.Count + SortedRoute[i][j].y))
                        {
                            if (room.haveRoom == false)
                            {
                                GameObject CrossRoadGo = Instantiate(CrossRoom, room.transform.position, Quaternion.identity) as GameObject;
                                CrossRoadGo.transform.parent = room.transform;
                                room.haveRoom = true;
                                room.IsMainland = true;
                                room.IsField = true;
                            }
                        }
                        room.GetChildDoor();
                        room.BuildWay();
                        room.FindPath();
                        GethaveRoomList();
                        GethaveRoomMatrix();
                        DetectIsland();
                        if (MainlandComplete())
                        {
                            return;
                        }
                    }

                }
            }
        }
    }

    public bool MainlandComplete()
    {
        foreach (RoomInfo room in GameObject.FindObjectsOfType<RoomInfo>())
        {
            if (room.IsMainland ==false && room.haveRoom == true)
            {
                return false;
            }
        }
        return true;
    }

    public void CloseDoor()
    {
        foreach (WayBuilder Door in GameObject.FindObjectsOfType<WayBuilder>())
        {
            switch (Door.DD)
            {
                case DoorDirect.EAST:
                    Door.DestroyDoor(NormalRoom.Walls[0], Door.transform.root);
                    break;
                case DoorDirect.WEST:
                    Door.DestroyDoor(NormalRoom.Walls[1], Door.transform.root);
                    break;
                case DoorDirect.SOUTH:
                    Door.DestroyDoor(NormalRoom.Walls[2], Door.transform.root);
                    break;
                case DoorDirect.NORTH:
                    Door.DestroyDoor(NormalRoom.Walls[3], Door.transform.root);
                    break;
            }

        }
    }

    public void DestroyEmptyRoom() { 
        foreach(RoomInfo room in GameObject.FindObjectsOfType<RoomInfo>())
        {
            if (room.IsWayNull == true)
            {
                if (room.transform.childCount != 0)
                {
                    for (int i = 0; i < room.transform.childCount; i++)
                    {
                        Destroy(room.transform.GetChild(i).gameObject);
                    }
                    room.haveRoom = false;
                }
            }
        }
    }

    public void BuildNavmesh()
    {
        foreach (RoomInfo room in GameObject.FindObjectsOfType<RoomInfo>())
        { 
            if(room.IsField != true && room.haveRoom == true)
            {
                room.BakeNavmesh();
            }
        }
    }

    public void SetRoomDiretType()
    {
        foreach(RoomInfo room in GameObject.FindObjectsOfType<RoomInfo>())
        {
            room.SethaveWallCount();
        }
    }

    #endregion

    #region 플레이어 만들기 ~ 

    public Vector3 GetSetPlayerPos()
    {
        TileInfo[] Tiles = GameObject.FindObjectsOfType<TileInfo>();
        int RandomPos = GetRandom();
        if(Tiles[RandomPos].transform.root.GetComponent<RoomInfo>().IsMainland ==true && Tiles[RandomPos].transform.root.GetComponent<RoomInfo>().IsWayNull == false)
        {
            Tiles[RandomPos].transform.root.GetComponent<RoomInfo>().StartPlayerHere = true;
            Tiles[RandomPos].transform.root.GetComponent<RoomInfo>().IsPlayerHere = true;
            Tiles[RandomPos].transform.root.GetComponent<RoomInfo>().HaveBeenHere = true;
            return Tiles[RandomPos].transform.position;
        }
        else
        {
            GetSetPlayerPos();
            return Vector3.zero;
        }
    }

    public int GetRandom()
    {
        TileInfo[] Tiles = GameObject.FindObjectsOfType<TileInfo>();
        int RandomPos = Random.Range(0, Tiles.Length);
        return RandomPos;
    }

    #endregion

    #region  미니맵 만들기 ~

    public void DrawMiniMap()
    {
        MiniMapController.Instance.CreateMiniMap();
        MiniMapController.Instance.InitMiniMap();
    }

    #endregion

    #region 몬스터 만들기 ~

    public void MakeMonster()
    {
        foreach (RoomInfo room in GameObject.FindObjectsOfType<RoomInfo>())
        {
            room.InMonsterCount = 0;
            if (room.StartPlayerHere == false
                && room.IsSpecialRoom == false
                && room.haveRoom == true
                && room.IsField == false)
            {
                room.MonsterCount = MonsterCount;
                List<TileInfo> TilesList = new List<TileInfo>();
                TilesList.AddRange(room.transform.GetComponentsInChildren<TileInfo>());
                for (int i = 0; i < room.MonsterCount; i++)
                {
                    int ChoiceMonsterType = Random.Range(0, 2);
                    int RandomPos = Random.Range(0, TilesList.Count);
                    switch ((MonsterType)ChoiceMonsterType)
                    {
                        //작은 몬스터
                        case MonsterType.Small:
                            int RandomSmallMonster = Random.Range(0, SmallMonsters.Length);
                            GameObject MonsterGo = Instantiate(SmallMonsters[RandomSmallMonster], TilesList[RandomPos].transform);
                            MonsterGo.transform.parent = room.transform;
                            room.InMonsterCount++;
                            break;
                        // 중간 몬스터
                        case MonsterType.Medium:
                            int RandomMediumMonster = Random.Range(0, MediumMonsters.Length);
                            GameObject MonsterGo1 = Instantiate(MediumMonsters[RandomMediumMonster], TilesList[RandomPos].transform);
                            MonsterGo1.transform.parent = room.transform;
                            room.InMonsterCount++;
                            break;
                    }
                }
            }
        }
    }
    #endregion

    #region 게임진행 세팅

    public void Portalinit()
    {
        foreach(RoomInfo room in FindObjectsOfType<RoomInfo>())
        {
            room.StartCoroutine("OpenPortal");
            foreach (WayBuilder way in room.transform.GetComponentsInChildren<WayBuilder>())
            {
                if (way.transform.root.GetComponent<RoomInfo>().IsShopRoom == false)
                {
                    if (way.Gotothis.transform.root.GetComponent<RoomInfo>().IsShopRoom)
                    {
                        way.gameObject.transform.GetChild(1).GetComponent<PortalColorController>().SetColor(Color.Lerp(Color.red, Color.yellow, 0.7f));
                    }
                    if (way.Gotothis.transform.root.GetComponent<RoomInfo>().IsBossRoom)
                    {
                        way.gameObject.transform.GetChild(1).GetComponent<PortalColorController>().SetColor(Color.red);
                    }
                    
                    //몬스터가 없고 상점이 아닐시 문열림 ( 플레이어 방하고 복도 ) 
                    if (room.InMonsterCount == 0 && room.IsShopRoom == false)
                    {
                        way.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    
    public void InitSpreadObject()
    {
        CanSpreadSwordRooms =new List<RoomInfo>();
        List<RoomInfo> TempRoomList;
        foreach(RoomInfo Room in GameObject.FindObjectsOfType<RoomInfo>())
        {
            if(Room.IsBossRoom==false && Room.IsShopRoom ==false &&Room.IsPlayerHere ==false && Room.IsField ==false && Room.haveRoom ==true)
            {
                CanSpreadSwordRooms.Add(Room);
            }
        }
        TempRoomList = CanSpreadSwordRooms;
        for (int i = 0; i < SwordMaxCount-2 ; i++) 
        {
            int RandomRoom = Random.Range(0, TempRoomList.Count-1);
            TempRoomList[RandomRoom].CreateSwordPiece(SwordPiece);
            TempRoomList.RemoveAt(RandomRoom);
        }
    }

    public void SpreadSwordPiece()
    {
        List<RoomInfo> TempRoomList;
        TempRoomList = CanSpreadSwordRooms;
        int RandomRoom = Random.Range(0, TempRoomList.Count - 1);
        TempRoomList[RandomRoom].CreateSwordPiece(SwordPiece);
    }

    #endregion
}


public class MapMatrix
{
    public bool haveRoom = false;
    public Vector3 RoomPos = Vector3.zero;
}
