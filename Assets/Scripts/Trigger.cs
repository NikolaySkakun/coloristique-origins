using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class Trigger : Obj 
{
	public event Game.DVoid OnTriggerEnterPlayer;
	public event Game.DVoid OnTriggerExitPlayer;

	public enum TriggerType { OTHER, BALL, PLAYER, LIFT }
	public TriggerType type = TriggerType.OTHER;
	// GO's into triggers
	public ArrayList innerObjs = new ArrayList();
	
	//public enum State {EMPTY, PLAYER};
	//public State currentState = State.EMPTY;


	bool playerStay = false;

	public bool PlayerStay
	{
		get
		{
			return playerStay;
		}
	}

	static public Trigger Create(XmlNode xml)
	{
		if(xml.Attributes["tree"] != null && xml.Attributes["tree"].Value == "true")
		{
			Trigger trig = NonXmlCreate(Vector3.one, TriggerType.PLAYER);
			Join(Level.current, xml, trig);

			return trig;
		}

		float y = xml.Attributes["scaleY"] != null ? Game.GetFloat(xml, "scaleY") : 1f;
		Trigger trigger = NonXmlCreate(new Vector3(Game.GetFloat(xml, "scaleX"), y, Game.GetFloat(xml, "scaleZ")));
		Join(xml, trigger);
		if(xml.Attributes["type"] != null)
			trigger.type = (TriggerType)Enum.Parse(typeof(TriggerType), xml.Attributes["type"].Value, true);


		return trigger;
	}

	public static void Join(XmlNode xml, Trigger trigger)
	{
		float x = Game.GetFloat(xml, "x");
		float z = Game.GetFloat(xml, "z");
		
		float y = xml.Attributes["scaleY"] != null ? Game.GetFloat(xml, "scaleY") : 1f;

		
		Room room = objStack[objStack.Count - 1] as Room;
		
		Vector3 position = Vector3.zero;
		
		position.x = trigger.transform.localScale.x/2f - room.Size.x/2f + x*(room.Size.x - trigger.transform.localScale.x)/100f;
		position.z = trigger.transform.localScale.z/2f - room.Size.z/2f + z*(room.Size.z - trigger.transform.localScale.z)/100f;
		
		trigger.transform.position = room.side[2].transform.position - position + y*(Vector3.up/2f);
	}

	public static void Join(Level level, XmlNode xml, Trigger trigger)
	{
		float x = Game.GetFloat(xml, "x");
		float z = Game.GetFloat(xml, "z");
		
		Room room = level.room[Game.GetInt(xml, "room")];
		
		Vector3 position = Vector3.zero;
		
		position.x = trigger.transform.localScale.x/2f - room.Size.x/2f + x*(room.Size.x - trigger.transform.localScale.x)/100f;
		position.z = trigger.transform.localScale.z/2f - room.Size.z/2f + z*(room.Size.z - trigger.transform.localScale.z)/100f;
		
		trigger.transform.position = room.side[2].transform.position - position + Vector3.up/2f;
	}
	

	static public Trigger NonXmlCreate(Vector3 scale, TriggerType t = TriggerType.OTHER)
	{
		Trigger trigger = NonXmlCreate();
		trigger.gameObject.transform.localScale = scale;
		trigger.type = t;
		return trigger;
	}

	static public Trigger NonXmlCreate(TriggerType t)
	{
		Trigger trigger = NonXmlCreate();
		trigger.type = t;
		return trigger;
	}

	static public Trigger NonXmlCreate()
	{
		GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GO.name = "Trigger";
		GO.GetComponent<Renderer>().enabled = false;
		GO.GetComponent<Collider>().isTrigger = true;
		GO.layer = LayerMask.NameToLayer("Ignore Raycast");
		return GO.AddComponent<Trigger>() as Trigger;
	}

	void OnTriggerStay(Collider obj)
	{
		if(type == TriggerType.LIFT && (obj.transform.parent==Level.current.transform || obj.transform.parent==null))
			obj.transform.parent = transform.parent;
	}

	void OnTriggerEnter(Collider obj)
	{
		foreach(GameObject o in innerObjs)
		{
			if(o == obj.gameObject)
				return;
		}
		innerObjs.Add(obj.gameObject);

//		if (gameObject.tag == "Outlet" && obj.GetComponent<Ball> ()) 
//		{
//			obj.gameObject.GetComponent<SphereCollider>().isTrigger = true;
//		}

		if((type == TriggerType.PLAYER || type == TriggerType.OTHER) && obj.tag == "Player")
		{
			playerStay = true;

			if (OnTriggerEnterPlayer != null)
				OnTriggerEnterPlayer ();

			//Debug.Log("Player");
		}

		if(type == TriggerType.LIFT && (obj.transform.parent==Level.current.transform || obj.transform.parent==null))
			obj.transform.parent = transform.parent;
	}
	
	void OnTriggerExit(Collider obj)
	{
		//Debug.LogError(innerObjs.Count);
		innerObjs.Remove(obj.gameObject);

//		if (gameObject.tag == "Outlet" && obj.GetComponent<Ball> ()) 
//		{
//			obj.gameObject.GetComponent<SphereCollider>().isTrigger = false;
//		}

		if ((type == TriggerType.PLAYER || type == TriggerType.OTHER) && obj.tag == "Player")
		{
			playerStay = false;

			if (OnTriggerExitPlayer != null)
				OnTriggerExitPlayer ();
		}

		if(type == TriggerType.LIFT && obj.transform.parent == transform.parent)
		{
			if(obj.tag == "Player")
				obj.transform.parent = null;
			else
			obj.transform.parent = Level.current.transform;
		}
	}
	
	
	/*public bool IsInside(GameObject obj)
	{
		foreach(GameObject element in objs)
			if(element == obj)
				return true;
		
		return false;
	}*/
}
