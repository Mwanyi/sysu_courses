using UnityEngine;
using System.Collections;

public class parebolaway1 : MonoBehaviour
{
	// 击中目标
	public GameObject target;
	// 设定速度
	public float speed = 10;
	// 距离
	private float distanceToTarget;
	private bool move = true;

	void Start()
	{
		distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
		StartCoroutine(Shoot());
	}

	IEnumerator Shoot()
	{
		while (move)
		{
			Vector3 targetPosition = target.transform.position;
			// 使瞄准方的物体瞄准target，即使物体的z轴正对target所对的位置
			this.transform.LookAt(targetPosition);
			// 角度
			float angle = Mathf.Min(1, Vector3.Distance(this.transform.position, targetPosition) / distanceToTarget) * 45;
			// 使用四元进行旋转
			this.transform.rotation = this.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
			float currentDist = Vector3.Distance(this.transform.position, target.transform.position);
			print("currentDist" + currentDist);
			// 当物体与target足够小的时候停止移动
			if (currentDist < 0.5f)
				move = false;
			// 物体移动，第一个参数为速度，为矢量，即（0，0，1）乘距离
			// 注意这里当剩余距离小于speed*单位长度时，只能移动剩余距离，因此要有一个最小值的取值
			this.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
			yield return null;
		}
	}
}