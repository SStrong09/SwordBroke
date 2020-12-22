
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Pallette
{ 
    [SerializeField]
    private UnityEngine.Color color;
    MapMaterialType MMT;
}


public class MiniMapController : MonoBehaviour
{
    #region Singleton
    private static MiniMapController instance;
    public static MiniMapController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<MiniMapController>();
            return instance;
        }
    }
    #endregion
    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면 
        if (Instance != this)
        {
            // 파괴
            Destroy(gameObject);
        }
    }

    // UI 상의 조각 이미지
    //[SerializeField] private Image[] piece_image;
    public GameObject MiniMapNode;
    public Sprite[] png;

    [SerializeField]
    private Dictionary<string, MiniMapNodeController> MiniMapList;

    private float NormalSize = 3.128f;

    public void CreateMiniMap()
    {
        MiniMapList = new Dictionary<string, MiniMapNodeController>();
        float increaseSpacing=0.256f;
        RoomInfo[] Rooms = GameObject.FindObjectsOfType<RoomInfo>();
        foreach (RoomInfo room in Rooms)
        {
            GameObject MiniNode = Instantiate(MiniMapNode, Vector3.zero, Quaternion.identity) as GameObject;
            MiniMapList.Add(room.RoomID.ToString(),MiniNode.GetComponent<MiniMapNodeController>());
            MiniNode.transform.SetParent(this.transform);
            MiniNode.GetComponent<RectTransform>().localScale = new Vector3 (1f*(NormalSize-increaseSpacing*Mathf.Sqrt(Rooms.Length)),1f * (NormalSize - increaseSpacing * Mathf.Sqrt(Rooms.Length)), 0);
            float Spacing = MiniNode.GetComponent<Image>().rectTransform.rect.width * MiniNode.GetComponent<RectTransform>().localScale.x;
            MiniNode.transform.localPosition = new Vector3(( room.transform.position.x/300 )* Spacing, (room.transform.position.z / 300) * Spacing, 0);
            MiniNode.GetComponent<MiniMapNodeController>().RoomID = room.RoomID;
            switch (room.GetMiniMapType())
            {
                //Default
                case 0:
                    MiniNode.GetComponent<Image>().sprite = png[0];
                    MiniNode.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    break;
                //RIGHT
                case 1:
                    MiniNode.GetComponent<Image>().sprite = png[1];
                    break;
                //LEFT
                case 2:
                    MiniNode.GetComponent<Image>().sprite = png[2];
                    break;
                //RL
                case 3:
                    MiniNode.GetComponent<Image>().sprite = png[3];
                    break;
                //DOWN
                case 4:
                    MiniNode.GetComponent<Image>().sprite = png[4];
                    break;
                //RD
                case 5:
                    MiniNode.GetComponent<Image>().sprite = png[5];
                    break;
                //LD
                case 6:
                    MiniNode.GetComponent<Image>().sprite = png[6];
                    break;
                //RLD
                case 7:
                    MiniNode.GetComponent<Image>().sprite = png[7];
                    break;
                //UP
                case 8:
                    MiniNode.GetComponent<Image>().sprite = png[8];
                    break;
                //RU
                case 9:
                    MiniNode.GetComponent<Image>().sprite = png[9];
                    break;
                //LU
                case 10:
                    MiniNode.GetComponent<Image>().sprite = png[10];
                    break;
                //RLU
                case 11:
                    MiniNode.GetComponent<Image>().sprite = png[11];
                    break;
                //UD
                case 12:
                    MiniNode.GetComponent<Image>().sprite = png[12];
                    break;
                //UDR
                case 13:
                    MiniNode.GetComponent<Image>().sprite = png[13];
                    break;
                //UDL
                case 14:
                    MiniNode.GetComponent<Image>().sprite = png[14];
                    break;
                //ALL
                case 15:
                    MiniNode.GetComponent<Image>().sprite = png[15];
                    break;
            }
        }
    }

    public void InitMiniMap()
    {
        RoomInfo[] Rooms = GameObject.FindObjectsOfType<RoomInfo>();
        foreach (RoomInfo room in Rooms)
        {
            if (room.haveRoom == true)
            {
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Blank, !room.HaveBeenHere);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Boss, room.IsBossRoom);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Player, room.IsPlayerHere);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Sword, room.IsSwordHere);
                if (room.IsPlayerHere == true)
                {
                    OnIconMiniMapShop(room.SearchShopRoom());
                }
                
            }
            else
            {
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Blank,   false);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Boss, false);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Player, false);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Shop, false);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Sword, false);
            }
        }
    }

    public void ReloadMiniMap()
    {
        RoomInfo[] Rooms = GameObject.FindObjectsOfType<RoomInfo>();
        foreach (RoomInfo room in Rooms)
        {
            if (room.haveRoom == true)
            {
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Blank, !room.HaveBeenHere);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Boss, room.IsBossRoom);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Player, room.IsPlayerHere);
                MiniMapList[room.RoomID.ToString()].OnOffIcon(IconType.Sword, room.IsSwordHere);
            }
        }
    }

    public void OnIconMiniMapShop(List<int> RoomID)
    {
        for(int i=0; i<RoomID.Count; i++)
        {
            if (RoomID[i] != 99)
            {
                MiniMapList[RoomID[i].ToString()].OnOffIcon(IconType.Shop, true);
            }
        }
    }
}
