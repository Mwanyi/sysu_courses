using UnityEngine;
using System.Collections;

public class parebolaway1 : MonoBehaviour
{
	// ����Ŀ��
	public GameObject target;
	// �趨�ٶ�
	public float speed = 10;
	// ����
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
			// ʹ��׼����������׼target����ʹ�����z������target���Ե�λ��
			this.transform.LookAt(targetPosition);
			// �Ƕ�
			float angle = Mathf.Min(1, Vector3.Distance(this.transform.position, targetPosition) / distanceToTarget) * 45;
			// ʹ����Ԫ������ת
			this.transform.rotation = this.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
			float currentDist = Vector3.Distance(this.transform.position, target.transform.position);
			print("currentDist" + currentDist);
			// ��������target�㹻С��ʱ��ֹͣ�ƶ�
			if (currentDist < 0.5f)
				move = false;
			// �����ƶ�����һ������Ϊ�ٶȣ�Ϊʸ��������0��0��1���˾���
			// ע�����ﵱʣ�����С��speed*��λ����ʱ��ֻ���ƶ�ʣ����룬���Ҫ��һ����Сֵ��ȡֵ
			this.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
			yield return null;
		}
	}
}