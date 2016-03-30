using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 6F;
	public float sensitivityY = 6F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -80f;//-60F;
	public float maximumY = 70f;//60F;

	public float rotationY = 5F;

	void OnEnable()
	{
		if(Level.current != null && Level.current.Index != 0)
			rotationY = transform.localEulerAngles.x > 0 ? 0 : -transform.localEulerAngles.x;
	}

	void Update ()
	{
		if(Input.touchCount > 0)
			this.enabled = false;

		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + (Input.GetAxis("Mouse X") + Input.GetAxis("Joy X")) * sensitivityX;
			
			rotationY += (Input.GetAxis("Mouse Y") + Input.GetAxis("Joy Y")) * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, (Input.GetAxis("Mouse X") + Input.GetAxis("Joy X")) * sensitivityX, 0);
		}
		else
		{
			rotationY += (Input.GetAxis("Mouse Y") + Input.GetAxis("Joy Y")) * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
//	public Mesh mesh;
//	public Material mat;
//	public void OnPostRender() {
//		// set first shader pass of the material
//		mat.SetPass(0);
//		// draw mesh at the origin
//		Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
//	}
	
	void Start ()
	{
//		mat = Game.BaseMaterial;
//		mat.color = Color.black;
//		mesh = CustomMesh.Circle ();
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}