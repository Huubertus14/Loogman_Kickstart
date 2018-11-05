Shader "Unlit/SonarUnlit2"
{
	Properties
	{
		_SonarBaseColor("Base Color",  Color) = (00, 0.0, 0.0, 0)
		_SonarWaveColor("Wave Color",  Color) = (1.0, 0.0, 0.0, 0)
		_SonarWaveParams("Wave Params", Vector) = (1, 20, 20, 10)
		_SonarWaveVector("Wave Vector", Vector) = (0, 0, 1, 0)
		_SonarStep("Step", Float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform float4 _SonarWaveColor;
			uniform float4 _SonarWaveParams; // Amp, Exp, Interval, Speed
			uniform float3 _SonarWaveVector;
			uniform float4 _SonarBaseColor;
			uniform float _SonarStep;

			struct vertexInput {
				float4 vertex : POSITION;
			};

			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
			};


			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;

				output.pos = UnityObjectToClipPos(input.vertex);
				output.worldPos = mul(unity_ObjectToWorld, input.vertex);

				return output;
			}

#pragma shader_feature VISIBLE

			float4 frag (vertexOutput input) : COLOR
			{
#ifdef VISIBLE
				float w = length(input.worldPos - _SonarWaveVector);
				w -= _SonarStep * _SonarWaveParams.w;				
#else
				float w = _SonarStep;
#endif
				return lerp(_SonarWaveColor, _SonarBaseColor, w);

			}

			ENDCG
		}
	}
}
