using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CanBuildWayType
{
    // U = UP , D = DOWN
    // L = LEFT, R =RIGHT

    Default,
    RIGHT,
    LEFT,
    RL,
    DOWN,
    RD,
    LD,
    RLD,
    UP,
    RU,
    LU,
    RLU,
    UD,
    UDR,
    UDL,
    ALL
}

public class RoomInfo : MonoBehaviour
{
    public int RoomID;
    public int InMonsterCount;
    public int MonsterCount;
    public bool haveRoom;
    public bool BossRoomhaveDoor;
    public bool IsField;
    public bool IsWayNull;
    public bool IsMainland;
    public bool IsCanBuild;
    
    public bool StartPlayerHere;
    public bool IsSpecialRoom;
    public bool ThisRoomClear;

    public bool HaveBeenHere;
    public bool IsSwordHere;
    public bool IsBossRoom;
    public bool IsShopRoom;
    public bool IsPlayerHere;


    private CanBuildWayType RoomDirect;

    private int haveRWay;
    private int haveLWay;
    private int haveDWay;
    private int haveUWay;

    public NavMeshSurface NMSF;

    [SerializeField]
    public List<WayBuilder> ChildDoorList;

    private void Start()
    {
        ThisRoomClear = false;
        haveRWay = 0;
        haveLWay = 0;
        haveDWay = 0;
        haveUWay = 0;
    }
     

    IEnumerator IsClear()
    {
        yield return null;

        foreach (WayBuilder Door in ChildDoorList)
        {
            Door.OpenDoor();
        }
        ThisRoomClear = true;
        StartCoroutine("IsClear");
    }

    public void AddChildDoor(WayBuilder ChildDoor)
    {
        ChildDoorList.Add(ChildDoor);
    }
    public void BuildWay()
    {
        GetChildDoor();
        foreach (WayBuilder Door in ChildDoorList)
        {
            Door.linkPortal();
        }
    }
    public void GetChildDoor()
    {
        if (ChildDoorList.Count == 0)
        {
            ChildDoorList.AddRange(transform.GetComponentsInChildren<WayBuilder>());
        }
    }
    public void FindPath()
    {
        int WayCount = 0;
        foreach (WayBuilder ChildDoor in ChildDoorList)
        {
            if (ChildDoor.haveWay == true)
            {
                WayCount++;
            }
        }
        if (WayCount == 0)
        {
            IsWayNull = true;
        }
        else
        {
            IsWayNull = false;
        }
    }

    public void GetCanBuildArea()
    {
        if (!IsMainland)
        {
            foreach (WayBuilder Door in ChildDoorList)
            {
                Door.FindCanBuild();
            }
        }
    }

    public void BuildCrossRoad(GameObject CrossRoad)
    {
        
        GameObject CrossRoadGo = Instantiate(CrossRoad, transform.position, Quaternion.identity) as GameObject;
        CrossRoadGo.transform.parent = this.gameObject.transform;
        this.haveRoom = true;
    }

    public void CloseChildrenDoor()
    {
        if (IsField == false && IsSpecialRoom == false && ThisRoomClear == false)
        foreach (WayBuilder Door in ChildDoorList)
        {
            Door.CloseDoor();
        }
    }

    public List<int> SearchShopRoom()
    {
        List<int> RoomID = new List<int>();
        
        foreach(WayBuilder Door in ChildDoorList)
        {
           RoomID.Add(Door.FindShopRoom());
        }
        return RoomID;
    }

    public void SethaveWallCount()
    {
        foreach (WayBuilder Door in ChildDoorList)
        {
            switch (Door.DD)
            {
                case DoorDirect.EAST:
                    haveRWay = 1;
                    break;
                case DoorDirect.WEST:
                    haveLWay = 2;
                    break;
                case DoorDirect.SOUTH:
                    haveDWay = 4;
                    break;
                case DoorDirect.NORTH:
                    haveUWay = 8;
                    break;
            }
        }
    }

    public int GetMiniMapType()
    {
        switch (haveRWay | haveLWay | haveDWay | haveUWay)
        {
            //Default
            case 0:
                RoomDirect = CanBuildWayType.Default;
                break;
            //RIGHT
            case 1:
                RoomDirect = CanBuildWayType.RIGHT;
                break;
            //LEFT
            case 2:
                RoomDirect = CanBuildWayType.LEFT;
                break;
            //RL
            case 3:
                RoomDirect = CanBuildWayType.RL;
                break;
            //DOWN
            case 4:
                RoomDirect = CanBuildWayType.DOWN;
                break;
            //RD
            case 5:
                RoomDirect = CanBuildWayType.RD;
                break;
            //LD
            case 6:
                RoomDirect = CanBuildWayType.LD;
                break;
            //RLD
            case 7:
                RoomDirect = CanBuildWayType.RLD;
                break;
            //UP
            case 8:
                RoomDirect = CanBuildWayType.UP;
                break;
            //RU
            case 9:
                RoomDirect = CanBuildWayType.RU;
                break;
            //LU
            case 10:
                RoomDirect = CanBuildWayType.LU;
                break;
            //RLU
            case 11:
                RoomDirect = CanBuildWayType.RLU;
                break;
            //UD
            case 12:
                RoomDirect = CanBuildWayType.UD;
                break;
            //UDR
            case 13:
                RoomDirect = CanBuildWayType.UDR;
                break;
            //UDL
            case 14:
                RoomDirect = CanBuildWayType.UDL;
                break;
            //ALL
            case 15:
                RoomDirect = CanBuildWayType.ALL;
                break;
        }
        return (int)RoomDirect;
    }

    public void BakeNavmesh()
    {
        if (IsShopRoom == false)
        {
            NMSF.BuildNavMesh();
        }
    }

    public void CreateSwordPiece(GameObject SwordPiece)
    {
            Debug.Log(RoomID);
            TileInfo[] Tiles = transform.GetComponentsInChildren<TileInfo>();
            int RandomNumber = Random.Range(0, Tiles.Length - 1);
            GameObject PieceGo = Instantiate(SwordPiece, Tiles[RandomNumber].transform.position, Quaternion.identity);
            IsSwordHere = true;
            MiniMapController.Instance.ReloadMiniMap();
    }

    IEnumerator OpenPortal()
    {
        yield return new WaitUntil(() => this.transform.GetComponentsInChildren<ObjectStatController>().Length == 0);
        if (IsShopRoom == false)
        {
            foreach (WayBuilder way in transform.GetComponentsInChildren<WayBuilder>())
            {
                Sequence DoorSequence;
                DoorSequence = DOTween.Sequence().OnStart(() =>
                {
                    way.gameObject.transform.GetChild(0).transform.DOMoveY(transform.position.y - 11f, 2f);
                }).Join(way.gameObject.transform.GetChild(0).transform.DOShakePosition(3f, new Vector3(2f, 0, 0)));
             
                //SoundManager.Instance.PlaySound("드르륵");
            }
            yield return new WaitForSeconds(2f);
            foreach (WayBuilder way in transform.GetComponentsInChildren<WayBuilder>())
            {
                way.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
