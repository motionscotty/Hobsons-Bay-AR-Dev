Shader "Imagine/ARShadowCatcherURP"
{
    Properties
    {
        _ShadowIntensity ("Intensity", Range (0, 1)) = 0.75
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
    }

    SubShader
    {
        Tags {"Queue"="AlphaTest" "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "UniversalMaterialType"="Unlit"}

        Pass
        {
            Name "ShadowCatcher"
            Tags {"LightMode"="UniversalForward"}

            Cull Back
            Blend OneMinusSrcAlpha OneMinusSrcAlpha  // Premultiply for catcher
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D_X(_CameraOpaqueTexture); SAMPLER(sampler_CameraOpaqueTexture);

            CBUFFER_START(UnityPerMaterial)
                float _ShadowIntensity;
                float4 _ShadowColor;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.screenPos = ComputeScreenPos(output.positionCS);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                half3 bgColor = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, screenUV).rgb;

                half shadow = 1.0;
                #if _MAIN_LIGHT_SHADOWS
                    float4 shadowCoord = GetShadowCoord(GetVertexPositionInputs(input.positionWS));
                    shadow = MainLightRealtimeShadow(shadowCoord);
                #endif

                half3 shadowCol = bgColor * _ShadowColor.rgb * (1.0 - shadow) * _ShadowIntensity;
                half alpha = (1.0 - shadow) * _ShadowIntensity;

                return half4(shadowCol, alpha);
            }
            ENDHLSL
        }
    }
}
