Shader "MaskedCutoutTexture"
{
   Properties
   {
      _MainTex ("Base (RGB)", 2D) = "white" {}
      _Mask ("Culling Mask", 2D) = "white" {}
      _Cutoff ("Alpha cutoff", Range (0,1)) = 0.1
   }
   SubShader
   {
      Tags {"Queue"="Transparent"}
      Lighting Off
      ZWrite Off
      //BlendOp Add
      //Blend SrcColor OneMinusSrcColor
      //Blend SrcAlpha OneMinusSrcAlpha
      //Blend SrcAlpha DstColor
      AlphaTest GEqual [_Cutoff]
      Pass
      {
      	Blend One One
        SetTexture [_Mask] {combine texture}
        SetTexture [_MainTex] {combine texture, previous}
      }
   }
}