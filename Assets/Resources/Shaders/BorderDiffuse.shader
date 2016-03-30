Shader "BorderDiffuse" {


Properties {


    _Color ("Main Color", Color) = (1,1,1,1)


    _MainTex ("Base (RGB)", 2D) = "white" {}


}


SubShader {


    Tags { "RenderType"="Opaque" }


    LOD 200


 


CGPROGRAM


#pragma surface surf Lambert


 


sampler2D _MainTex;


fixed4 _Color;


 


struct Input 


{


    float2 uv_MainTex;


};


 


void surf (Input IN, inout SurfaceOutput o) 


{


    float2 finalUV = IN.uv_MainTex;


    float mutedColor = (tex2D(_MainTex, finalUV) * _Color) * 0.2f;


 


    float borderAmount = 0.025f;


 


    if( finalUV.x < borderAmount || finalUV.x > (0.95f + borderAmount) )


    {


        o.Albedo = mutedColor;


    }


    else if( finalUV.y < borderAmount || finalUV.y > (0.95f + borderAmount) )


    {


        o.Albedo = mutedColor;


    }


    else


    {


        o.Albedo = (tex2D(_MainTex, finalUV) * _Color);


    }


}


ENDCG


}


 


Fallback "VertexLit"


}