using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/GunData", fileName = "GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public int damage = 25;
    public float distance = 10f;
    public float shotDelay = 0.1f;
}
