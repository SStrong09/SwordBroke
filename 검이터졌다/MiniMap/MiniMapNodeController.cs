using System.Collections.Generic;
using UnityEngine;

public enum IconType
{
    Blank,
    Player,
    Boss,
    Shop,
    Sword
}

public class MiniMapNodeController : MonoBehaviour
{
    [SerializeField] private List<GameObject> Icons;

    public int RoomID;
    
    private void Awake()
    {
        InitIcon();
    }
    public void InitIcon()
    {
        Icons = new List<GameObject>();
        Icons.Add(transform.GetChild((int)IconType.Blank).gameObject);
        Icons.Add(transform.GetChild((int)IconType.Player).gameObject);
        Icons.Add(transform.GetChild((int)IconType.Boss).gameObject);
        Icons.Add(transform.GetChild((int)IconType.Shop).gameObject);
        Icons.Add(transform.GetChild((int)IconType.Sword).gameObject);
        Icons[(int)IconType.Blank].SetActive(true);
        Icons[(int)IconType.Player].SetActive(false);
        Icons[(int)IconType.Boss].SetActive(false);
        Icons[(int)IconType.Shop].SetActive(false);
        Icons[(int)IconType.Sword].SetActive(false);
    }

    public void OnOffIcon(IconType icon, bool OnOff)
    {
        switch (icon)
        {
            case IconType.Blank:
                Icons[(int)IconType.Blank].SetActive(OnOff);
                break;
            case IconType.Player:
                Icons[(int)IconType.Player].SetActive(OnOff);
                break;
            case IconType.Boss:
                Icons[(int)IconType.Boss].SetActive(OnOff);
                break;
            case IconType.Shop:
                Icons[(int)IconType.Shop].SetActive(OnOff);
                break;
            case IconType.Sword:
                Icons[(int)IconType.Sword].SetActive(OnOff);
                break;
        }
    }
}
