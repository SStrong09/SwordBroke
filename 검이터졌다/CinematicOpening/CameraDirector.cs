using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CameraDirector : MonoBehaviour
{

    public PlayableDirector PD;
    public TimelineAsset[] TLA;
    // Start is called before the first frame update
    void Start()
    {
        //int ChoiceRandomScene = Random.Range(0,7);
        int ChoiceRandomScene = Random.Range(0, TLA.Length);
        //if(ChoiceRandomScene < 3)
        //{
        //    PD.playableAsset = TLA[0];
        //}
        //else
        //{
        //    PD.playableAsset = TLA[1];
        //}
           PD.playableAsset = TLA[ChoiceRandomScene];
        PD.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
