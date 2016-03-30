Shader "Custom/Test" {
        Properties {
                _MainTex ("Base (RGB)", 2D) = "white" {}
                _CL ("Clamp lo",Range (0,1)) = 1
                _CH ("Clamp hi",Range (0,1)) = 1
        }
        SubShader {
                Tags { "RenderType"="Opaque" }
                LOD 200
                
                CGPROGRAM
                #pragma surface surf Lambert

                sampler2D _MainTex;
                fixed _CL;
                fixed _CH;

                struct Input {
                        fixed2 uv_MainTex;
                };

                void surf (Input IN, inout SurfaceOutput o) {
                        half4 c = tex2D (_MainTex, IN.uv_MainTex);
                        o.Albedo = clamp(c, _CL,_CH);
                        o.Alpha = c.a;
                }
                ENDCG
        } 
        FallBack "Diffuse"
}