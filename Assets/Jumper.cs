using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{

	private enum State {Start, S1}
	private State _state;
	public GameObject _stair;
	public GameObject _floor;
	public List<GameObject> _validTargets = new List<GameObject>();
	public int _currentTargetID = 0;
	public GameObject _currentTarget;

	public float jumpUpVelocity;

	// Start is called before the first frame update
	void Start() {
		_state = State.Start;

		for (int i = 0; i < _stair.transform.childCount; i++){
			_validTargets.Add(_stair.transform.GetChild(i).gameObject);
		}
	}

	// Update is called once per frame
	void Update()
	{
		// StateAction();
		if (Mathf.Approximately(GetComponent<Rigidbody>().velocity.y, 0.0f)){
			StateAction();
		}
	}

	void StateAction(){
		_currentTargetID++;
		if (_currentTargetID >= _validTargets.Count) _currentTargetID = 0;

		jump_to(_validTargets[_currentTargetID].transform.position);
	}

	void jump_to(Vector3 point){
		Vector3 direction = point - transform.position;
		float time = timeToLand(point);
		float vX = direction.x / time;
		float vZ = direction.z / time;

		GetComponent<Rigidbody>().velocity = new Vector3(vX, jumpUpVelocity, vZ);
	}

	float timeToLand(Vector3 point){
		float g = Physics.gravity.y;
		float sqrt = Mathf.Sqrt(Mathf.Abs(jumpUpVelocity * jumpUpVelocity - 2 * g * (transform.position - point).y));
		return Mathf.Min(-jumpUpVelocity + sqrt, -jumpUpVelocity - sqrt)/g;
	}
}
