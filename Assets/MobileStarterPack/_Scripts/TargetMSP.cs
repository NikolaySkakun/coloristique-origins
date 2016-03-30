using UnityEngine;
using System.Collections;

public class TargetMSP : MonoBehaviour {
	public int hit;
	
	void Update () {
		float Dist = Vector3.Distance(transform.position,TargetSocle.target.transform.position);
		if(Dist < 10 && hit <= 5){
			transform.localRotation =  Quaternion.Slerp (transform.localRotation, Quaternion.Euler(0, 0, 0),  0.3f);
		}else if (Dist >= 10){
		transform.localRotation =  Quaternion.Slerp (transform.localRotation, Quaternion.Euler(-90, 0, 0),  0.5f);
			hit =0;
		}else if (hit > 5 && Dist < 10){
			transform.localRotation =  Quaternion.Slerp (transform.localRotation, Quaternion.Euler(-90, 0, 0),  0.5f);
			
		}
		
	}
	
	public void Hit(int PowerofWeapon){
		//in exemple, we don't use the Power of Weapon
		hit	++;
	}
}
