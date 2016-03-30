Shader "NewShader" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
		Pass 
		{
			Material 
			{
				Diffuse [_Color]
				Ambient [_Color]
			} 
			Lighting On
			//SeparateSpecular On
			SetTexture [_MainTex] {
				//constantColor (1,1,1,1)
				Combine texture * primary DOUBLE, constant // UNITY_OPAQUE_ALPHA_FFP
			} 
		}
	}
}
