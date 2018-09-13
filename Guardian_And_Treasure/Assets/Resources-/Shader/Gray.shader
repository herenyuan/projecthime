// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HIM/Gray"
{
	Properties
	{
		_Color("Color",Color) = (1.0,1.0,1.0,1.0)
		_R("R",Range(.222,1.0)) = .222
		_G("G",Range(.707,1.0)) =.707
		_B("B",Range(.071,1.0)) = .071
	}
	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		LOD 200
		Cull off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			
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
			float _R;
			float _G;
			float _B;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);//投影矩阵变换
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);//创建2D纹理
				col.rgb = dot(col.rgb, fixed3(_R,_G,_B));//灰度公式
				return col;
			}
			ENDCG
		}
	}
}
