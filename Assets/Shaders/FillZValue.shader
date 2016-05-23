// Copyright (C) Stanislaw Adaszewski, 2013
// http://algoholic.eu

Shader "Custom/FillZValue" {
	Properties {
		_Z ("Z Value", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Geometry-10"}
		LOD 200
		
		ZTest Always
		ZWrite On
		// Blend SrcAlpha OneMinusSrcAlpha
		ColorMask 0
		// Cull Off
		
		Pass {
			CGPROGRAM
			#pragma exclude_renderers gles flash
			#pragma vertex vert
			#pragma fragment frag
	
			float _Z;
			
			struct AppData {
				float4 vertex : POSITION;
			};
	
			struct VertOut {
				float4 pos : POSITION;
			};
			
			struct FragOut {
				float4 col : COLOR;
				float dep : DEPTH;
			};
			
			VertOut vert(AppData IN) {
				VertOut o;
				o.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
				return o;
			}
			
			FragOut frag(VertOut IN) {
				FragOut o;
				
				o.col = float4(1,0,0,1);
				o.dep = _Z;
				
				return o;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
