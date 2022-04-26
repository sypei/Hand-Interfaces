Shader "GuineaLion/LightweightBook/Standard (Alpha Blend)" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _NormalMap("Normal", 2D) = "bump" {}
		[NoScaleOffset] _MetallicMap("Metallic", 2D) = "black" {}
		_InkMetalic("Ink Metalic", range(0,1)) = 1


		[Header(Page Atlas properties)]
		[NoScaleOffset] _PaperContent ("Content Texture", 2D) = "white" {}
		
		_ColorA ("Transparency Color", Color) = (0,0,0,0)
		//[Enum(Fade, 0, Cutoff, 1)] _AlphaBlend("Alpha Blend", float) = 0
		
		[Space][Space]
		_Progress("Progress", float) = 0
		_ProgressThreshold("Progress Threshold", range(0,0.4)) = 0.02
		_Pages ("Pages", Int) = 2
		_Columns ("Columns", Int) = 2
		_Rows ("Rows", Int) = 2
		
		[Header(Page Distortion)]
		_Open("Open Ammount", range(0,1)) = 1
		_WaveHeight("Wave Height", range(0, 0.1)) = 0.026
		_WaveLength("Wave Lenght", float) = 0.43
		_WaveAntecipation("Wave Antecipation", range(-3, 3)) = 2
		_FlipPageOffset("Flip Page Offset", float) = 0.0001

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard vertex:vert fullforwardshadows addshadow
		#define _IMAGEMODE_ALPHABLEND
		#pragma target 3.0
		#include "AnimatedBook.cginc"

		sampler2D _MainTex, _NormalMap, _MetallicMap;

		struct Input {
			float2 uv_MainTex;
			float4 data; //xy = pageUv | z = isPage | w = shadowValue
		};

		fixed4 _Color;
		half _InkMetalic;

		void vert (inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input, o);
			CalculateVertices(v, o.data);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 col = tex2D (_MainTex, IN.uv_MainTex) * _Color;

			col = CalculateColors(col, IN.data);

			o.Albedo = col.rgb;
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
			o.Metallic = lerp(tex2D(_MetallicMap, IN.uv_MainTex).r, _InkMetalic, col.a);

		}
		ENDCG

	}
	FallBack "Diffuse"
}
