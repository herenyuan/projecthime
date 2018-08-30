﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color; // why ?
//c.rgb *= c.a; // why ?
Shader "DQ/Sprites/BGSprite"
{
	Properties
	{
		 _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		_Offset("Offset", Vector) = (1,1,1,1)
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
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile DUMMY PIXELSNAP_ON
#include "UnityCG.cginc"

		struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
	};

	fixed4 _Color;
	fixed4 _Offset;


	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif
		return OUT;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f IN) : SV_Target
	{

		float turn_light = 1.0f;
	if (IN.color.r == 0 && (IN.color.g > 0.43f && IN.color.g < 0.45f) && IN.color.b == 1) {
		IN.color.r = 1.0f;
		IN.color.g = 1.0f;
		float value = _Time.z % 2;
		if (value >= 1)
		{
			turn_light = (2 - value) + 1;
		}
		else {
			turn_light = value + 1;
		}

	}
		
		IN.texcoord += _Offset;
		fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color * turn_light;
		
		c.rgb *= c.a;
	return c;
	}
		ENDCG
	}
	}
}