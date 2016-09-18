using UnityEngine;
using System.Collections;

public class Side : Obj 
{
	public static Material whiteSide, blackSide;
	
	public Line[] line;
	public int index = 0;


	Vector2 size;

	public Vector2 Size
	{
		set
		{
			size = value;
		}
		get
		{
			return size;
		}
	}
	
	static Side()
	{
		whiteSide = new Material(Shader.Find("SideShader"));
		whiteSide.SetColor("_LineColor", Game.Black);
		whiteSide.SetColor("_Color", Game.White);
		
		
		
		blackSide = new Material(Shader.Find("SideShader"));
		blackSide.SetColor("_LineColor", Game.White);
		blackSide.SetColor("_Color", Game.Black);
			
	}
	
	static public Side Create(Obj.Colour colour, Vector2 size)
	{
		GameObject GO = CustomObject.CreatePrimitive(PrimitiveType.Quad);
		//Destroy(GO.GetComponent<MeshCollider>());
		float colliderSize = 0.2f;
		//GO.AddComponent<BoxCollider>();
		//.size = new Vector3(1, 1, colliderSize);
		//GO.GetComponent<BoxCollider>().center = new Vector3(0, 0, colliderSize/2f);
		//GO.GetComponent<Renderer>().material = new Material(Shader.Find("Base"));
		GO.name = "Side";
		GO.tag = "Side";
		
		Side side = GO.AddComponent<Side>() as Side;
		side.color = colour;
		GO.GetComponent<Renderer>().material.color = Game.GetColor(side.color = colour);
		//GO.layer = LayerMask.NameToLayer("Side");
		
		side.Size = size;
		
		side.AddLines();
		//GO.renderer.material = GetMaterial(side.color = colour); //!!!!!!!!!!!
		
		return side;
	}

	static public Side Create(Obj.Colour colour, Vector3 size)
	{
		GameObject GO = CustomObject.CreatePrimitive(PrimitiveType.Quad);
		//Destroy(GO.GetComponent<MeshCollider>());
		float colliderSize = 2f / size.z;
		//GO.AddComponent<BoxCollider>();//.size = new Vector3(1, 1, colliderSize);
		//GO.GetComponent<BoxCollider>().center = new Vector3(0, 0, colliderSize/2f);
		//GO.GetComponent<Renderer>().material = new Material(Shader.Find("Base"));
		GO.name = "Side";
		GO.tag = "Side";
		
		Side side = GO.AddComponent<Side>() as Side;
		side.color = colour;

		GO.GetComponent<Renderer>().material.color = Game.GetColor(side.color = colour);


		//GO.GetComponent<Renderer>().sharedMaterial.color = Game.GetColor(side.color = colour);
		//GO.layer = LayerMask.NameToLayer("Side");
		
		side.Size = new Vector2(size.x, size.y);
		
		side.AddLines();
		//GO.renderer.material = GetMaterial(side.color = colour); //!!!!!!!!!!!
		
		return side;
	}

	public void AddLines()
	{
		if(line == null)
		{
		line = new Line[4];

			for(int i=0; i<line.Length; ++i)
			{
				line[i] = Line.Create(size[i/2]);
				line[i].name = "Line_" + i.ToString();
				line[i].GetComponent<Renderer>().material.color = Game.ReverseColor(Game.GetColor(color));
				line[i].color = Game.ReverseColor(color);
				Vector3 position = Vector3.zero;

				position[i/2] += 0.5f*(i%2==0 ? 1 : -1) + (line[i].transform.localScale.x/2f)*(i%2==0 ? -1 : 1);
				line[i].transform.localPosition += position;

				if(i > 1)
					line[i].transform.localEulerAngles += Vector3.forward * 90;

				line[i].transform.parent = transform;

				//AddDrawLineAnimation(line[i]);
			}
		}
	}

	void Start()
	{
		//base.Start();
		StartCoroutine(PostDrawing(Game.drawTime));

	}

