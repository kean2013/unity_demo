using System.Collections;
using UnityEngine;

namespace HA
{
	public class Group
    {
		public LayerMask mask;		// 成员所在层级
		public float keepDistance;  // 成员之间距离
		public float keepWeight;    // 成员之间距离权重

		public float targetCloseDistance = 20; // 距离目标距离
		public float targetWeight = 1.25f;  // 距离目标权重
		public float moveWeight = 0.8f;   // 成员移动权重

		public Vector3 targetPosition;	// 移动目标
		public Transform attackTarget;	// 攻击目标
	}

    public class FollowingBehaviour 
    {
		public Group group;
		public Transform transform;

		public Vector3 movement = Vector3.zero;
		private float tgtSpeed = 0, speed = 0, currentSpeed;
		public float moveSpeed = 5, rotateSpeed = 20;//移动旋转速度

		public Vector3 position => transform.position;
		private const float minMoveCheck = 0.2f;
       
		public FollowingBehaviour(Group g, Transform trans)
        {
			group = g;
			transform = trans;
        }

        void Update()
		{
			Vector3 displacement = group.targetPosition - position;//获取目标距离
			Vector3 direction = displacement.normalized * group.targetWeight;//方向*权重

			if (displacement.magnitude < group.targetCloseDistance)//重新计算目的地距离权重
				direction *= displacement.magnitude / group.targetCloseDistance;

			direction += GetGroupMovement();//获取周围组的移动

			if ((group.targetPosition - position).magnitude < minMoveCheck)//计算移动速度
				tgtSpeed = 0;
			else
				tgtSpeed = moveSpeed;

			speed = Mathf.Lerp(speed, tgtSpeed, 2 * Time.deltaTime);

			Drive(direction, speed);//移动
		}

		private Vector3 GetGroupMovement()
		{
			Collider[] c = Physics.OverlapSphere(position, group.keepDistance, group.mask);//获取周围成员
			Vector3 dis, v1 = Vector3.zero, v2 = Vector3.zero;
			for (int i = 0; i < c.Length; i++)
			{
				FollowingBehaviour others = c[i].GetComponent<FollowingBehaviour>();
				dis = position - others.position;//距离
				v1 += dis.normalized * (1 - dis.magnitude / group.keepDistance);//查看与周围单位的距离
				v2 += others.movement;//查看周围单位移动方向

				Debug.DrawLine(position, others.position, Color.yellow);
			}

			return v1.normalized * group.keepWeight + v2.normalized * group.moveWeight;//添加权重因素
		}

		private void Drive(Vector3 direction, float spd)
		{
			Vector3 finialDirection = direction.normalized;
			float finialSpeed = spd, finialRotate = 0;
			float rotateDir = Vector3.Dot(finialDirection, transform.right);
			float forwardDir = Vector3.Dot(finialDirection, transform.forward);

			if (forwardDir < 0)
				rotateDir = Mathf.Sign(rotateDir);

			if (forwardDir < -0.2f)
				finialSpeed = Mathf.Lerp(currentSpeed, -spd * 8, 4 * Time.deltaTime);

			if (forwardDir < 0.98f)//防抖
				finialRotate = Mathf.Clamp(rotateDir * 180, -rotateSpeed, rotateSpeed);

			finialSpeed *= Mathf.Clamp01(direction.magnitude);
			finialSpeed *= Mathf.Clamp01(1 - Mathf.Abs(rotateDir) * 0.8f);

			transform.Translate(Vector3.forward * finialSpeed * Time.deltaTime);
			transform.Rotate(Vector3.up * finialRotate * Time.deltaTime);

			currentSpeed = finialSpeed;
			movement = direction * finialSpeed;
		}
	}
}
