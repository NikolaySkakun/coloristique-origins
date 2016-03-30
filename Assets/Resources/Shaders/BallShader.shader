Shader "BallShader" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
      
		_Border ("BorderSize", Range (0.0, 0.1)) = 0.01
		_BorderColor("BorderColor", Color) = (0,0,0,1)   
		_Illum ("Illumin (A)", 2D) = "white" {}
	}
	SubShader 
	{
		UsePass "Self-Illumin/VertexLit/BASE"
		
		 
		Pass 
		{

			Cull Front
			CGPROGRAM
         
       		uniform float   _Border;
       		uniform float4 _BorderColor;
         
       		#pragma vertex vert
       		#pragma fragment frag
      
			struct appdata 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f 
			{
				float4 pos : POSITION;
			};
   
			v2f vert(appdata v)
			{
				v2f o;
				float3 PosObject = v.vertex.xyz + (v.normal *_Border);
				o.pos = mul(UNITY_MATRIX_MVP,float4(PosObject,1));
				return o; 
			}
   
			float4 frag (v2f i) : COLOR
			{
				return  float4(_BorderColor);   
			}
			ENDCG
			
		}
		
		
	} 
}