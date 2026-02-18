// URP-compatible replacement for the Skybox shader
// Recreates: gradient color blend between top and bottom colors
Shader "SyntyStudios/SkyboxUnlit"
{
    Properties
    {
        _ColorTop("Color Top", Color) = (0, 1, 0.7517242, 1)
        _ColorBottom("Color Bottom", Color) = (0.8161765, 0.9087222, 1, 1)
        _Offset("Offset", Float) = 0
        _Distance("Distance", Float) = 1
        _Falloff("Falloff", Range(0.001, 100)) = 0.001
        [Toggle] _UV_Based_Toggle("UV Based Toggle", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Background"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Background"
            "PreviewType" = "Skybox"
        }

        Pass
        {
            Name "Skybox"
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _UV_BASED_TOGGLE_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _ColorTop;
                float4 _ColorBottom;
                float _Offset;
                float _Distance;
                float _Falloff;
                float _UV_Based_Toggle;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float blendFactor;

                if (_UV_Based_Toggle > 0.5)
                {
                    // UV-based gradient
                    blendFactor = input.uv.y;
                }
                else
                {
                    // World position-based gradient
                    float worldY = input.positionWS.y;
                    float gradientValue = (_Offset + worldY) / max(_Distance, 0.001);
                    gradientValue = saturate(gradientValue);
                    blendFactor = saturate(pow(gradientValue, _Falloff));
                }

                half4 color = lerp(_ColorBottom, _ColorTop, blendFactor);
                return color;
            }
            ENDHLSL
        }
    }

    FallBack Off
}
