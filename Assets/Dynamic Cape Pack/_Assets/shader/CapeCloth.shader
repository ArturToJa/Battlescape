Shader "CapeCloth"
{
	Properties
	{
		_mainColor("mainColor", Color) = (0,0,0,0)
		_albedo("albedo", 2D) = "white" {}
		_normal("normal", 2D) = "white" {}
		_normalMultiplier("normalMultiplier", Float) = 0
		_metallic("metallic", 2D) = "white" {}
		_smoothness("smoothness", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _normal;
		uniform float4 _normal_ST;
		uniform float _normalMultiplier;
		uniform sampler2D _albedo;
		uniform float4 _albedo_ST;
		uniform float4 _mainColor;
		uniform sampler2D _metallic;
		uniform float4 _metallic_ST;
		uniform float _smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_normal = i.uv_texcoord * _normal_ST.xy + _normal_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _normal, uv_normal ), _normalMultiplier );
			float2 uv_albedo = i.uv_texcoord * _albedo_ST.xy + _albedo_ST.zw;
			o.Albedo = ( tex2D( _albedo, uv_albedo ) * _mainColor ).rgb;
			float2 uv_metallic = i.uv_texcoord * _metallic_ST.xy + _metallic_ST.zw;
			o.Metallic = tex2D( _metallic, uv_metallic ).r;
			o.Smoothness = _smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}