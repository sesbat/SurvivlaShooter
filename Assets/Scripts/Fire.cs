using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Zombie"))
        {
            RaycastHit hit;
            Vector3 hitPos = Vector3.zero;
            var ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, other.transform.position)))
            {
                var target = hit.collider.GetComponent<IAliveObject>();
                hitPos = hit.point;
                if (target != null)
                {
                    other.GetComponent<AliveObjectHealth>().OnDamage(10, hitPos, hit.normal);
                }
            }
        }
    }

}
