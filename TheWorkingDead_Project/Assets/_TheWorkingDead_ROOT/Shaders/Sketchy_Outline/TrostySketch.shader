Shader "Trosty/SketchOutlineURP"
{
    Properties
    {
        _ContourColor("Contour Color", Color) = (0,0,0,1)
        _ContourWidth("Contour Width", Range(0,0.1)) = 0.02
        _Amplitude("Noise Amplitude", Range(0,0.1)) = 0.01
        _Speed("Noise Speed", Float) = 6.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        Cull Front
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "SketchOutline"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _ContourColor;
            float _ContourWidth;
            float _Amplitude;
            float _Speed;

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

            // Hash noise function
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898,78.233))) * 43758.5453);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 n = normalize(IN.normalOS);
                float h = hash(IN.uv + floor(_Time.y * _Speed)); // frame-step noise
                float outline = _ContourWidth + _Amplitude * (h - 0.5);

                float3 offsetPos = IN.positionOS.xyz + n * outline;

                OUT.positionHCS = TransformObjectToHClip(offsetPos);
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
}
