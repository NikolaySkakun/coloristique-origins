Shader "Invisible" {
  SubShader {
    Tags {"Queue" = "Geometry+100" } // earlier = hides stuff later in queue
    Lighting Off
    ZTest LEqual
    ZWrite On
    ColorMask 0
    Pass {}
  }
}