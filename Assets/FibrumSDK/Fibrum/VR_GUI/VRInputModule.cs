using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class VRInputModule : BaseInputModule {
	
	public const int kLookId = -3;
	private PointerEventData lookData;
	private GameObject lastActiveButton;
	private float lookTimer;
	public float timeToLookPress=2f;
	private Image circleProgressBar;
	private Sprite circleProgressBarSprite;
	private Texture vrPointerTexture;

	StandaloneInputModule sim;

	private bool initialized=false;
	void Init()
	{
		if( initialized ) return;
		sim = GetComponent<StandaloneInputModule>();
		initialized=true;
	}

	public void SetProgressBarTexture(Sprite sprite)
	{
		Init ();
		circleProgressBarSprite = sprite;
	}

	public void SetVRpointerTexture(Texture vrptex)
	{
		Init ();
		vrPointerTexture = vrptex;
	}

		
	// name of button to use for click/submit
	public string submitButtonName = "Fire1";
		
	private bool _guiRaycastHit;
	public bool guiRaycastHit {
		get {
			return _guiRaycastHit;
		}
	}
		
	// the UI element to use for the cursor
	public RectTransform cursor;
		
	public bool ignoreInputsWhenLookAway = true;

	private GameObject currentLook;
	private GameObject currentPressed;
	private float nextAxisActionTime;

	// use screen midpoint as locked pointer location, enabling look location to be the "mouse"
	private PointerEventData GetLookPointerEventData() {
		if( sim.enabled ) return null;
		Vector2 lookPosition;
		lookPosition.x = Screen.width/2;
		lookPosition.y = Screen.height/2;
		if (lookData == null) {
			lookData = new PointerEventData(eventSystem);
		}
		lookData.Reset();
		lookData.delta = Vector2.zero;
		lookData.position = lookPosition;
		lookData.scrollDelta = Vector2.zero;
		eventSystem.RaycastAll(lookData, m_RaycastResultCache);
		lookData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
		if (lookData.pointerCurrentRaycast.gameObject != null) {
			_guiRaycastHit = true;
		} else {
			_guiRaycastHit = false;
		}
		m_RaycastResultCache.Clear();
		return lookData;
	}
		
	private void UpdateCursor(PointerEventData lookData) {

		if( sim.enabled ) return;
		if (cursor != null) {
			if (lookData.pointerCurrentRaycast.gameObject!=null) {
				RectTransform draggingPlane = lookData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>();
				Vector3 globalLookPos;
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, lookData.position, lookData.enterEventCamera, out globalLookPos)) {
					cursor.gameObject.SetActive(true);
					cursor.position = globalLookPos;
					cursor.rotation = draggingPlane.rotation;
				} else {
					cursor.gameObject.SetActive(false);
				}
			} else {
				cursor.gameObject.SetActive(false);
			}
		}
	}

			
	private void ClearSelection() {
		if( sim.enabled ) return;
		if (eventSystem.currentSelectedGameObject) {
			eventSystem.SetSelectedGameObject(null);
		}
	}

	private void Select(GameObject go) {
		if( sim.enabled ) return;
		ClearSelection();
		if (ExecuteEvents.GetEventHandler<ISelectHandler> (go)) {
			eventSystem.SetSelectedGameObject(go);
		}
	}
		
	private bool SendUpdateEventToSelectedObject() {
		if( sim.enabled ) return false;
		if (eventSystem.currentSelectedGameObject == null)
			return false;
		BaseEventData data = GetBaseEventData ();
		ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
		return data.used;
	}

	void SimulatePress()
	{
		if( sim.enabled ) return;
		ClearSelection();
		lookData.pressPosition = lookData.position;
		lookData.pointerPressRaycast = lookData.pointerCurrentRaycast;
		lookData.pointerPress = null;
		if (currentLook != null) {
			currentPressed = currentLook;
			GameObject newPressed = null;
			newPressed = ExecuteEvents.ExecuteHierarchy (currentPressed, lookData, ExecuteEvents.pointerDownHandler);
			if (newPressed == null) {
				newPressed = ExecuteEvents.ExecuteHierarchy (currentPressed, lookData, ExecuteEvents.pointerClickHandler);
				if (newPressed != null) {
					currentPressed = newPressed;
				}
			} else {
				currentPressed = newPressed;
				ExecuteEvents.Execute (newPressed, lookData, ExecuteEvents.pointerClickHandler);
			}
			ExecuteEvents.ExecuteHierarchy (currentPressed, lookData, ExecuteEvents.pointerUpHandler);
		}
	}

	GameObject GetCanvasOfUI(GameObject ui)
	{
		if( ui.GetComponent<Canvas>() != null ) return ui;
		else if( ui.transform.parent!=null ) return GetCanvasOfUI(ui.transform.parent.gameObject);
		else return null;
	}

	public override void Process() {

		if( sim.enabled ) { enabled = false; return; }

		bool needActivateCursor=false;
		if( cursor != null )
		{
			needActivateCursor = cursor.gameObject.activeSelf;
			cursor.gameObject.SetActive(false);
		}
		SendUpdateEventToSelectedObject();

		PointerEventData lookData = GetLookPointerEventData();
		currentLook = lookData.pointerCurrentRaycast.gameObject;

		if (needActivateCursor && cursor!=null) cursor.gameObject.SetActive(true);

		if ( currentLook == null) {
			ClearSelection();
		}

		HandlePointerExitAndEnter(lookData,currentLook);

		UpdateCursor(lookData);

		if (Input.GetButtonDown (submitButtonName) && currentLook != null) {
			SimulatePress();
		}
		if( currentLook != null )
		{
			bool clickable=false;
			GameObject canvasGO = GetCanvasOfUI(currentLook);
			if(	currentLook.transform.gameObject.GetComponent<Button>()!=null ) clickable=true;
			if( currentLook.transform.parent!=null )
			{
				if( currentLook.transform.parent.gameObject.GetComponent<Button>()!=null ) clickable=true;
				if( currentLook.transform.parent.gameObject.GetComponent<Toggle>()!=null ) clickable=true;
				if( currentLook.transform.parent.gameObject.GetComponent<Slider>()!=null ) clickable=true;
				if( currentLook.transform.parent.parent!=null )
				{
					if( currentLook.transform.parent.parent.gameObject.GetComponent<Slider>()!=null )
					{
						if( currentLook.name != "Handle" )	clickable=true;
					}
					if( currentLook.transform.parent.parent.gameObject.GetComponent<Toggle>()!=null ) clickable=true;					
				}
			}

			if( clickable )
			{
				if( lastActiveButton==currentLook )
				{
					if(circleProgressBar!=null)
					{
						RectTransform draggingPlane = lookData.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>();
						Vector3 globalLookPos;
						if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, lookData.position, lookData.enterEventCamera, out globalLookPos))
						{
							circleProgressBar.transform.position = globalLookPos;
							circleProgressBar.transform.rotation = draggingPlane.rotation;
							circleProgressBar.transform.Translate(-Vector3.forward*0.1f,Space.Self);
						}
						circleProgressBar.fillAmount = (Time.realtimeSinceStartup-lookTimer)/timeToLookPress;
					}
					else if( Time.realtimeSinceStartup-lookTimer>0 )
					{
						if( canvasGO.transform.FindChild("VRprogressBar")!=null )
						{
							circleProgressBar = canvasGO.transform.FindChild("VRprogressBar").gameObject.GetComponent<Image>();
							circleProgressBar.fillAmount = 0f;
							circleProgressBar.rectTransform.SetAsLastSibling();
							if( circleProgressBar.GetComponent<CanvasGroup>()==null )
							{
								CanvasGroup cg = circleProgressBar.gameObject.AddComponent<CanvasGroup>();
								cg.interactable = false;
								cg.blocksRaycasts = false;
							}
							circleProgressBar.gameObject.SetActive(true);
						}
						else
						{
							GameObject pointerGO = new GameObject();
							pointerGO.name = "VRprogressBar";
							circleProgressBar = pointerGO.AddComponent<Image>();
							circleProgressBar.rectTransform.SetParent(canvasGO.transform);
							pointerGO.transform.localScale = Vector3.one;
							float size = (canvasGO.GetComponent<RectTransform>().rect.width+canvasGO.GetComponent<RectTransform>().rect.height)*0.08f;
							circleProgressBar.rectTransform.sizeDelta = Vector2.one*size;
							circleProgressBar.sprite = circleProgressBarSprite;
							circleProgressBar.type = Image.Type.Filled;
							circleProgressBar.fillOrigin = 2;
							circleProgressBar.fillAmount = 0f;
							circleProgressBar.rectTransform.SetAsLastSibling();
							if( circleProgressBar.GetComponent<CanvasGroup>()==null )
							{
								CanvasGroup cg = circleProgressBar.gameObject.AddComponent<CanvasGroup>();
								cg.interactable = false;
								cg.blocksRaycasts = false;
							}
							if( circleProgressBar.sprite==null ) circleProgressBar.color = new Color(1f,1f,1f,0f);
						}
					}
					if( Time.realtimeSinceStartup-lookTimer>timeToLookPress )
					{
						circleProgressBar.gameObject.SetActive(false);
						circleProgressBar = null;
						//lastActiveButton = null;
						SimulatePress();
						lookTimer = Time.realtimeSinceStartup+timeToLookPress*3f;
					}
				}
				else
				{
					lastActiveButton=currentLook;
					lookTimer = Time.realtimeSinceStartup;
					if( circleProgressBar!=null )
					{
						circleProgressBar.gameObject.SetActive(false);
						circleProgressBar = null;
					}
				}
			}
			else
			{
				lastActiveButton = null;
				if( circleProgressBar!=null )
				{
					circleProgressBar.gameObject.SetActive(false);
					circleProgressBar = null;
				}
				eventSystem.SetSelectedGameObject(null);
			}
			if( cursor == null )
			{
				if( canvasGO.transform.FindChild("VRpointer")!=null )
				{
					cursor = canvasGO.transform.FindChild("VRpointer").gameObject.GetComponent<RectTransform>();
					cursor.SetAsLastSibling();
				}
				else
				{
					GameObject pointerGO = new GameObject();
					pointerGO.name = "VRpointer";
					RawImage pointerImage = pointerGO.AddComponent<RawImage>();
					pointerImage.rectTransform.SetParent(canvasGO.transform);
					pointerGO.transform.localScale = Vector3.one;
					float size = (canvasGO.GetComponent<RectTransform>().rect.width+canvasGO.GetComponent<RectTransform>().rect.height)*0.04f;
					pointerImage.rectTransform.sizeDelta = Vector2.one*size;
					pointerImage.rectTransform.pivot = new Vector2(0.3f,0.85f);
					cursor = pointerImage.rectTransform;
					pointerImage.rectTransform.SetAsLastSibling();
					pointerImage.texture = vrPointerTexture;
					if( pointerImage.texture==null ) pointerImage.color = new Color(1f,1f,1f,0f);
				}
			}
		}
		else
		{
			lastActiveButton = null;
			circleProgressBar = null;
			eventSystem.SetSelectedGameObject(null);
			cursor = null;
		}
	}   

}
