using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSet : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;

        var brain = Camera.main.GetComponent<CinemachineBrain>();
        var vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        vcam.Follow = transform;
        vcam.LookAt = transform;
    }
}
