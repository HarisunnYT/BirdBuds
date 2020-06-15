using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayers : MonoBehaviour
{
    [SerializeField]
    private float followSpeed;

    private void FixedUpdate()
    {
        if (GameManager.Instance.Players.Count == 1)
        {
            Transform target = GameManager.Instance.Players[0].transform;
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), followSpeed * Time.deltaTime);
        }
        else if (GameManager.Instance.Players.Count == 2)
        {
            Transform p1 = GameManager.Instance.Players[0].transform;
            Transform p2 = GameManager.Instance.Players[1].transform;
            Vector3 target = p1.position + (p2.position - p1.position) / 2;
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, target.y, transform.position.z), followSpeed * Time.deltaTime);
        }
    }
}
