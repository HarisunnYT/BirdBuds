using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private float followSpeed;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z), followSpeed * Time.deltaTime);
    }
}
