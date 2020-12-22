using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Player,
    Enemy,
    Boss
}


public enum UnitAttacktype //유닛 공격 타입, 움직이면서 공격, 멈춘후 공격.
{    
    Dynamic,
    Static

}

public enum UnitAttackRangeType //원거리, 근거리
{
    Melee,
    Range
}


//이 스크립터블 오브젝트는 유니티 에디터 상에서 생성 가능합니다.
//경로는 "New Object/DataBase System/Object/Status"
[CreateAssetMenu(fileName = "New Object", menuName = "DataBase System/Object/Status")]
public class ObjectInfo : ScriptableObject
{
    public UnitType type;
    public UnitAttacktype Attacktype;
    public UnitAttackRangeType AttackRangeType;
    [TextArea(15, 20)]
    public string Description;
    public Unit unit;
}


[System.Serializable]
public class Unit
{
    public string Name;
    public int Id;
    public UnitStatus Stat;

}
[System.Serializable]
public class UnitStatus
{
    public float HP;
    public float MaxHP;
    public float Shield;
    public float Power;
    public float MoveSpeed;
    public float AttackSpeed;
    public int Gold;
    public float Range;
}
