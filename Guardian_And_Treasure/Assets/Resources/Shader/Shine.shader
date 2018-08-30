Shader "HIM/Shine"
{
	Properties
	{
		_Ink("Ink",Color) = (1,1,1,1)
		_Shine("Shine",Range(0,1)) =  .25
		_Rate("Rate",Range(0,255)) = 30
		_Alpha("Alpha",Range(0,1))=.25
	}
	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent"
		}
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _Ink;
			float _Shine;
			float _Rate;
			float _Alpha;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = col.rgb * _Ink.rgb;
				col.rgb = col.rgb + abs(sin(_Time.x * _Rate)) * _Shine;
				col.a = col.a * _Alpha;
				return col;
			}
			ENDCG
		}
	}
}
