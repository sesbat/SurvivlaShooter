using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Vector3 = UnityEngine.Vector3;

internal interface IAliveObject
{
    bool OnDamage(int dmg, Vector3 hitPoint, Vector3 hitNormal); //데미지 받을때 호출
}

