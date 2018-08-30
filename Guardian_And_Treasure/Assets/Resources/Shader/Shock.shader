Shader "HIM/DragonBones/Shock"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Strength("Strength",Range(0,1)) =  .25
	}
	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex		: POSITION;
				float4 color		: COLOR;
				float2 uv			: TEXCOORD0;
			};

			struct v2f
			{
				float2 uv			: TEXCOORD0;
				fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed _Strength;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 main = tex2D(_MainTex, i.uv) * i.color;
				float alpha = main.a * i.color.a;
				fixed3 rgb = lerp(main.rgb, _Color.rgb, _Strength);
				main.rgb = rgb * main.a;
				return main;
			}
			ENDCG
		}
	}
}
