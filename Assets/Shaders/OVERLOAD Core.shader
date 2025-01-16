Shader "Custom/OVERLOAD core"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _EmissionColor ("Emission Color", Color) = (1, 0, 0, 1)
        _OverloadIntensity ("Overload Intensity", Range(0, 10)) = 1
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OverloadIntensity;
            float _PulseSpeed;
            float4 _BaseColor;
            float4 _EmissionColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
                float4 color = _BaseColor;
                color.rgb += _EmissionColor.rgb * pulse * _OverloadIntensity;

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
