using NodeCanvas.Tasks.Conditions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class AnimateOnPlayerEntered : NetworkBehaviour
{
    [SerializeField]
    private bool requiresGrounded = false;

    private Animator animator;

    private bool triggeredEnterAnimation = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.IsPlayer(collision.gameObject))
        {
            if (requiresGrounded && Mathf.CeilToInt(GameManager.Instance.GetPlayerFromObject(collision.gameObject).Rigidbody.velocity.y) == 0)
            {
                PlayerInteracted(true);
                triggeredEnterAnimation = true;
            }
            else if (!requiresGrounded)
            {
                PlayerInteracted(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (GameManager.Instance.IsPlayer(collision.gameObject))
        {
            if (!requiresGrounded || triggeredEnterAnimation)
            {
                triggeredEnterAnimation = false;
                PlayerInteracted(false);
            }
        }
    }

    private void PlayerInteracted(bool entered)
    {
        if (isServer)
            RpcPlayerInteracted(entered);
        else
            CmdPlayerInteracted(entered);
    }

    [Command(ignoreAuthority = true)]
    public void CmdPlayerInteracted(bool entered)
    {
        RpcPlayerInteracted(entered);
    }

    [ClientRpc]
    public void RpcPlayerInteracted(bool entered)
    {
        animator.SetTrigger(entered ? "PlayerEntered" : "PlayerExited");
    }

}
