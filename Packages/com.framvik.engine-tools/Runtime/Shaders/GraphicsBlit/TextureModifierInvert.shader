/// Used by TextureModifier Graphics.Blit() to make texture inverted.
Shader "GraphicsBlit/TextureModifierInvert"
{
    Properties
    {
        _Texture ("Texture RGB", 2D) = "white" {}
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

            sampler2D _Texture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 tex = tex2D(_Texture, i.uv);
                float3 col = float3(1.0 - tex.r, 1.0 - tex.g, 1.0 - tex.b);
                return float4(col.rgb, 1);
            }
            ENDCG
        }
    }
}
