Shader "Custom/URP/PainterlySketchy_ToonThreshold"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _Normal("Normal Map", 2D) = "bump" {}
        _RampTex("Toon Ramp", 2D) = "white" {}
        _RampThreshold("Toon Threshold", Range(0,1)) = 0.2
        _ShadowLines("Sketch Lines", 2D) = "white" {}
        _ShadowScale("Shadow Scale", Float) = 2.0
        _PainterlyScale("Painterly Scale", Float) = 4.0
        _LightSteps("Light Steps", Float) = 4.0
        _ShadowThreshold("Shadow Threshold", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque"
            "Queue"="Geometry"
            "RenderPipeline"="UniversalRenderPipeline"
        }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags{"LightMode"="UniversalForward"}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS   : TEXCOORD1;
                float3 tangentWS  : TEXCOORD2;
                float3 bitangentWS: TEXCOORD3;
                float2 uv         : TEXCOORD0;
                float3 positionWS : TEXCOORD4;
            };

            // PROPERTIES
            TEXTURE2D(_BaseMap);       SAMPLER(sampler_BaseMap);
            TEXTURE2D(_Normal);        SAMPLER(sampler_Normal);
            TEXTURE2D(_RampTex);       SAMPLER(sampler_RampTex);
            TEXTURE2D(_ShadowLines);   SAMPLER(sampler_ShadowLines);

            float4 _BaseColor;
            float _PainterlyScale;
            float _LightSteps;
            float _ShadowScale;
            float _ShadowThreshold;
            float _RampThreshold;

            // VERTEX
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);

                OUT.normalWS   = TransformObjectToWorldNormal(IN.normalOS);
                float3 t = TransformObjectToWorldDir(IN.tangentOS.xyz);
                float3 b = cross(OUT.normalWS, t) * IN.tangentOS.w;
                OUT.tangentWS = t;
                OUT.bitangentWS = b;

                OUT.uv = IN.uv;
                return OUT;
            }

            // TRIPLANAR SHADOW LINES SIN ANIMACIÓN
            float SampleShadowLinesTriplanar(float3 worldPos, float3 worldNormal)
            {
                worldPos *= _ShadowScale;
                float3 n = abs(normalize(worldNormal));
                float3 w = n / (n.x + n.y + n.z);

                float2 uvX = worldPos.yz;
                float2 uvY = worldPos.xz;
                float2 uvZ = worldPos.xy;

                float sX = SAMPLE_TEXTURE2D(_ShadowLines, sampler_ShadowLines, uvX).r;
                float sY = SAMPLE_TEXTURE2D(_ShadowLines, sampler_ShadowLines, uvY).r;
                float sZ = SAMPLE_TEXTURE2D(_ShadowLines, sampler_ShadowLines, uvZ).r;

                return (sX * w.x + sY * w.y + sZ * w.z);
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // Base color
                float4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                // Normal map intacto
                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_Normal, sampler_Normal, IN.uv));
                float3 normalWS = normalize(
                    normalTS.x * IN.tangentWS +
                    normalTS.y * IN.bitangentWS +
                    normalTS.z * IN.normalWS
                );

                // Main light
                Light light = GetMainLight();
                float NdotL = saturate(dot(normalWS, light.direction));

                // Aplicamos threshold para toon ramp
                float NdotL_thresh = saturate(NdotL - _RampThreshold);

                // Painterly steps
                float stepped = floor(NdotL_thresh * _LightSteps) / _LightSteps;

                // Toon ramp lookup
                float ramp = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, float2(stepped,0.5)).r;

                // Shadow lines triplanar
                float sketch = SampleShadowLinesTriplanar(IN.positionWS, normalWS);

                // Líneas solo en sombras profundas
                float shadowMask = step(NdotL, _ShadowThreshold); // 1 si NdotL < threshold
                sketch = lerp(1.0, sketch, shadowMask);

                // Combinamos ramp con sketch
                float lighting = ramp * sketch;

                float3 finalColor = albedo.rgb * lighting;

                return float4(finalColor,1.0);
            }

            ENDHLSL
        }
    }
}
