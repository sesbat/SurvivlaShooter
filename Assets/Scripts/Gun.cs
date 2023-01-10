using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform gunPivot;

    public ParticleSystem gunFireEffect;
    [SerializeField]
    public GameObject gunLight;
    private LineRenderer bulletLine;
    private EffectSound gunAudio;

    private float lastFireTime;

    private void Awake()
    {
        gunAudio = GetComponent<EffectSound>();
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
        gunLight.SetActive(true);

        bulletLine.SetPosition(0, gunPivot.transform.position);
        bulletLine.SetPosition(1, hitPos);

        gunAudio.PlayOneShot(gunData.shotClip);

        yield return new WaitForSeconds(0.03f);

        bulletLine.enabled = false;
        gunLight.SetActive(false);
    }

}
