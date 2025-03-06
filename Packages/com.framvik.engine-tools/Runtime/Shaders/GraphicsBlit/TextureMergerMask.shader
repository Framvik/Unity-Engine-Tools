/// Used by TextureMerger Graphics.Blit() to combine 2 textures using a mask.
Shader "GraphicsBlit/TextureMergerMask"
{
    Properties
    {
        _TexA ("Texture A", 2D) = "white" {}
        _TexB ("Texture B", 2D) = "white" {}
        _TexMask ("Texture Mask", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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

            sampler2D _TexA;
            sampler2D _TexB;
            sampler2D _TexMask;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 colA = tex2D(_TexA, i.uv);
                float4 colB = tex2D(_TexB, i.uv);
                float3 colM = tex2D(_TexMask, i.uv);
                float l = (colM.r+colM.g+colM.b)/3;
                return lerp(colA, colB, l);
            }
            ENDCG
        }
    }
}
