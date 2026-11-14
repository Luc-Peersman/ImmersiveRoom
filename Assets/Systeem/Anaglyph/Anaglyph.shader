// Voegt het beeld van het linker- en rechteroog samen tot één anaglyph (3D met een rood/blauw- of rood/cyaan-brilletje).
// _MainTex = linkeroog (rood glas), _RechtsTex = rechteroog (blauw of cyaan glas).
Shader "Systeem/Anaglyph"
{
    Properties
    {
        _MainTex ("Linkeroog", 2D) = "black" {}
        _RechtsTex ("Rechteroog", 2D) = "black" {}
        _Modus ("Bril: 0 rood/blauw, 1 rood/cyaan, 2 rood/cyaan Dubois", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _RechtsTex;
            float _Modus;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 L = tex2D(_MainTex, i.uv).rgb;
                float3 R = tex2D(_RechtsTex, i.uv).rgb;
                float3 grijs = float3(0.299, 0.587, 0.114);
                float3 kleur;

                if (_Modus < 0.5)
                {
                    // Rood/blauw: links helderheid in rood, rechts helderheid in blauw
                    kleur = float3(dot(L, grijs), 0, dot(R, grijs));
                }
                else if (_Modus < 1.5)
                {
                    // Rood/cyaan (kleur): rood van links, groen en blauw van rechts
                    kleur = float3(L.r, R.g, R.b);
                }
                else
                {
                    // Rood/cyaan volgens Dubois (minder spookbeelden)
                    float3x3 ml = float3x3( 0.437,  0.449,  0.164,
                                           -0.062, -0.062, -0.024,
                                           -0.048, -0.050, -0.017);
                    float3x3 mr = float3x3(-0.011, -0.032, -0.007,
                                            0.377,  0.761,  0.009,
                                           -0.026, -0.093,  1.234);
                    kleur = saturate(mul(ml, L) + mul(mr, R));
                }
                return fixed4(kleur, 1);
            }
            ENDCG
        }
    }
}
