Shader "Custom/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0.0, 10.0)) = 1.0
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
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float _BlurSize;
            float _MainTex_TexelSize;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 sum = float4(0.0f, 0.0f, 0.0f, 0.0f);
                
                // Apply horizontal blur
                float stepSize = _BlurSize * _MainTex_TexelSize;
                sum += tex2D(_MainTex, i.uv - 4.0 * stepSize);
                sum += tex2D(_MainTex, i.uv - 3.0 * stepSize);
                sum += tex2D(_MainTex, i.uv - 2.0 * stepSize);
                sum += tex2D(_MainTex, i.uv - 1.0 * stepSize);
                sum += tex2D(_MainTex, i.uv);
                sum += tex2D(_MainTex, i.uv + 1.0 * stepSize);
                sum += tex2D(_MainTex, i.uv + 2.0 * stepSize);
                sum += tex2D(_MainTex, i.uv + 3.0 * stepSize);
                sum += tex2D(_MainTex, i.uv + 4.0 * stepSize);
                
                return sum / 9.0f; // 9 samples for Gaussian-like distribution
            }
            ENDCG
        }
    }
}