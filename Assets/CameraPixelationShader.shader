Shader "Custom/CameraPixelation"
{
    Properties
    {
        _MainTex("Render Texture", 2D) = "white" {}
        _PixelDensity("Pixel Density", Range(1, 100)) = 20
        _MaxDistance("Max Distance", Float) = 20.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float _PixelDensity;
                float _MaxDistance;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float3 screenPos : TEXCOORD1;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.screenPos = ComputeScreenPos(o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Sample the original render texture
                    float2 uv = i.uv;

                    // Calculate dynamic pixel size based on distance from the center of the screen
                    float centerDistance = length(i.uv - float2(0.5, 0.5));
                    float pixelSize = lerp(_PixelDensity, 1.0, saturate(centerDistance / _MaxDistance));

                    // Apply pixelation effect by snapping UV coordinates
                    float2 pixelatedUV = floor(uv * pixelSize) / pixelSize;

                    // Sample the texture with pixelated UV
                    fixed4 col = tex2D(_MainTex, pixelatedUV);

                    return col;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
