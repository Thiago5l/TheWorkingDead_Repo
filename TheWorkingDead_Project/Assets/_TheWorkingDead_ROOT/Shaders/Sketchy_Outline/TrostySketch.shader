Shader "Trosty/SketchURP"
{
    Properties
    {
        _ContourColor("Contour Color", Color) = (0,0,0,1)
        _ContourWidth("Contour Width", Range(0,0.1)) = 0.02
        _Amplitude("Amplitude", Range(0,0.1)) = 0.01
        _Speed("Speed", Float) = 6.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Front
        ZWrite Off

        Pass
        {
            Name "SketchOutline"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            float4 _ContourColor;
            float _ContourWidth;
            float _Amplitude;
            float _Speed;

            // Pseudo-random hash
            float hash(float2 seed)
            {
                return frac(sin(dot(seed.xy, float2(12.9898,78.233))) * 43758.5453);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 n = normalize(IN.normalOS);
                float h = hash(IN.uv.xy + floor(_Time.y * _Speed));
                float offset = _ContourWidth + _Amplitude * (h - 0.5);
                float3 displaced = IN.positionOS.xyz + n * offset;

                OUT.positionHCS = TransformObjectToHClip(displaced);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _ContourColor;
            }

            ENDHLSL
        }
    }

    FallBack Off
}
