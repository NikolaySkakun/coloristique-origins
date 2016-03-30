using UnityEngine;
using System.Collections;

public class ExampleClasss : MonoBehaviour {
	public static Material lineMaterial;
	static void CreateLineMaterial() {
		if (!lineMaterial) {
			lineMaterial = Game.BaseMaterial;//new Material("Shader \"Lines/Colored Blended\" {" + "SubShader { Pass { " + "    Blend SrcAlpha OneMinusSrcAlpha " + "    ZWrite Off Cull Off Fog { Mode Off } " + "    BindChannels {" + "      Bind \"vertex\", vertex Bind \"color\", color }" + "} } }");
			lineMaterial.color = Color.red;
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	void Start()
	{

		CreateLineMaterial();
	}

	void Update()
	{
		Graphics.DrawMeshNow(CustomMesh.CurveWall(), Vector3.zero, Quaternion.Euler(Vector3.zero));
	}

	void OnPostRender() {

		GL.PushMatrix();
		lineMaterial.SetPass(0);
		GL.LoadOrtho();
		GL.Color(Color.red);
		GL.Begin(GL.TRIANGLES);
		GL.Vertex3(0.0F, 0.1351F, 0);
		GL.Vertex3(0.0F, 0.3F, 0);
		GL.Vertex3(0.5F, 0.3F, 0);
		GL.End();
		GL.Color(Color.yellow);
		GL.Begin(GL.TRIANGLES);
		GL.Vertex3(0.5F, 0.25F, -1);
		GL.Vertex3(0.5F, 0.1351F, -1);
		GL.Vertex3(0.1F, 0.25F, -1);
		GL.End();
		GL.PopMatrix();
	}
}