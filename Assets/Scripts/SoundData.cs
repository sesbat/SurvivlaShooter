using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/SoundData", fileName = "SoundData")]
public class SoundData : ScriptableObject
{
    #region 좀비 데이터
    public float bgmScale = 1;
    public float effectScale = 1;
    public bool isMute = false;
    #endregion

}
