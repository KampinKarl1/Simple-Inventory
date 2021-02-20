using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private float rotateSpeed = 3.0f;
    Quaternion prevRot = Quaternion.identity;

    void Update()
    {
        transform.position = target.position;

        transform.rotation = Quaternion.Slerp(prevRot, target.rotation, Time.deltaTime * rotateSpeed);

        prevRot = transform.rotation;
    }
}
