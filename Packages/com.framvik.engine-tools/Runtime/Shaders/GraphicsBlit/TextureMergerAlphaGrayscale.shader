/// Used by TextureMerger Graphics.Blit() to copy a grayscale texture into the alpha channel of another.
Shader "GraphicsBlit/TextureMergerAlphaGrayscale"
{
    Properties
    {
        _TexRGB ("Texture RGB", 2D) = "white" {}
        _TexGrayscale ("Texture Grayscale", 2D) = "white" {}
        _InvertAlpha ("Invert Alpha", Float) = 1
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

            sampler2D _TexRGB;
            sampler2D _TexGrayscale;
            float _InvertAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_TexRGB, i.uv);
                float3 gray = tex2D(_TexGrayscale, i.uv);
                float a = (gray.r+gray.g+gray.b)/3;
                col.a = clamp(_InvertAlpha * -1, 0, 1) + (a * _InvertAlpha);
                return col;
            }
            ENDCG
        }
    }
}