	IEnumerator PostDrawing(float waitTime) 
	{
		yield return new WaitForSeconds(waitTime);

		foreach(Line l in line)
			if(l.hasClone)
			{
				//if(l.animation.isPlaying)
					//l.animation.Stop();
				l.PostDrawing();
			}
	}
	
	public void AddDrawAnimation()
	{
		//foreach(Line l in line)
			AddDrawLineAnimation();
	}

	void AddDrawLineAnimation()
	{
		for(int u=0; u<line.Length; ++u)
		{
			Animation anim;

			if(line[u].gameObject.GetComponent<Animation>() == null)
				anim = line[u].gameObject.AddComponent<Animation>() as Animation;
			else
				anim = line[u].gameObject.GetComponent<Animation>() as Animation;

			if(anim == null)
				continue;

			anim.playAutomatically = false;

			AnimationCurve[] scale = new AnimationCurve[3];
			AnimationCurve[] position = new AnimationCurve[3];

			for(int i=0; i<scale.Length; ++i)
			{
				scale[i] = new AnimationCurve(
					new Keyframe(0, i==1 ? 0 : line[u].transform.localScale[i]), 
					//new Keyframe(drawTime*line[u].transform.localScale[i], line[u].transform.localScale[i]));
					new Keyframe(Game.drawTime, i==1 ? 1 : line[u].transform.localScale[i]));
					//new Keyframe(line[u].hasClone ? drawTime*line[u].transform.localScale[i] : drawTime, line[u].transform.localScale[i]));

				position[i] = new AnimationCurve(
					//new Keyframe(0, ((u/2 == 0 && i==1) || (u/2 == 1 && i==0)) ? ( (u%2==0 && index<4 || u%2!=0 && index>3) ? -1 : 1 )*Mathf.Abs(line[u].transform.localScale.y)/2f : line[u].transform.localPosition[i]), //*0.5f
					new Keyframe(0, ((u/2 == 0 && i==1) || (u/2 == 1 && i==0)) ? ( (u%2==0 && index<4 || u%2!=0 && index>3) ? -1 : 1 )*0.5f : line[u].transform.localPosition[i]),
					new Keyframe(Game.drawTime, ((u/2 == 0 && i==1) || (u/2 == 1 && i==0)) ? 0 : line[u].transform.localPosition[i]));
			}

		//scale[1] = new AnimationCurve(new Keyframe(0, 0), new Keyframe(drawTime, 1));
		//scale[2] = new AnimationCurve(new Keyframe(0, 0), new Keyframe(drawTime, 1));
		
			AnimationClip clip = new AnimationClip();
			clip.legacy = true;
			clip.SetCurve("", typeof(Transform), "localScale.x", scale[0]);
			clip.SetCurve("", typeof(Transform), "localScale.y", scale[1]);
			clip.SetCurve("", typeof(Transform), "localScale.z", scale[2]);

			clip.SetCurve("", typeof(Transform), "localPosition.x", position[0]);
			clip.SetCurve("", typeof(Transform), "localPosition.y", position[1]);
			clip.SetCurve("", typeof(Transform), "localPosition.z", position[2]);
		


			anim.AddClip(clip, "Draw");

			//if(line[u].hasClone)
				//line[u].clone.animation = 
		}
	}

	static public Material GetMaterial(Obj.Colour color)
	{
		if(color == Obj.Colour.WHITE)
			return whiteSide;
		else if(color == Obj.Colour.BLACK)
			return blackSide;
		else
		{
			Debug.LogError("Game -> GetMaterial()");
			return null;
		}
	}

	override public void Repaint()
	{
		base.Repaint();

		foreach(Line l in line)
			l.Repaint();

		foreach(Renderer rend in transform.GetComponentsInChildren<Renderer>())
		{
			if(rend.tag == "Side")
			{
				rend.material.color = Game.GetColor(color);
			}
		}
	}

	override public void Draw()
	{
		
	}
}
