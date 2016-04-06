using UnityEngine;
using System.Collections;

public class VR_canvas : MonoBehaviour {

	public enum BindType:int {FixedOrientation=0,FixedInCenter=1,AlwaysInViewField=2};
	public BindType bindType = BindType.AlwaysInViewField;
	public float scale = 1f;
	private GameObject parentGO;

	private VRCamera vrCameraGO;
	private Camera[] uiCameras;
	private float fieldViewAngle=30f;

	void SetLayerRecursively(GameObject go, int layerNumber)
	{
		go.layer = layerNumber;
		foreach (Transform trans in go.transform)
		{
			SetLayerRecursively(trans.gameObject,layerNumber);
		}
	}

	void OnEnable()
	{
		Init ();
	}

	bool initialized=false;
	void Init()
	{
		if( initialized ) return;

		parentGO = new GameObject();
		gameObject.transform.SetParent(parentGO.transform);
		gameObject.transform.localRotation = Quaternion.identity;
		vrCameraGO = GameObject.FindObjectOfType<VRCamera>();
		if( vrCameraGO!=null )
		{
			Canvas cv = gameObject.GetComponentInChildren<Canvas>();
			if( cv!=null )
			{
				SetLayerRecursively(cv.gameObject,LayerMask.NameToLayer("TransparentFX"));
				cv.renderMode = RenderMode.WorldSpace;
				Camera UI_dummyCamera = vrCameraGO.transform.Find("VRCamera/VR_UI_dummyCamera").GetComponent<Camera>();
				cv.worldCamera = UI_dummyCamera;
				uiCameras = new Camera[vrCameraGO.cameras.Length];
				float eyeDist = 0.07f;
				float eyeFieldOfView = 100f;
				for(int k=0; k<uiCameras.Length; k++ )
				{
					uiCameras[k] = (Instantiate(vrCameraGO.cameras[k].gameObject,Vector3.zero,Quaternion.identity) as GameObject).GetComponent<Camera>();
					foreach( Transform child in uiCameras[k].transform ) Destroy (child.gameObject);
					uiCameras[k].cullingMask = 2;
					uiCameras[k].clearFlags = CameraClearFlags.Depth;
					uiCameras[k].transform.SetParent(parentGO.transform);
					uiCameras[k].transform.localPosition = vrCameraGO.cameras[k].transform.localPosition;
					uiCameras[k].transform.localRotation = Quaternion.identity;
					eyeDist = Mathf.Abs (uiCameras[k].transform.localPosition.magnitude);
					eyeFieldOfView = uiCameras[k].fieldOfView;
					uiCameras[k].transform.localRotation = Quaternion.identity;
					uiCameras[k].SendMessage("DisableLensCorrection");
					if( uiCameras[k].GetComponent<ChangeCameraEye>()!=null ) uiCameras[k].GetComponent<ChangeCameraEye>().enabled=false;
				}
				float uiDist = 5f*(eyeDist/0.07f);
				transform.localPosition = new Vector3(0f,0f,uiDist);
				RectTransform rt = cv.gameObject.GetComponent<RectTransform>();
				rt.localScale = Vector3.one*(2f*scale*uiDist*Mathf.Tan(eyeFieldOfView*0.5f*Mathf.Deg2Rad))/rt.rect.height;
				if( bindType == BindType.FixedInCenter )
				{
					vrCameraGO.Init ();
					parentGO.transform.SetParent(vrCameraGO.vrCameraHeading.transform);
					parentGO.transform.localPosition = Vector3.zero;
					parentGO.transform.localRotation = Quaternion.identity;
				}
				else if( bindType == BindType.FixedOrientation )
				{
					vrCameraGO.Init ();
					parentGO.transform.SetParent(vrCameraGO.vrCameraHeading.transform);
					parentGO.transform.localPosition = Vector3.zero;
					parentGO.transform.localRotation = Quaternion.identity;
					transform.SetParent(vrCameraGO.transform);
					transform.rotation = vrCameraGO.vrCameraHeading.transform.rotation;
				}
				else if( bindType == BindType.AlwaysInViewField )
				{
					vrCameraGO.Init ();
					parentGO.transform.SetParent(vrCameraGO.transform);
					parentGO.transform.localPosition = Vector3.zero;
					parentGO.transform.localRotation = Quaternion.Euler(Vector3.up*vrCameraGO.vrCameraHeading.localRotation.eulerAngles.y);
					for( int k=0; k<uiCameras.Length; k++ )
					{
						uiCameras[k].transform.SetParent(vrCameraGO.vrCameraHeading.transform);
						uiCameras[k].transform.localRotation = Quaternion.identity;
					}
					fieldViewAngle = Mathf.Atan(cv.pixelRect.width*rt.localScale.x*0.5f/uiDist)*Mathf.Rad2Deg*1.2f;
				}
			}
		}

		initialized=true;
	}

	void DeInit()
	{
		if( !initialized ) return; 
		for( int k=0; k<uiCameras.Length; k++ ) Destroy(uiCameras[k].gameObject);
		Destroy(parentGO);
		initialized=false;
	}

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		if( bindType==BindType.AlwaysInViewField )
		{
			float angle = Mathf.DeltaAngle(vrCameraGO.vrCameraHeading.transform.localRotation.eulerAngles.y,parentGO.transform.localRotation.eulerAngles.y);
			if( Mathf.Abs(angle)>fieldViewAngle )
			{
				float dAngle = -(Mathf.Abs(angle)-fieldViewAngle)*Mathf.Sign(angle);
				parentGO.transform.Rotate(0f,dAngle*Time.deltaTime*10f,0f);
			}
		}
	}

	void OnDisable()
	{
		//transform.SetParent(null);
		DeInit ();
	}

	void OnDestroy()
	{
		DeInit ();
	}
}
