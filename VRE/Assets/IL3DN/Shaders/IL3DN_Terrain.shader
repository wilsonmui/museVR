// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "IL3DN/Terrain First-Pass"
{
	Properties
	{
		[HideInInspector]_Control("Control", 2D) = "white" {}
		[HideInInspector]_Splat3("Splat3", 2D) = "white" {}
		[HideInInspector]_Splat2("Splat2", 2D) = "white" {}
		[HideInInspector]_Splat1("Splat1", 2D) = "white" {}
		[HideInInspector]_Splat0("Splat0", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0,0,0)
		_Color1("Color 1", Color) = (0,0,0,0)
		_Color2("Color 2", Color) = (0,0,0,0)
		_Color3("Color 3", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry-100" "SplatCount"="4" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Control;
		uniform float4 _Control_ST;
		uniform float4 _Color0;
		uniform sampler2D _Splat0;
		uniform float4 _Splat0_ST;
		uniform float4 _Color1;
		uniform sampler2D _Splat1;
		uniform float4 _Splat1_ST;
		uniform float4 _Color2;
		uniform sampler2D _Splat2;
		uniform float4 _Splat2_ST;
		uniform float4 _Color3;
		uniform sampler2D _Splat3;
		uniform float4 _Splat3_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Control = i.uv_texcoord * _Control_ST.xy + _Control_ST.zw;
			float4 tex2DNode5_g6 = tex2D( _Control, uv_Control );
			float dotResult20_g6 = dot( tex2DNode5_g6 , float4(1,1,1,1) );
			float SplatWeight22_g6 = dotResult20_g6;
			float localSplatClip74_g6 = ( SplatWeight22_g6 );
			float SplatWeight74_g6 = SplatWeight22_g6;
			#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
				clip(SplatWeight74_g6 == 0.0f ? -1 : 1);
			#endif
			float4 SplatControl26_g6 = ( tex2DNode5_g6 / ( localSplatClip74_g6 + 0.001 ) );
			float2 uv_Splat0 = i.uv_texcoord * _Splat0_ST.xy + _Splat0_ST.zw;
			float2 uv_Splat1 = i.uv_texcoord * _Splat1_ST.xy + _Splat1_ST.zw;
			float2 uv_Splat2 = i.uv_texcoord * _Splat2_ST.xy + _Splat2_ST.zw;
			float2 uv_Splat3 = i.uv_texcoord * _Splat3_ST.xy + _Splat3_ST.zw;
			float4 weightedBlendVar9_g6 = SplatControl26_g6;
			float4 weightedBlend9_g6 = ( weightedBlendVar9_g6.x*( _Color0 * tex2D( _Splat0, uv_Splat0 ) ) + weightedBlendVar9_g6.y*( _Color1 * tex2D( _Splat1, uv_Splat1 ) ) + weightedBlendVar9_g6.z*( _Color2 * tex2D( _Splat2, uv_Splat2 ) ) + weightedBlendVar9_g6.w*( _Color3 * tex2D( _Splat3, uv_Splat3 ) ) );
			float4 MixDiffuse28_g6 = weightedBlend9_g6;
			o.Albedo = MixDiffuse28_g6.xyz;
			o.Alpha = 1;
		}

		ENDCG
	}

	Dependency "BaseMapShader"="ASESampleShaders/SimpleTerrainBase"
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17009
190;422;1673;994;944.678;517.3597;1;True;True
Node;AmplifyShaderEditor.FunctionNode;12;-445.678,-162.3597;Inherit;False;IL3DN - Four Splats First Pass Terrain;0;;6;c3e2b73ed8eb8c64681162a2abca2a89;0;3;59;FLOAT4;0,0,0,0;False;60;FLOAT4;0,0,0,0;False;62;FLOAT;0;False;2;FLOAT4;0;FLOAT;19
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;69,-161;Float;False;True;2;ASEMaterialInspector;0;0;Standard;IL3DN/Terrain First-Pass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;-100;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;1;SplatCount=4;False;1;BaseMapShader=ASESampleShaders/SimpleTerrainBase;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;0;0;12;0
ASEEND*/
//CHKSM=DBAAF9AE455784449CA906BD1C7012B4D484C236