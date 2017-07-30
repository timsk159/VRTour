Shader "Custom/Unlit_SphereInside" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
	}
		
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		ZWrite on
		// Non-lightmapped
		Pass 
		{
			Lighting Off
			Cull Front
		
			SetTexture [_MainTex] 
			{ 
				constantColor[_Color]
				Combine texture * constant, texture * constant 
			}
		}
	}
}


