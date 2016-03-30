Shader "Custom/First" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
	}
	SubShader 
	{
        Pass 
        {
        	Color (1,1,1,0)
        
            Cull Off
        }
    }
	FallBack "Diffuse"
}
