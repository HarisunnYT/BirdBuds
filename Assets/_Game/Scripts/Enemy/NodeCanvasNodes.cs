using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Category("Custom")]
    [Description("Gets a lists of game objects that are in the physics overlap sphere at the position of the agent, excluding the agent")]
    public class GetClosestPlayer : ConditionTask<Transform>
    {

        public LayerMask layerMask = -1;
        public BBParameter<float> radius = 2;
        [BlackboardOnly]
        public BBParameter<GameObject> saveObjectsAs;

        protected override bool OnCheck()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(agent.transform.position, radius.value, layerMask.value);
            if (cols.Length > 0)
            {
                Collider2D closest = cols[0];
                foreach (var col in cols)
                {
                    if (col != closest && Vector3.Distance(agent.transform.position, col.transform.position) < Vector3.Distance(agent.transform.position, closest.transform.position))
                    {
                        closest = col;
                    }
                }

                saveObjectsAs.value = closest.gameObject;
                return true;

            }
            else
            {
                saveObjectsAs.value = null;
                return false;
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (agent != null)
            {
                Gizmos.color = new Color(1, 1, 1, 0.2f);
                Gizmos.DrawSphere(agent.position, radius.value);
            }
        }
    }
}