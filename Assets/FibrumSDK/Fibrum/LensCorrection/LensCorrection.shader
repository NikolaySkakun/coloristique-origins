Shader "Fibrum/LensCorrection" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
		_k ("k",float) = 0.8
		_kcube ("kCube",float) = 0.8
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		half4 pos : POSITION;
		half2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	
	half2 intensity;
	
	half _k;
	half _kcube;
	
	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	} 
	
	half4 frag(v2f i) : COLOR 
	{
	
 		half2  coords = i.uv;
		
		
		half r2 = (coords.x-0.5) * (coords.x-0.5) + (coords.y-0.5) * (coords.y-0.5); 

        half f = 0;
		
		f = 0.8 + r2 * (_k + _kcube * sqrt(r2));
		  
		half2 realCoordOffs;
		realCoordOffs.x = f*(coords.x-0.5)+0.5; 
		realCoordOffs.y = f*(coords.y-0.5)+0.5;
		
		fixed4 col;
		//fixed4 black = (0,0,0,0);
		
		if (realCoordOffs.x >= 0.0 && realCoordOffs.x <= 1.0 && realCoordOffs.y >= 0.0 && realCoordOffs.y <= 1.0) { 
			col = tex2D (_MainTex, realCoordOffs);	 
		} else {
			col = fixed4(0,0,0,0);	 
		}
		
		return col;
	}

 
	ENDCG 
	
Subshader {
 Pass {
	  ZTest Always Cull Off ZWrite Off
	  Fog { Mode off }      

      CGPROGRAM
      #pragma fragmentoption ARB_precision_hint_fastest 
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
  
}

Fallback off
	
} 