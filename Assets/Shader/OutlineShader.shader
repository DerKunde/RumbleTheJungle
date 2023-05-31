Shader "Custom/OutlineShader" {
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0.002, 0.1)) = 0.002
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100
 
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineWidth;
 
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target {
                // Sample the sprite texture
                fixed4 col = tex2D(_MainTex, i.uv);
 
                // Calculate the outline by sampling the texture multiple times and adding the colors together
                fixed4 outline = fixed4(0, 0, 0, 0);
                outline += tex2D(_MainTex, i.uv + float2(-_OutlineWidth, 0));
                outline += tex2D(_MainTex, i.uv + float2(_OutlineWidth, 0));
                outline += tex2D(_MainTex, i.uv + float2(0, -_OutlineWidth));
                outline += tex2D(_MainTex, i.uv + float2(0, _OutlineWidth));
                outline *= _OutlineColor;
 
                // Add the outline to the sprite color and return
                col += outline;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}