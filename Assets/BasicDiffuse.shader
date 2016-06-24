Shader "Custom/BasicDiffuse" {
	Properties {
		_EmissiveColor ("EmissiveColor", Color) = (1,1,1,1)
		_AmbientColor ("AmbientColo", Color) = (1,1,1,1)
		_Slider ("Slider", Range(0, 10)) = 5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM



		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float4 _EmissiveColor;
		float4 _AmbientColor;
		float _Slider;

		struct Input {
			float2 uv_MainTex;
		};

		//half _Glossiness;
		//half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float4 c;
			c = pow((_EmissiveColor + _AmbientColor), _Slider);
			o.Albedo = c.rgb;

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
