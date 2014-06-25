using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class SteeringBehaviours : MonoBehaviour{

	public Vector3 SeekAll(string tag, float maxDistance){
		Vector3 dir = Vector3.zero;
		Vector3 one = new Vector3(1,1,1);
		Transform[] targets = GameObject.FindGameObjectsWithTag(tag).Select (go => go.transform)
			.Where(t => Vector3.SqrMagnitude(transform.position-t.position) < maxDistance * maxDistance)
			.OrderByDescending( t => {
				return Vector3.SqrMagnitude(transform.position-t.position);
		});

		foreach(Vector3 v in targets.position)
			dir+=one/(v-transform.position);
		return dir.normalized;
	}
	
	public Vector3 Seek(string tag, float maxDistance){
		Transform target = GameObject.FindGameObjectsWithTag(tag).Select (go => go.transform)
			.Where(t => Vector3.SqrMagnitude(transform.position-t.position) < maxDistance * maxDistance)
			.OrderByDescending( t => {
				return Vector3.SqrMagnitude(transform.position-t.position);
		}).LastOrDefault();

		return target ? (target.position-transform.position).normalized : Vector3.zero;
	}
	
	public Vector3 Seek(Transform target){
		return (target.position-transform.position).normalized;
	}
	
	public Vector3 Seek(Vector3 target){
		return (target-transform.position).normalized;
	}

	public Vector3 Wander(){
		return Seek(new Vector3(Random.Range (0.0f,5.0f),Random.Range (0.0f,5.0f),Random.Range (0.0f,5.0f)));
	}

	public Vector3 FleeAll(string tag, float maxDistance = 5.0f){
		Vector3 dir = Vector3.zero;
		Vector3 one = new Vector3(1,1,1);
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
		Transform[] targets = gos.Select( go => go.transform)
		.Where(t => Vector3.SqrMagnitude(transform.position-t.position) < maxDistance * maxDistance);

		foreach(Transform t in targets)
			dir+=one/(transform.position-t.position);
		return dir.normalized;
	}
	
	public Vector3 Flee(string tag, float maxDistance){
		Transform target = GameObject.FindGameObjectsWithTag(tag).Select (go => go.transform)
			.Where(t => Vector3.SqrMagnitude(transform.position-t.position) < maxDistance * maxDistance)
				.OrderByDescending( t => {
					return Vector3.SqrMagnitude(transform.position-t.position);
				}).LastOrDefault();
		
		return target ? (transform.position-target.position).normalized : Vector3.zero;
	}
	
	public Vector3 Flee(Transform target){
		return (transform.position-target.position).normalized;
	}

	public Vector3 Flee(Vector3 target){
		return (transform.position-target).normalized;
	}

	public Vector3 Avoid(float maxDistance){
		Ray ray = new Ray(transform.position,transform.forward*rigidbody.velocity.magnitude);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, maxDistance))
			return (Vector3.Distance(hit.point,hit.transform.position)*(hit.point-hit.transform.position)).normalized;

		return Vector3.zero;
	}

	public Vector3 Avoid(int layerMask){
		Ray ray = new Ray(transform.position,transform.forward*rigidbody.velocity.magnitude);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity,1 << layerMask))
			return (Vector3.Distance(hit.point,hit.transform.position)*(hit.point-hit.transform.position)).normalized;
		
		return Vector3.zero;
	}

	public Vector3 Avoid(int layerMask, float maxDistance){
		Ray ray = new Ray(transform.position,transform.forward*rigidbody.velocity.magnitude);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, maxDistance,1 << layerMask))
			return (Vector3.Distance(hit.point,hit.transform.position)*(hit.point-hit.transform.position)).normalized;

		return Vector3.zero;
	}

	public Vector3 Arrive(string tag, float maxDistance){
		Vector3 v;
		Transform[] targets = GameObject.FindGameObjectsWithTag(tag)
			.Select( go => go.transform)
			.Where( t => Vector3.Distance(transform.position,t.position) < maxDistance/2);

		foreach(Transform t in targets)
			v+= -rigidbody.velocity*(Vector3.Distance(transform.position,t.position)/maxDistance);
		return v.normalized;
	}
	
	public Vector3 Arrive(Transform target){
		return (-rigidbody.velocity*(Vector3.Distance(transform.position,target.position)/maxDistance)).normalized;
	}

	public Vector3 Arrive(Vector3 target){
		return (-rigidbody.velocity*(Vector3.Distance(transform.position,target)/maxDistance)).normalized;
	}

	public Vector3 Pursuit(Transform target){
		float T = Vector3.Distance(transform.position,target.position)/target.rigidbody.velocity.magnitude;

		return transform.position-(target.position+(target.position+target.rigidbody.velocity*T));
	}

	public Vector3 Pursuit(string tag, float maxDistance){
		Vector3 dir = Vector3.zero;
		float T = Vector3.Distance(transform.position,target.position)/target.rigidbody.velocity.magnitude;
		Transform[] targets = GameObject.FindGameObjectsWithTag(tag)
			.Select( go => go.transform)
				.Where( t => Vector3.SqrMagnitude(transform.position-t.position) < maxDistance * maxDistance);
		
		foreach(Transform t in targets)
			dir+=transform.position-(t.position+(t.position+t.rigidbody.velocity*T));
		return dir;
	}

	public Vector3 Evade(Transform target){
		float T = Vector3.Distance(transform.position,target.position)/target.rigidbody.velocity.magnitude;

		return target.position+(target.position+target.rigidbody.velocity*T)-transform.position;
	}

	public Vector3 Evade(string tag, float maxDistance){
		Vector3 dir = Vector3.zero;
		float T = Vector3.Distance(transform.position,target.position)/target.rigidbody.velocity.magnitude;
		Transform[] targets = GameObject.FindGameObjectsWithTag(tag)
			.Select( go => go.transform)
				.Where( t => Vector3.SqrMagnitude(transform.position-t.position) < maxDistance * maxDistance);
		
		foreach(Transform t in targets)
			dir+=t.position+(t.position+target.rigidbody.velocity*T)-transform.position;
		return dir;
	}

	public Vector3 Alignment(string tag, float maxDistance){
		Vector3 v = Vector3.zero;
		Transform[] neighbours = GameObject.FindGameObjectsWithTag(tag).Select (go => go.transform)
			.Where(t => Vector3.SqrMagnitude(transform.position-t.position) < maxDistance * maxDistance);

		foreach(Transform t in neighbours)
			v+= t.rigidbody.velocity;
		return (v/neighbours.Length).normalized;
	}

	public Vector3 Cohesion(string tag, float maxDistance){
		Vector3 v = Vector3.zero;
		Transform[] neighbours = GameObject.FindGameObjectsWithTag(tag).Select (go => go.transform)
			.Where(t => Vector3.SqrMagnitude(transform.position-t.position) < maxDistance * maxDistance);

		foreach(Transform t in neighbours)
			v+=t.position;
		return (v/neighbours.Length).normalized;
	}

	public Vector3 Seperation(string tag, float maxDistance){
		Vector3 v = Vector3.zero;
		Transform[] neighbours = GameObject.FindGameObjectsWithTag(tag).Select (go => go.transform)
			.Where(t => Vector3.SqrMagnitude(transform.position-t.position) < maxDistance * maxDistance);
		
		foreach(Transform t in neighbours)
			v+=transform.position-t.position;
		v/=neighbours.Length;
		return (-v).normalized;
	}

	public Vector3 Flock(string tag, float maxDistance){
		Vector3 a = Alignment(tag,maxDistance);
		Vector3 c = Cohesion(tag,maxDistance);
		Vector3 s = Seperation(tag,maxDistance);
		
		return (a+c+s).normalized;
	}

	public Vector3 Flock(string tag, float maxDistance, params float[] weights){
		Vector3 a = Alignment(tag,maxDistance);
		Vector3 c = Cohesion(tag,maxDistance);
		Vector3 s = Seperation(tag,maxDistance);

		a*= weights[0] ? weights[0] : 1.0f;
		c*= weights[1] ? weights[1] : 1.0f;
		s*= weights[2] ? weights[2] : 1.0f;

		return (a+c+s).normalized;
	}
}

