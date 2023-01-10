using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform gunPivot;

    public ParticleSystem gunFireEffect;
    private LineRenderer bulletLine;
    private AudioSource gunAudio;

    private float lastFireTime;

    private void Awake()
    {
        gunAudio = GetComponent<AudioSource>();
        bulletLine = GetComponent<LineRenderer>();

        bulletLine.enabled = false;

        lastFireTime = 0f;
    }
    public void Fire()
    {
        if (Time.time - lastFireTime > gunData.shotDelay)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }
    private void Shot()
    {
        
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        var ray = new Ray(gunPivot.position, gunPivot.forward);

        if (Physics.Raycast(ray, out hit, gunData.distance))
        {
            var target = hit.collider.GetComponent<IAliveObject>();
            hitPos = hit.point;
            if (target != null)
            {
                target.OnDamage(gunData.damage, hitPos, hit.normal);
            }
        }
        else
        {
            hitPos = gunPivot.position + gunPivot.forward * gunData.distance;
        }
            StartCoroutine(ShotEffect(hitPos));
    }

    private IEnumerator ShotEffect(Vector3 hitPos)
    {

        bulletLine.enabled = true;
        gunFireEffect.Play();

        bulletLine.SetPosition(0, gunPivot.transform.position);
        bulletLine.SetPosition(1, hitPos);

        gunAudio.PlayOneShot(gunData.shotClip);

        yield return new WaitForSeconds(0.03f);

        bulletLine.enabled = false;
    }
}
