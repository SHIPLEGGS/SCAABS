using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionRotationScript : MonoBehaviour
{
    Quaternion rot;

    private void Awake()
    {
        rot = this.transform.rotation;
    }

    private void LateUpdate()
    {
        this.transform.rotation = rot;
    }
}
