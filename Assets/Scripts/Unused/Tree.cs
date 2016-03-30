using UnityEngine;
using System.Collections;
using System.Xml;

/*
<trigger class="Trigger">
				<Trigger index="0" x="50" z="0" scaleX="4" scaleZ="4" type="Player"></Trigger>
				<Trigger index="1" x="100" z="24" scaleX="4" scaleZ="4" type="Player"></Trigger>
				<Trigger index="2" x="50" z="50" scaleX="10" scaleZ="12" type="Player"></Trigger>
			</trigger>

*/

public class Tree : Obj 
{
	public Ball[] ball;
	public Trigger[] trigger;

	public GameObject stem;
	public GameObject[] stick;

	int counter = 0;

	static float diameter = 0.1f, height = 4.2f;

	static Obj.Colour CharToColor(char c)
	{
		if(c == 'b')
			return Obj.Colour.BLACK;
		else
			return Obj.Colour.WHITE;
	}

	static Ball.BallType CharToBallType(char c)
	{
		if(c == 'n')
			return Ball.BallType.NORMAL;
		else
			return Ball.BallType.SMALL;
	}

	static public Tree Create(XmlNode xml)
	{
		Tree tree = Obj.Create<Tree>();

		int ballCount = Game.GetInt(xml, "ballCount");
		tree.stick = new GameObject[ballCount];
		tree.ball = new Ball[ballCount];
		tree.trigger = new Trigger[ballCount];

		tree.stem = CreateStick(diameter * 2f);
		tree.stem.transform.SetParent(tree.transform);

		tree.stem.transform.localPosition += Vector3.up * 0.025f;
		tree.stem.transform.localScale = new Vector3(1, height + ballCount*1.2f, 1);

		string ballColor = xml.Attributes["color"].Value;
		string ballSize = xml.Attributes["size"].Value;

		for(int i=0; i<ballCount; ++i)
		{
			tree.stick[i] = CreateStick(diameter / 1.5f);
			tree.stick[i].transform.localScale = new Vector3(1, 3f - 0.1f*i, 1);
			tree.stick[i].transform.localPosition = Vector3.up * (height + i);

			tree.stick[i].transform.localEulerAngles = new Vector3(60, i*(360f/ballCount) + (Random.Range(-10, 10)), 0);

			GameObject s = CreateStick(diameter / 1.5f);
			s.transform.localScale = new Vector3(1, 2.5f - 0.1f*i, 1);
			s.transform.localPosition = Vector3.up * (height + 0.5f + i);
			s.transform.localEulerAngles = new Vector3(60, i*(360f/ballCount) + (Random.Range(30, 330)), 0);

			s.transform.SetParent(tree.transform);


			tree.ball[i] = Ball.NonXmlCreate(CharToColor(ballColor[i]), CharToBallType(ballSize[i]));

			tree.ball[i].GetComponent<Rigidbody>().isKinematic = true;

			tree.ball[i].transform.position = tree.stick[i].transform.TransformPoint(Vector3.up) - Vector3.up*tree.ball[i].transform.localScale.x*0.7f;

			tree.stick[i].transform.SetParent(tree.transform);

			tree.ball[i].transform.SetParent(tree.transform);

			tree.trigger[i] = Trigger.Create(xml.ChildNodes[i]);
			GameObject circle = CustomObject.CircleBorder(0.5f, Colour.BLACK);
			circle.transform.SetParent(tree.trigger[i].transform);
			circle.transform.localScale = Vector3.one;
			circle.transform.localPosition = -Vector3.up*0.495f;

			tree.trigger[i].transform.SetParent(Level.current.transform);
			                            
		}

		Join(xml, Level.current, tree);

		foreach(Ball b in tree.ball)
			b.transform.parent = Level.current.transform;

		return tree;
	}


	static GameObject CreateStick(float d = 0.08f, float h = 1f)
	{
		GameObject parent = new GameObject("Stick");
		GameObject s = CustomObject.CreatePrimitive(PrimitiveType.Cylinder);

		s.transform.localScale = new Vector3(d, h/2f, d);
		s.transform.localPosition += Vector3.up * s.transform.localScale.y;
		s.GetComponent<Renderer>().material = Ball.whiteBall;

		s.transform.SetParent(parent.transform);

		return parent;
	}

	public static void Join(XmlNode xml, Level level, Tree t)
	{
		float x = Game.GetFloat(xml, "x");// float.Parse(xml.Attributes["x"].Value);
		float z = Game.GetFloat(xml, "z"); //float.Parse(xml.Attributes["z"].Value);
		
		Room room = level.room[Game.GetInt(xml, "room")];
		
		Vector3 position = Vector3.zero;

		position.x = 0.5f - room.Size.x/2f + x*(room.Size.x - 1)/100f;
		position.z = 0.5f - room.Size.z/2f + z*(room.Size.z - 1)/100f;
			
		t.transform.position = room.side[2].transform.position - position;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i = counter; i<trigger.Length; ++i)
		{
			if(trigger[i].PlayerStay)
			{
				//Debug.LogWarning("trigger");
				++counter;



				Animation anim = trigger[i].gameObject.AddComponent<Animation>() as Animation;
				AnimationClip clip = new AnimationClip();
				clip.legacy = true;
				AnimationCurve[] scale = new AnimationCurve[3];
				
				for(int u=0; u<scale.Length; ++u)
				{
					scale[u] = new AnimationCurve(
						new Keyframe(0, 1), 
						new Keyframe(Game.drawTime*0.75f, 0));

				}
				
				clip.SetCurve("", typeof(Transform), "localScale.x", scale[0]);
				clip.SetCurve("", typeof(Transform), "localScale.y", scale[1]);
				clip.SetCurve("", typeof(Transform), "localScale.z", scale[2]);
				
				//clip.SetCurve("", typeof(Transform), "localPosition.x", position[0]);
				//clip.SetCurve("", typeof(Transform), "localPosition.y", position[1]);
				//clip.SetCurve("", typeof(Transform), "localPosition.z", position[2]);



				anim.AddClip(clip, "Dying");
				anim.Play("Dying");

				ball[i].GetComponent<Rigidbody>().isKinematic = false;
				ball[i].GetComponent<Rigidbody>().WakeUp();

				break;
			}
		}
	}
}
