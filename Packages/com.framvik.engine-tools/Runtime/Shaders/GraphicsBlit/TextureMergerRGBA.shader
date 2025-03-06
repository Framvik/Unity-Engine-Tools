/// Used by TextureMerger Graphics.Blit() to copy channels from several textures to a single new texture.
/// _Channes are used to invert each channel, 1 = no inversion, -1 = inversion.
Shader "GraphicsBlit/TextureMergerRGBA"
{
    Properties
    {
        _TexR ("Texture R", 2D) = "white" {}
        _TexG ("Texture G", 2D) = "white" {}
        _TexB ("Texture B", 2D) = "white" {}
        _TexA ("Texture A", 2D) = "white" {}
        _Channels ("Channels", Vector) = (1,1,1,1)
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

            sampler2D _TexR;
            sampler2D _TexG;
            sampler2D _TexB;
            sampler2D _TexA;
            float4 _Channels;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 colR = tex2D(_TexR, i.uv);
                float4 colG = tex2D(_TexG, i.uv);
                float4 colB = tex2D(_TexB, i.uv);
                float4 colA = tex2D(_TexA, i.uv);
                float4 col = float4(0, 0, 0, 0);
                col.r = clamp(_Channels.r * -1, 0, 1) + (colR.r * _Channels.r);
                col.g = clamp(_Channels.g * -1, 0, 1) + (colG.g * _Channels.g);
                col.b = clamp(_Channels.b * -1, 0, 1) + (colB.b * _Channels.b);
                col.a = clamp(_Channels.a * -1, 0, 1) + (colA.a * _Channels.a);
                return col;
            }
            ENDCG
        }
    }
}
