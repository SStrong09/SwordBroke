using UnityEngine;



//공통된 사항을 넣을 클래스

//이 스크립터블 오브젝트는 유니티 에디터 상에서 생성 가능합니다.
//경로는 "/DataBase System/Sound/Track"


[System.Serializable]
public class Track
{
    public AudioClip clip;
    [Range(0f,1f)]
    public float Volume=1f;
}
[CreateAssetMenu(fileName = "New AudioClip", menuName = "DataBase System/Sound/Track")]
public class SoundBundle : ScriptableObject
{
    [Header("사운드 등록")]
    [SerializeField] 
    public Track[] Sounds;
}
