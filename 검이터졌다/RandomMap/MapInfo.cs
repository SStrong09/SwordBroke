
using System.Collections.Generic;
using UnityEngine;

enum BuildType
{
    Empty,
    Tile,
    Wall,
    Pillars
}


public enum MapMaterialType
{
    City,
    Dark,
    Forest,
    Ice,
    Light,
    Ruin,
    Sea,
    Volcano
}

public enum MonsterType
{
    Small,
    Medium
}


[System.Serializable]
//이 스크립터블 오브젝트는 유니티 에디터 상에서 생성 가능합니다.
//경로는 "New Object/DataBase System/Stage/Map"
[CreateAssetMenu(fileName = "New Map", menuName = "DataBase System/Stage/Map")]
public class MapInfo : ScriptableObject
{
    [Header("맵 타일")]
    public GameObject[] Tiles;
    public GameObject[] Walls;
    public GameObject[] Doors;
    public GameObject Pillars;


    [Header("맵 팔레트")]
    public Material[] MapMaterial;

    private float Spacing = 9;

    private int CountEWall;
    private int CountWWall;
    private int CountNWall;
    private int CountSWall;

    private int CountEDoor;
    private int CountWDoor;
    private int CountNDoor;
    private int CountSDoor;

    private List<string[]> TileInfo;
    
    public void Init()
    {
        CountEWall = 1;
        CountWWall = 1;
        CountNWall = 1;
        CountSWall = 1;

        CountEDoor = 2;
        CountWDoor = 2;
        CountNDoor = 2;
        CountSDoor = 2;

        TileInfo = new List<string[]>();
    }

