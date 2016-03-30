Shader "BaseBack" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
	Tags {"Queue" = "Geometry-10" }
		Pass 
		{
			Material 
			{
				Ambient [_Color]
			} 
			Lighting On
			ZWrite On
			 //Cull Off
			//SeparateSpecular On
			SetTexture [_MainTex] {
				//constantColor (1,1,1,1)
				Combine texture * primary DOUBLE//, constant // UNITY_OPAQUE_ALPHA_FFP
			} 
			

		}
	}
}