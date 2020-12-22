using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum DoorDirect
{
    EAST,
    WEST,
    SOUTH,
    NORTH
}

public class WayBuilder : MonoBehaviour
{
    public DoorDirect DD;
    public bool haveWay;
    public Transform Gotothis;
    public int RayLength;
    // Start is called before the first frame update
    void Start()
    {
        //transform.GetChild(1).gameObject.SetActive(false);
    }

    public void linkPortal()
    {
        RaycastHit[] hitInfo;

        if (haveWay != true)
        {
            switch (DD)
            {
                //이 문이 동쪽문일때
                case DoorDirect.EAST:
                    hitInfo = Physics.RaycastAll(transform.position, Vector3.right, 200f);

                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.gameObject.CompareTag("Room"))
                        {
                            foreach (WayBuilder Door in hit.collider.GetComponent<RoomInfo>().ChildDoorList)
                            {
                                if (hit.transform.GetComponent<RoomInfo>().IsBossRoom == false)
                                {
                                    if (this.transform.root.GetComponent<RoomInfo>().IsBossRoom != true)
                                    {
                                        //서쪽문하고 연결한다.
                                        if (Door.DD == DoorDirect.WEST)
                                        {
                                            Door.SetPortalPos(this.transform);
                                            SetPortalPos(Door.transform);
                                            haveWay = true;
                                            hit.transform.GetComponent<RoomInfo>().IsWayNull = false;
                                            this.transform.root.GetComponent<RoomInfo>().IsWayNull = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if(hit.transform.GetComponent<RoomInfo>().BossRoomhaveDoor == false)
                                    {
                                        //서쪽문하고 연결한다.
                                        if (Door.DD == DoorDirect.WEST)
                                        {
                                            Door.SetPortalPos(this.transform);
                                            SetPortalPos(Door.transform);
                                            haveWay = true;
                                            hit.transform.GetComponent<RoomInfo>().IsWayNull = false;
                                            hit.transform.GetComponent<RoomInfo>().BossRoomhaveDoor = true;
                                            this.transform.root.GetComponent<RoomInfo>().IsWayNull = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                //이 문이 서쪽문일때
                case DoorDirect.WEST:
                    hitInfo = Physics.RaycastAll(transform.position, Vector3.left, 200f);
                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.gameObject.CompareTag("Room"))
                        {
                            foreach (WayBuilder Door in hit.collider.GetComponent<RoomInfo>().ChildDoorList)
                            {
                                if (hit.transform.GetComponent<RoomInfo>().IsBossRoom == false)
                                {
                                    if (this.transform.root.GetComponent<RoomInfo>().IsBossRoom != true)
                                    {
                                        //동쪽문하고 연결한다.
                                        if (Door.DD == DoorDirect.EAST)
                                        {
                                            Door.SetPortalPos(this.transform);
                                            SetPortalPos(Door.transform);
                                            haveWay = true;
                                            hit.transform.GetComponent<RoomInfo>().IsWayNull = false;
                                            this.transform.root.GetComponent<RoomInfo>().IsWayNull = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (hit.transform.GetComponent<RoomInfo>().BossRoomhaveDoor == false)
                                    {
                                        //동쪽문하고 연결한다.
                                        if (Door.DD == DoorDirect.EAST)
                                        {
                                            Door.SetPortalPos(this.transform);
                                            SetPortalPos(Door.transform);
                                            haveWay = true;
                                            hit.transform.GetComponent<RoomInfo>().IsWayNull = false;
                                            hit.transform.GetComponent<RoomInfo>().BossRoomhaveDoor = true;
                                            this.transform.root.GetComponent<RoomInfo>().IsWayNull = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;

                //이 문이 남쪽문일때
                case DoorDirect.SOUTH:
                    hitInfo = Physics.RaycastAll(transform.position, Vector3.back, 200f);
                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.gameObject.CompareTag("Room"))
                        {
                            foreach (WayBuilder Door in hit.collider.GetComponent<RoomInfo>().ChildDoorList)
                            {
                                if (hit.transform.GetComponent<RoomInfo>().IsBossRoom == false)
                                {
                                    if (this.transform.root.GetComponent<RoomInfo>().IsBossRoom != true)
                                    {
                                        //북쪽문하고 연결한다.
                                        if (Door.DD == DoorDirect.NORTH)
                                        {
                                            Door.SetPortalPos(this.transform);
                                            SetPortalPos(Door.transform);
                                            haveWay = true;
                                            hit.transform.GetComponent<RoomInfo>().IsWayNull = false;
                                            this.transform.root.GetComponent<RoomInfo>().IsWayNull = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (hit.transform.GetComponent<RoomInfo>().BossRoomhaveDoor == false)
                                    {
                                        //북쪽문하고 연결한다.
                                        if (Door.DD == DoorDirect.NORTH)
                                        {
                                            Door.SetPortalPos(this.transform);
                                            SetPortalPos(Door.transform);
                                            haveWay = true;
                                            hit.transform.GetComponent<RoomInfo>().IsWayNull = false;
                                            hit.transform.GetComponent<RoomInfo>().BossRoomhaveDoor = true;
                                            this.transform.root.GetComponent<RoomInfo>().IsWayNull = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;

                //이 문이 북쪽문일때
                case DoorDirect.NORTH:
                    hitInfo = Physics.RaycastAll(transform.position, Vector3.forward, 200f);
                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.gameObject.CompareTag("Room"))
                        {
                            foreach (WayBuilder Door in hit.collider.GetComponent<RoomInfo>().ChildDoorList)
                            {
                                if (hit.transform.GetComponent<RoomInfo>().IsBossRoom == false)
                                {
                                    if (this.transform.root.GetComponent<RoomInfo>().IsBossRoom != true)
                                    {
                                        //남쪽문하고 연결한다.
                                        if (Door.DD == DoorDirect.SOUTH)
                                        {
                                            Door.SetPortalPos(this.transform);
                                            SetPortalPos(Door.transform);
                                            haveWay = true;
                                            hit.transform.GetComponent<RoomInfo>().IsWayNull = false;
                                            this.transform.root.GetComponent<RoomInfo>().IsWayNull = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (hit.transform.GetComponent<RoomInfo>().BossRoomhaveDoor == false)
                                    {
                                        //남쪽문하고 연결한다.
                                        if (Door.DD == DoorDirect.SOUTH)
                                        {
                                            Door.SetPortalPos(this.transform);
                                            SetPortalPos(Door.transform);
                                            haveWay = true;
                                            hit.transform.GetComponent<RoomInfo>().IsWayNull = false;
                                            hit.transform.GetComponent<RoomInfo>().BossRoomhaveDoor = true;
                                            this.transform.root.GetComponent<RoomInfo>().IsWayNull = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }


    public void FindCanBuild()
    {
        RaycastHit[] hitInfo;

        if (haveWay != true)
        {
            switch (DD)
            {
                //이 문이 동쪽문일때
                case DoorDirect.EAST:
                    hitInfo = Physics.RaycastAll(transform.position, Vector3.right, 200f);

                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.gameObject.CompareTag("Room"))
                        {
                            if (hit.collider.GetComponent<RoomInfo>().haveRoom == false)
                            {
                                hit.collider.GetComponent<RoomInfo>().IsCanBuild = true;
                            }
                        }
                    }
                    break;
                //이 문이 서쪽문일때
                case DoorDirect.WEST:
                    hitInfo = Physics.RaycastAll(transform.position, Vector3.left, 200f);
                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.gameObject.CompareTag("Room"))
                        {
                            
                            if (hit.collider.GetComponent<RoomInfo>().haveRoom == false)
                            {
                                hit.collider.GetComponent<RoomInfo>().IsCanBuild = true;
                            }
                        }
                    }
                    break;

                //이 문이 남쪽문일때
                case DoorDirect.SOUTH:
                    hitInfo = Physics.RaycastAll(transform.position, Vector3.back, 200f);
                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.gameObject.CompareTag("Room"))
                        {
                            
                            if (hit.collider.GetComponent<RoomInfo>().haveRoom == false)
                            {
                                hit.collider.GetComponent<RoomInfo>().IsCanBuild = true;
                            }
                        }
                    }
                    break;

                //이 문이 북쪽문일때
                case DoorDirect.NORTH:
                    hitInfo = Physics.RaycastAll(transform.position, Vector3.forward, 200f);
                    foreach (RaycastHit hit in hitInfo)
                    {
                        if (hit.collider.gameObject.CompareTag("Room"))
                        {
                            if (hit.collider.GetComponent<RoomInfo>().haveRoom == false)
                            {
                                hit.collider.GetComponent<RoomInfo>().IsCanBuild = true;
                            }
                        }
                    }
                    break;
            }
        }
    }

    public int FindShopRoom()
    {
        RaycastHit[] hitInfo;
        switch (DD)
        {
            //이 문이 동쪽문일때
            case DoorDirect.EAST:
                hitInfo = Physics.RaycastAll(transform.position, Vector3.right, 200f);

                foreach (RaycastHit hit in hitInfo)
                {
                    if (hit.collider.gameObject.CompareTag("Room"))
                    {
                        if (hit.collider.GetComponent<RoomInfo>().IsShopRoom == true)
                        {
                            return hit.collider.GetComponent<RoomInfo>().RoomID;
                        }
                    }
                }
                break;
            //이 문이 서쪽문일때
            case DoorDirect.WEST:
                hitInfo = Physics.RaycastAll(transform.position, Vector3.left, 200f);
                foreach (RaycastHit hit in hitInfo)
                {
                    if (hit.collider.gameObject.CompareTag("Room"))
                    {
                        if (hit.collider.GetComponent<RoomInfo>().IsShopRoom == true)
                        {
                            return hit.collider.GetComponent<RoomInfo>().RoomID;
                        }
                    }
                }
                break;

            //이 문이 남쪽문일때
            case DoorDirect.SOUTH:
                hitInfo = Physics.RaycastAll(transform.position, Vector3.back, 200f);
                foreach (RaycastHit hit in hitInfo)
                {
                    if (hit.collider.gameObject.CompareTag("Room"))
                    {
                        if (hit.collider.GetComponent<RoomInfo>().IsShopRoom == true)
                        {
                            return hit.collider.GetComponent<RoomInfo>().RoomID;
                        }
                    }
                }
                break;

            //이 문이 북쪽문일때
            case DoorDirect.NORTH:
                hitInfo = Physics.RaycastAll(transform.position, Vector3.forward, 200f);
                foreach (RaycastHit hit in hitInfo)
                {
                    if (hit.collider.gameObject.CompareTag("Room"))
                    {
                        if (hit.collider.GetComponent<RoomInfo>().IsShopRoom == true)
                        {
                            return hit.collider.GetComponent<RoomInfo>().RoomID;
                        }
                    }
                }
                break;
        }
        return 99;
    }

    public void SetPortalPos(Transform DoorPos)
    {
        if (DoorPos != null)
        {
            Gotothis = DoorPos;
            haveWay = true;
        }
    }


    public void DestroyDoor(GameObject Wall,Transform Room)
    {
        if (haveWay == false)
        {
            if (transform.root.GetComponent<RoomInfo>().IsSpecialRoom !=true && transform.root.GetComponent<RoomInfo>().IsField != true)
            {
                GameObject CloseDoor = Instantiate(Wall, transform.position, Quaternion.identity) as GameObject;
                CloseDoor.transform.parent = Room;
            }
            Room.GetComponent<RoomInfo>().ChildDoorList.Remove(this);
            if (Room.GetComponent<RoomInfo>().IsShopRoom == false)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(this.transform.parent.gameObject);
            }
        }
    }

    public void CloseDoor()
    {
        //transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OpenDoor()
    {
        //transform.GetChild(1).gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(new Vector3(transform.position.x, 0, transform.position.z), _DetectRange);
        switch (DD)
        {
            case DoorDirect.EAST:
                Gizmos.DrawRay(transform.position, Vector3.right * 200f);
                break;
            case DoorDirect.WEST:
                Gizmos.DrawRay(transform.position, Vector3.left * 200f);
                break;
            case DoorDirect.SOUTH:
                Gizmos.DrawRay(transform.position, Vector3.back * 200f);
                break;
            case DoorDirect.NORTH:
                Gizmos.DrawRay(transform.position, Vector3.forward * 200f);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (DD)
            {
                case DoorDirect.EAST:
                    other.transform.position = new Vector3( Gotothis.position.x+6f,0,Gotothis.position.z);
                    break;
                case DoorDirect.WEST:
                    other.transform.position = new Vector3(Gotothis.position.x - 6f, 0, Gotothis.position.z);
                    break;
                case DoorDirect.SOUTH:
                    other.transform.position = new Vector3(Gotothis.position.x, 0, Gotothis.position.z-6f);
                    break;
                case DoorDirect.NORTH:
                    other.transform.position = new Vector3(Gotothis.position.x, 0, Gotothis.position.z+6f);
                    break;
            }
            Gotothis.transform.root.GetComponent<RoomInfo>().HaveBeenHere = true;
            Gotothis.transform.root.GetComponent<RoomInfo>().IsPlayerHere = true;
            this.transform.root.GetComponent<RoomInfo>().IsPlayerHere = false;
            MiniMapController.Instance.OnIconMiniMapShop(Gotothis.transform.root.GetComponent<RoomInfo>().SearchShopRoom());
            MiniMapController.Instance.ReloadMiniMap();
            //Gotothis.root.GetComponent<RoomInfo>().CloseChildrenDoor();
        }
    }
}
