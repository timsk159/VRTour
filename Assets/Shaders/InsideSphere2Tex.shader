// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Unlit_SphereInside2Tex" 
{
	Properties 
	{
		_MainTex ("TopTex", 2D) = "black" {}
		_SecondTex ("BotText", 2D) = "black" {}
		_Color("Main Color", Color) = (1,1,1,1)
	}
		
	SubShader 
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		ZWrite on
		// Non-lightmapped
		Pass 
		{
			Lighting Off
			Cull Front

			Blend SrcAlpha OneMinusSrcAlpha

		    CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			sampler2D _MainTex;
			sampler2D _SecondTex;

			float4 _MainTex_ST;
			float4 _SecondTex_ST;

			float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

			fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                fixed4 col;
				if(i.uv.y >= 0.5)
				{
					float2 newUV = TRANSFORM_TEX(i.uv, _MainTex);
					col = tex2D(_MainTex, newUV) * _Color;
				}
				else
				{
					float2 newUV = TRANSFORM_TEX(i.uv, _SecondTex);
					col = tex2D(_SecondTex, newUV) * _Color;
				}

                return col;
            }

			ENDCG
			/*
			SetTexture [_MainTex] 
			{ 
				constantColor[_Color]
				Combine texture * constant, texture * constant 
			}
			*/
		}
	}
}


