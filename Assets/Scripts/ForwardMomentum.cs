using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMomentum : MonoBehaviour {

	public float momentum = 10;
	private Rigidbody rigid;

	void Start () {
		rigid = GetComponent<Rigidbody>();	
	}

	void OnCollisionStay(Collision collisionInfo) {
        foreach (ContactPoint contact in collisionInfo.contacts) {
			if(contact.otherCollider.tag == "Platform") {
				rigid.AddForce(0, 0, momentum);
			}
        }
    }
}
