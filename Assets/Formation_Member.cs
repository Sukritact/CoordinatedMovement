using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation_Member : Steering
{
	//---------------------------------------------------------

	public float					min_radius		= 4.0f;
	public Formation_Member			parent;

	private int						num_members		= 0;
	private float 					member_spacing	= 1.5f;
	private List<Formation_Member>	members			= new List<Formation_Member>();
	private List<Vector3>			memberLocations = new List<Vector3>();

	//---------------------------------------------------------

	void Start()
	{
		target_point = transform.position;
		velocity = Vector3.zero;

		if (parent){
			parent.RegisterChild(this);
		}

		UpdateMemberLocation();
	}

	// Update is called once per frame
	void Update()
	{

		if (!parent){
			// Set Target Point to mouseclick
			if (Input.GetMouseButton(0)){
				RaycastHit hit;

				if (Physics.Raycast(
					Camera.main.ScreenPointToRay(Input.mousePosition),
					out hit,
					100
				)){
					target_point = hit.point;
				}
			}
		}

		Vector3 steer_force = Seek(target_point);
		Vector3 avoid_force = Avoid();

		velocity += steer_force;
		velocity += avoid_force;
		velocity.y = 0;

		Vector3.ClampMagnitude(velocity, max_speed);
		transform.position += velocity;

		if (velocity.magnitude > 0.01){
			transform.rotation = Quaternion.LookRotation(velocity);
		}

		PingChildren();
	}

	// void OnDrawGizmos()
	// {
	// 	// Draw a yellow sphere at the transform's position
	// 	Gizmos.color = Color.yellow;
	// 	for (int i = 0; i < memberLocations.Count; i++){
	// 		Gizmos.DrawSphere(transform.position + memberLocations[i], 1);
	// 	}
	// }

	//---------------------------------------------------------

	public void RegisterChild(Formation_Member new_child){
		members.Add(new_child);
		num_members = members.Count;
		UpdateMemberLocation();

		print(num_members);
	}

	void PingChildren(){
		if (num_members < 1) return;

		Vector3 location = transform.position;
		for (int i = 0; i < num_members; i++){
			Vector3				offset	= memberLocations[i];
			Formation_Member	member 	= members[i];

			member.target_point = location + offset;
		}
	}

	void UpdateMemberLocation(){

		if (num_members < 1) return;

		float circumference = member_spacing * num_members;
		float radius = circumference/ (2 * Mathf.PI);
			radius = Mathf.Max(radius, min_radius);
		float angle_increment = 360.0f/num_members;

		memberLocations.Clear();
		for (int i = 0; i < num_members; i++){
			Vector3 offset = transform.forward * radius;
			offset = Quaternion.AngleAxis(angle_increment * i, Vector3.up) * offset;
			memberLocations.Add(offset);
		}
	}
}
