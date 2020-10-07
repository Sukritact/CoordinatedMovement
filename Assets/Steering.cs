using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{

	//---------------------------------------------------------

	public Vector3 target_point;

	public float mass			= 50.0f;
	public float max_speed		= 20.0f;

	public float arrive_dist	= 5.0f;

	public float avoid_force	= 10.0f;
	public float avoid_dist		= 10.0f;

	protected Vector3 velocity;

	//---------------------------------------------------------

	public Vector3 Seek(Vector3 target, bool flee = false, bool wander = false){
		Vector3 target_velocity = target - transform.position;
		float distance = target_velocity.magnitude;

		if (distance < arrive_dist){
			target_velocity.Normalize();
			target_velocity *= max_speed * Time.deltaTime;
		} else {
			target_velocity.Normalize();
			target_velocity *= max_speed * Time.deltaTime;
			target_velocity *= distance/arrive_dist;
		}

		if (flee) target_velocity *= -1;
		if (wander) target_velocity /= 2;

		Vector3 steer_force = target_velocity - velocity;
		steer_force = Vector3.ClampMagnitude(steer_force, max_speed);
		steer_force = steer_force/mass;

		return steer_force;
	}

	public Vector3 Avoid(){
		RaycastHit hit;
		int layer_mask = 1 << 8;

		if (Physics.Raycast(
			transform.position,
			transform.forward,
			out hit,
			avoid_dist,
			layer_mask
		)){
			Vector3 avoid_force = Seek(hit.point, true);
			float turn_force = Vector3.Dot(transform.forward, hit.normal);
			if (turn_force < 1){
				Vector3 rotated_avoid_force = Quaternion.AngleAxis(90, Vector3.up) * avoid_force;
				float lerp = (turn_force + 1)/2;
				avoid_force = Vector3.Slerp(rotated_avoid_force, avoid_force, lerp);
			}

			return avoid_force;
		}

		return Vector3.zero;
	}
}
