using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/ZombieData", fileName = "ZombieData")]
public class ZombieData : ScriptableObject
{
    #region 좀비 데이터
    public AudioClip hitSound; // 발사 소리
    public int hp = 100;
    public int maxHp = 100;
    public int damage = 10; // 공격력
    public int speed = 1;
    public float findDis = 20;
    public int spawnDelay = 4;
    #endregion

}
