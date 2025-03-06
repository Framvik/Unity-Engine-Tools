/// Used by TextureModifier Graphics.Blit() to make texture into grayscale.
Shader "GraphicsBlit/TextureModifierMakeGrayscale"
{
    Properties
    {
        _Texture ("Texture RGB", 2D) = "white" {}
        _Multiply("Multiply", Float) = 1
        _Invert ("Invert", Float) = 1
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

            sampler2D _Texture;
            float _Multiply;
            float _Invert;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_Texture, i.uv);
                float c = (col.r+col.g+col.b)/3;
                c = clamp(_Invert * -1, 0, 1) + (c * _Invert);
                col.r = c;
                col.g = c;
                col.b = c;
                col.a = 1;
                return col;
            }
            ENDCG
        }
    }
}
