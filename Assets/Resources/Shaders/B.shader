Shader "B"
{
 Properties
 {
 	_Color ("Main Color", Color) = (1,1,1,1)
 	_MainTex ("Base (RGB)", 2D) = "white" {}
 	_Illum ("Illumin (A)", 2D) = "white" {}
 	
   _LineColor ("Line Color", Color) = (0,0,0,1)
   _LineWidth ("Line Width", float) = 0.003
 }
 SubShader
 {
 //UsePass "Self-Illumin/VertexLit/BASE"
   Pass
   {
     Tags { "RenderType" = "Transparent" }
     Blend SrcAlpha OneMinusSrcAlpha
     AlphaTest Greater 0.5
 
     CGPROGRAM
     #pragma vertex vert
     #pragma fragment frag
 
     uniform float4 _LineColor;
     uniform float4 _GridColor;
     uniform float _LineWidth;
 
     // vertex input: position, uv1, uv2
     struct appdata
     {
       float4 vertex : POSITION;
       float4 texcoord1 : TEXCOORD0;
       float4 color : COLOR;
     };
 
     struct v2f
     {
       float4 pos : POSITION;
       float4 texcoord1 : TEXCOORD0;
       float4 color : COLOR;
     };
 
     v2f vert (appdata v)
     {
       v2f o;
       o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
       o.texcoord1 = v.texcoord1;
       o.color = v.color;
       return o;
     }
 
     fixed4 frag(v2f i) : COLOR
     {
       fixed4 answer;

       float lx = step(_LineWidth, i.texcoord1.x);
       float ly = step(_LineWidth, i.texcoord1.y);
       float hx = step(i.texcoord1.x, 1.0 - _LineWidth);
       float hy = step(i.texcoord1.y, 1.0 - _LineWidth);
 
       answer = lerp(_LineColor, _GridColor, lx*ly*hx*hy);
 
       return answer;
     }
     ENDCG
    }
 } 
 Fallback "Vertex Colored", 1
}