    public void BuildRandomTile(Vector3 BuildPosition,int RandomMapNumber,GameObject Room)
    {
        Init();
        TileInfo =BuildManager.Instance.PMI.GetRandomMap(RandomMapNumber);

        //맵 정보를 가져옴 ... 텍스트맵 
        for (int i = 0; i < UnityEngine.Random.Range(0, 3); i++)
        {
            TileInfo = BuildManager.Instance.PMI.RotateMap(TileInfo);
        }


        //맵 정보를 토대로 맵을 만듬 ㅋㅋ
        for (int x=0; x<TileInfo.Count; x++)
        {
            for(int y=0; y < TileInfo[x].Length; y++)
            {
                var pos = new Vector3((x * Spacing)+BuildPosition.x-22.5f, 0, (y * Spacing) + BuildPosition.z-22.5f);

                // 맵 정보로 스위치를 돌림
                // Empty =0 , Tile = 1, Wall =2 , Pillars =3
                switch (int.Parse(TileInfo[x][y]))
                {
                    //빈공간
                    case (int)BuildType.Empty:

                        break;

                    //타일
                    case (int)BuildType.Tile:
                        GameObject NodeTile = Instantiate(Tiles[UnityEngine.Random.Range(0, Tiles.Length)], pos, Quaternion.identity) as GameObject;
                        NodeTile.transform.parent = Room.transform;
                        break;

                    //벽
                    case (int)BuildType.Wall:

                        //'동'쪽 벽 //0
                        if (x + 1 == TileInfo.Count)
                        {
                            //'동'쪽에 만들어진 문이 없을때
                            if (CountEWall != 0)
                            {
                                //랜덤확률로 문이 만들어질경우
                                if (CountEWall < Random.Range(0, CountEWall) + CountEDoor)
                                {
                                    GameObject NodeDoor = Instantiate(Doors[0], pos, Quaternion.identity) as GameObject;
                                    NodeDoor.transform.parent = Room.transform;
                                    NodeDoor.GetComponent<WayBuilder>().DD = DoorDirect.EAST;
                                    Room.GetComponent<RoomInfo>().AddChildDoor(NodeDoor.GetComponent<WayBuilder>());
                                    CountEWall = 0;
                                }
                                //랜덤확률로 문이 안만들어질경우
                                else
                                {
                                    GameObject NodeWall = Instantiate(Walls[0], pos, Quaternion.identity) as GameObject;
                                    NodeWall.transform.parent = Room.transform;
                                    CountEDoor++;
                                }
                            }

                            //'동'쪽에 만들어진 문이 이미 있을때
                            else
                            {
                                GameObject NodeWall = Instantiate(Walls[0], pos, Quaternion.identity) as GameObject;
                                NodeWall.transform.parent = Room.transform;
                            }
                        }

                        //'서'쪽 벽 //1
                        if (x == 0)
                        {
                            //'서'쪽에 만들어진 문이 없을때
                            if (CountWWall != 0)
                            {
                                //랜덤확률로 문이 만들어질경우
                                if (CountWWall < Random.Range(0, CountWWall) + CountWDoor)
                                {
                                    GameObject NodeDoor = Instantiate(Doors[1], pos, Quaternion.identity) as GameObject;
                                    NodeDoor.transform.parent = Room.transform;
                                    NodeDoor.GetComponent<WayBuilder>().DD = DoorDirect.WEST;
                                    Room.GetComponent<RoomInfo>().AddChildDoor(NodeDoor.GetComponent<WayBuilder>());
                                    CountWWall = 0;
                                }
                                //랜덤확률로 문이 안만들어질경우
                                else
                                {
                                    GameObject NodeWall = Instantiate(Walls[1], pos, Quaternion.identity) as GameObject;
                                    NodeWall.transform.parent = Room.transform;
                                    CountWDoor++;
                                }
                            }

                            //'서'쪽에 만들어진 문이 이미 있을때
                            else
                            {
                                GameObject NodeWall = Instantiate(Walls[1], pos, Quaternion.identity) as GameObject;
                                NodeWall.transform.parent = Room.transform;
                            }

                        }

                        //남쪽 벽
                        if (y == 0)
                        {
                            //'남'쪽에 만들어진 문이 없을때
                            if (CountSWall != 0)
                            {
                                //랜덤확률로 문이 만들어질경우
                                if (CountSWall < Random.Range(0, CountSWall) + CountSDoor)
                                {
                                    GameObject NodeDoor = Instantiate(Doors[2], pos, Quaternion.identity) as GameObject;
                                    NodeDoor.transform.parent = Room.transform;
                                    NodeDoor.GetComponent<WayBuilder>().DD = DoorDirect.SOUTH;
                                    Room.GetComponent<RoomInfo>().AddChildDoor(NodeDoor.GetComponent<WayBuilder>());
                                    CountSWall = 0;
                                }
                                //랜덤확률로 문이 안만들어질경우
                                else
                                {
                                    GameObject NodeWall = Instantiate(Walls[2], pos, Quaternion.identity) as GameObject;
                                    NodeWall.transform.parent = Room.transform;
                                    CountSDoor++;
                                }
                            }

                            //'남'쪽에 만들어진 문이 이미 있을때
                            else
                            {
                                GameObject NodeWall = Instantiate(Walls[2], pos, Quaternion.identity) as GameObject;
                                NodeWall.transform.parent = Room.transform;
                            }
                        }

                        //북쪽 벽 //3
                        if (y + 1 == TileInfo.Count)
                        {
                            //'북'쪽에 만들어진 문이 없을때
                            if (CountNWall != 0)
                            {
                                //랜덤확률로 문이 만들어질경우
                                if (CountNWall < Random.Range(0, CountNWall) + CountNDoor)
                                {
                                    GameObject NodeDoor = Instantiate(Doors[3], pos, Quaternion.identity) as GameObject;
                                    NodeDoor.transform.parent = Room.transform;
                                    NodeDoor.GetComponent<WayBuilder>().DD = DoorDirect.NORTH;
                                    Room.GetComponent<RoomInfo>().AddChildDoor(NodeDoor.GetComponent<WayBuilder>());
                                    CountNWall = 0;
                                }
                                //랜덤확률로 문이 안만들어질경우
                                else
                                {
                                    GameObject NodeWall = Instantiate(Walls[3], pos, Quaternion.identity) as GameObject;
                                    NodeWall.transform.parent = Room.transform;
                                    CountNDoor++;
                                }
                            }
                            //'북'쪽에 만들어진 문이 이미 있을때
                            else
                            {
                                GameObject NodeWall = Instantiate(Walls[3], pos, Quaternion.identity) as GameObject;
                                NodeWall.transform.parent = Room.transform;
                            }
                        }
                        break;

                    //꼭짓점 기둥
                    case (int)BuildType.Pillars:
                        GameObject NodePillar = Instantiate(Pillars, pos, Quaternion.identity) as GameObject;
                        NodePillar.transform.parent = Room.transform;
                        break;
                }
            }
        }
        Room.GetComponent<RoomInfo>().haveRoom = true;
    }
    public void PalletteSwap()
    {
        // 팔레트 스왑
        // Tile
        foreach (GameObject Tile in Tiles)
        {
            if (Tile.transform.GetChild(0).GetComponent<MeshRenderer>() != null)
            {
                Tile.transform.GetChild(0).GetComponent<MeshRenderer>().material = MapMaterial[(int)BuildType.Tile - 1];
            }
        }
        // Door
        foreach (GameObject Door in Doors)
        {
            if (Door.transform.GetChild(2).GetComponent<MeshRenderer>() != null)
            {
                Door.transform.GetChild(2).GetComponent<MeshRenderer>().material = MapMaterial[(int)BuildType.Tile - 1];
            }
        }
        // Wall
        foreach (GameObject Wall in Walls)
        {
            if (Wall.transform.GetChild(0).GetComponent<MeshRenderer>() != null)
            {
                Wall.transform.GetChild(0).GetComponent<MeshRenderer>().material = MapMaterial[(int)BuildType.Tile - 1];
            }
            if (Wall.transform.GetChild(1).GetComponent<MeshRenderer>() != null)
            {
                Wall.transform.GetChild(1).GetComponent<MeshRenderer>().material = MapMaterial[(int)BuildType.Wall - 1];
            }
        }

        if (Pillars.transform.GetChild(1).GetComponent<MeshRenderer>() != null)
        {
            Pillars.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = MapMaterial[(int)BuildType.Tile - 1];
            Pillars.transform.GetChild(1).GetComponent<MeshRenderer>().material = MapMaterial[(int)BuildType.Wall - 1];
        }

        //Door
    }


}


