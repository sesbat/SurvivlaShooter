using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AliveObjectHealth : MonoBehaviour, IAliveObject
{
    protected float lastAttackTime;
    protected float timeBetAttack = 0.5f;
    protected bool isHit { 
        get {
            if (!isDead && Time.time - lastAttackTime > timeBetAttack)
            {
                lastAttackTime = Time.time;
                return true;
            }
            else 
                return false; }
    }

    protected bool isDead;

    protected int hp;
    protected int maxHp;
    protected int speed;
    protected int damage;

    public abstract void DieAniamtion(); //필수로 구현해야하는 가상함수

    public virtual bool OnDamage(int dmg, Vector3 hitPoint, Vector3 hitNormal)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            return true;
        }
        return false;
    }
}

