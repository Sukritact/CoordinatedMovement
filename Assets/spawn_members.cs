using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_members : MonoBehaviour
{

	public GameObject myPrefab;
	public Formation_Member parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp("space")){
			GameObject new_object = Instantiate(myPrefab, transform.position, Quaternion.identity);
			Formation_Member new_member = new_object.GetComponent<Formation_Member>();
			new_member.parent = parent;
		}
	}
}
