Shader "Custom/URP/PainterlyLighting"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        [HDR]_SpecularColor("Specular color", Color) = (1, 1, 1, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [Normal]_Normal("Normal", 2D) = "bump" {}
        _NormalStrength("Normal strength", Range(-2, 2)) = 1

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _ShadingGradient("Shading gradient", 2D) = "white" {}
        _PainterlyGuide("Painterly guide", 2D) = "white" {}
        _PainterlySmoothness("Painterly smoothness", Range(0, 1)) = 0.1
        _PainterlyScale("Painterly world scale", Float) = 0.3
    }

    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalRenderPipeline"
            "Queue"="Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
                float4 tangentOS  : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 tangentWS : TEXCOORD2;
                float3 bitangentWS : TEXCOORD3;
                float3 positionWS : TEXCOORD4;
            };

            TEXTURE2D(_MainTex);          SAMPLER(sampler_MainTex);
            TEXTURE2D(_Normal);           SAMPLER(sampler_Normal);
            TEXTURE2D(_PainterlyGuide);   SAMPLER(sampler_PainterlyGuide);
            TEXTURE2D(_ShadingGradient);  SAMPLER(sampler_ShadingGradient);

            float4 _Color;
            float4 _SpecularColor;
            float _NormalStrength;
            float _Glossiness;
            float _Metallic;
            float _PainterlySmoothness;
            float _PainterlyScale;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);

                OUT.normalWS   = TransformObjectToWorldNormal(IN.normalOS);
                
                float3 t = TransformObjectToWorldDir(IN.tangentOS.xyz);
                float3 b = cross(OUT.normalWS, t) * IN.tangentOS.w;

                OUT.tangentWS = t;
                OUT.bitangentWS = b;

                OUT.uv = IN.uv;

                return OUT;
            }

            // --------------------------
            // TRIPLANAR MAPPING (NO SEAMS)
            // --------------------------
            float SamplePainterGuideTriplanar(float3 worldPos, float3 worldNormal)
            {
                worldPos *= _PainterlyScale;

                float3 n = abs(normalize(worldNormal));
                float3 w = n / (n.x + n.y + n.z);

                float2 uvX = worldPos.yz;
                float2 uvY = worldPos.xz;
                float2 uvZ = worldPos.xy;

                float gX = SAMPLE_TEXTURE2D(_PainterlyGuide, sampler_PainterlyGuide, uvX).r;
                float gY = SAMPLE_TEXTURE2D(_PainterlyGuide, sampler_PainterlyGuide, uvY).r;
                float gZ = SAMPLE_TEXTURE2D(_PainterlyGuide, sampler_PainterlyGuide, uvZ).r;

                return gX * w.x + gY * w.y + gZ * w.z;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_Normal, sampler_Normal, IN.uv));
                normalTS.xy *= _NormalStrength;

                float3 normalWS = normalize(
                    normalTS.x * IN.tangentWS +
                    normalTS.y * IN.bitangentWS +
                    normalTS.z * IN.normalWS
                );

                float4 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _Color;

                // PAINTERLY GUIDE SIN COSTURAS
                float painterGuide = SamplePainterGuideTriplanar(IN.positionWS, normalWS);

                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);

                float NdotL = saturate(dot(normalWS, -lightDir) + 0.2);

                float diff = smoothstep(
                    painterGuide - _PainterlySmoothness,
                    painterGuide + _PainterlySmoothness,
                    NdotL
                );

                float3 viewDir = normalize(_WorldSpaceCameraPos - IN.positionWS);

                float3 refl = reflect(lightDir, normalWS);
                float vDotRefl = dot(viewDir, -refl);

                float specularThreshold = painterGuide + _Glossiness;

                float3 specular = 
                    _SpecularColor.rgb * 
                    mainLight.color * 
                    smoothstep(
                        specularThreshold - _PainterlySmoothness,
                        specularThreshold + _PainterlySmoothness,
                        vDotRefl
                    ) * _Glossiness;

                float atten = mainLight.distanceAttenuation * mainLight.shadowAttenuation;

                atten = smoothstep(
                    painterGuide - _PainterlySmoothness,
                    painterGuide + _PainterlySmoothness,
                    atten
                );

                float3 gradient = SAMPLE_TEXTURE2D(_ShadingGradient, sampler_ShadingGradient, float2(diff, 0)).rgb;

                float3 finalColor = (albedo.rgb * gradient * mainLight.color + specular) * atten;

                return float4(finalColor, albedo.a);
            }

            ENDHLSL
        }
    }
}
