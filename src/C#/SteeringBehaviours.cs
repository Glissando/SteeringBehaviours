using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class SteeringBehaviours : MonoBehaviour{

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
		return Seek(new Vector3(Random.Range (0.0f,1.0f),Random.Range (0.0f,1.0f),Random.Range (0.0f,1.0f)));
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

	public Vector3 Avoid(string tag, float maxDistance){
		Vector3 dir = Vector3.zero;
		var objects = GameObject.FindGameObjectsWithTag(tag)
		.Select(go => go.transform)
		.Where(t => Vector3.SqrMagnitude(transform.position,t.position) < (maxDistance*maxDistance));
		
		foreach(Transform t in objects)
			dir += Flee(t);
		return dir.normalized;
	}
	
	public Vector3 Arrive(string tag, float maxDistance){
		Vector3 dir = Vector3.zero;
		Transform[] targets = GameObject.FindGameObjectsWithTag(tag)
			.Select( go => go.transform)
			.Where( t => Vector3.SqrMagnitude(transform.position,t.position) < (maxDistance*maxDistance)/2);

		foreach(Transform t in targets){
			float dRatio = maxDistance/Vector3.Distance(transform.position,t.position);
			dir += (dRatio > 1) ? Seek(t)/dRatio : Seek(t);
		}
		return dir.normalized;
	}
	
	public Vector3 Arrive(Transform target, float maxDistance){
		dRatio = maxDistance/Vector3.Distance(transform.position,target.position);
		return (dRatio > 1) ? Seek(target)/dRatio : Seek(target);
	}

	public Vector3 Arrive(Vector3 target,float maxDistance){
		dRatio = maxDistance/Vector3.Distance(transform.position,target);
		return (dRatio > 1) ? Seek(target)/dRatio : Seek(target);
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
		return Seek(v/neighbours.Length);
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

