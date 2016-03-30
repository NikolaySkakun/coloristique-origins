using UnityEngine;
using System.Collections;

public class Level_7__ : MonoBehaviour 
{
	Level level;


	void Start () 
	{
		level = Level.current;

		GameObject[] objs = new GameObject[] {
			level.room[0].side[2].line[0].gameObject,
			level.room[0].side[2].line[1].gameObject,
			level.room[2].side[2].line[0].gameObject,
			level.room[2].side[2].line[1].gameObject,
			level.room[3].side[2].line[2].gameObject,
			level.room[3].side[2].line[3].gameObject,
			level.room[4].side[2].line[2].gameObject,
			level.room[4].side[2].line[3].gameObject,

			level.room[9].side[5].line[2].gameObject,
			level.room[9].side[1].line[2].gameObject,

			level.room[10].side[5].line[2].gameObject,
			level.room[10].side[0].line[2].gameObject,

			level.room[11].side[4].line[2].gameObject,
			level.room[11].side[1].line[2].gameObject,

			level.room[12].side[4].line[2].gameObject,
			level.room[12].side[0].line[2].gameObject,
		};

		foreach(Ball b in level.ball)
		{
			b.transform.position += Vector3.up * 3f;
		}

		for(int i=0; i<objs.Length; ++i)
			objs[i].SetActive(true);

		for(int i=10; i<=12; ++i)
		{
			GameObject c = CustomObject.Cube(new Vector3(1.5f, 0.25f, 1.5f));

			//c.transform.localScale -= Vector3.up * c.transform.localScale.y / 2f;
			//c.transform.localScale += Vector3.right*0.25f + Vector3.forward*0.25f;
			//Vector3[] verts = c.GetComponent<MeshFilter>().mesh.vertices;

			//for(int j=0; j<verts.Length; ++j)
			//	if(verts[j].y > 0)
			//		verts[j].y = 0f;
				//Debug.LogWarning(verts[j]);
			//for(int j=2; j<6; ++j)
				//verts[i].y -= 0.5f;

			//c.GetComponent<MeshFilter>().mesh.vertices = verts;

			c.transform.position = level.room[i].transform.position + Vector3.up * c.transform.FindChild("QB").transform.localScale.y/2f;
			level.room[i].cell[0].transform.position += Vector3.up * c.transform.FindChild("QB").transform.localScale.y;
			c.transform.parent = Level.current.transform;
		}

		GameObject blockRoom = BlockRoom.Create();
		blockRoom.transform.position = level.room[1].transform.position;
		blockRoom.transform.parent = level.room[1].transform;

		level.room[1].side[4].gameObject.SetActive(false);

		level.room[2].side[4].gameObject.SetActive(false);
		level.room[3].side[1].gameObject.SetActive(false);
		level.room[4].side[0].gameObject.SetActive(false);

		level.room[5].side[1].gameObject.SetActive(false);
		level.room[5].side[5].gameObject.SetActive(false);

		level.room[6].side[0].gameObject.SetActive(false);
		level.room[6].side[5].gameObject.SetActive(false);

		level.room[7].side[1].gameObject.SetActive(false);
		level.room[7].side[4].gameObject.SetActive(false);

		level.room[8].side[0].gameObject.SetActive(false);
		level.room[8].side[4].gameObject.SetActive(false);
	}

}
