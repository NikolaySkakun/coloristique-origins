using UnityEngine;
using System.Xml;
using System.Collections;
using System;

abstract public class Obj : MonoBehaviour 
{
	public enum Colour { WHITE, BLACK, NONE };

	static public ArrayList objStack;

	public Colour color;
	public int levelIndex = 0;

	static Obj()
	{
		objStack = new ArrayList();
	}

	public Obj()
	{
		Game.DrawEvent += Draw;
		Game.DestroyEvent += Destroy;


		if(Level.current != null)
		{
			if(Level.current.Index == 0)
			{
				Game.DestroyEvent -= Destroy;
				levelIndex = Level.current.Index;
			}
		}
		else if(levelIndex == 0)
		{
			Game.DestroyEvent -= Destroy;
		}
	}
	
	static public T Create<T> () where T : Obj
	{
		Type type = typeof(T);
		return (new GameObject(type.ToString(), type)).GetComponent(type) as T;	
	}
	
	static public T Create<T> (XmlNode node) where T : Obj
	{
		return typeof(T).GetMethod("Create").Invoke(null, new object[]{node}) as T;	
	}
	
	static public object Create(Type T, XmlNode xml)
	{
		return T.GetMethod("Create").Invoke(null, new object[]{xml});	
	}
	
	static public T Create<T> (T obj, XmlNode xml) where T  : Obj
	{
		objStack.Add(obj);

		foreach(XmlNode node in xml.ChildNodes)
		{
			switch(node.Name)
			{
				case "join":
				{
					for(int i=0; i<node.ChildNodes.Count; ++i)
					{
						Type type = Type.GetType(node.ChildNodes[i].Name);
						type.GetMethod("Join").Invoke(null, new object[]{node.ChildNodes[i], obj});
					}
				} break;
				case "triggers":
				{
						
				} break;
				case "extra":
				{
					
				} break;
				default:
				{
					Type type = Type.GetType(node.Attributes["class"].Value);
					Array arr = Array.CreateInstance(type, node.ChildNodes.Count);
					typeof(T).GetField(node.Name).SetValue(obj, arr);
					
					for(int i=0; i<arr.Length; ++i)
					{
						Obj tmp = Obj.Create(type, node.ChildNodes[i]) as Obj;
						arr.SetValue(tmp, i);	
						tmp.transform.parent = obj.transform;
					}
				} break;
			}
		}
		objStack.Remove(obj);

		return obj;	
	}
	
	virtual public void Draw()
	{

	}

	virtual public void PostDrawing()
	{
		
	}

	virtual public void Repaint()
	{
		color = Game.ReverseColor(color);
		
		if(GetComponent<Renderer>() != null)
			GetComponent<Renderer>().material.color = Game.GetColor(color);
	}

	virtual public void Paint(Colour c)
	{
		color = c;
		
		if(GetComponent<Renderer>() != null)
			GetComponent<Renderer>().material.color = Game.GetColor(color);
	}

	public void SetColor(Obj.Colour c)
	{
		if(color != c)
			Repaint();
	}

	public void DestroyAnyway()
	{
		Game.DrawEvent -= Draw;
		Game.DestroyEvent -= Destroy;
		UnityEngine.Object.Destroy(gameObject);
	}

	virtual public void Destroy()
	{
		if(this != null)
		{
			Game.DrawEvent -= Draw;
			Game.DestroyEvent -= Destroy;
			UnityEngine.Object.Destroy(gameObject);
		}
	}
}