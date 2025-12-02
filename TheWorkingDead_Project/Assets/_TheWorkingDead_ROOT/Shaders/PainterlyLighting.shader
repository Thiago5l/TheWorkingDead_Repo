Shader "Custom/PainterlyURP_WithGuide"
{
    Properties
    {
        _MainTex("Base Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalStrength("Normal Strength", Range(0,2)) = 1
        _ShadingLevels("Shading Levels", 2D) = "white" {}
        _PainterlyGuide("Painterly Guide", 2D) = "white" {}
        _ShadowThreshold("Shadow Threshold", Range(0.01,1)) = 0.25
        _OverlayStrength("Overlay Strength", Range(0,2)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionH : SV_POSITION;
                float3 normalWS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            TEXTURE2D(_ShadingLevels);
            SAMPLER(sampler_ShadingLevels);
            TEXTURE2D(_PainterlyGuide);
            SAMPLER(sampler_PainterlyGuide);

            float _NormalStrength;
            float _ShadowThreshold;
            float _OverlayStrength;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionH = TransformObjectToHClip(IN.positionOS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            float3 PainterlyOverlay(float3 normalWS, float2 uv)
            {
                // Factor de la normal
                float factor = saturate((normalWS.y + 1.0) * 0.5);

                // Muestra la guía painterly
                float guide = SAMPLE_TEXTURE2D(_PainterlyGuide, sampler_PainterlyGuide, uv).r;

                // Combinar con factor de normal
                factor = lerp(factor, guide, 0.5);

                // Aplicar threshold para discretizar el shading
                factor = floor(factor / _ShadowThreshold) * _ShadowThreshold;

                // Muestrear shading levels
                float3 overlay = SAMPLE_TEXTURE2D(_ShadingLevels, sampler_ShadingLevels, float2(factor,0)).rgb;

                // Exagerar efecto
                overlay = lerp(float3(1,1,1), overlay, _OverlayStrength);

                return overlay;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float3 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv).rgb;
                float3 normalMap = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, IN.uv));
                float3 normalWS = normalize(IN.normalWS + normalMap * _NormalStrength);

                float3 overlay = PainterlyOverlay(normalWS, IN.uv);

                return float4(baseColor * overlay, 1.0);
            }
            ENDHLSL
        }
    }
}
