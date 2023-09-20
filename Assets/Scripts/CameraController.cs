using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private float snapTime;
    private Vector3 currentSpeed = new Vector3();

    private void Awake()
    {
        offset = this.transform.position - target.position;
    }
    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentSpeed, snapTime);
    }
}
