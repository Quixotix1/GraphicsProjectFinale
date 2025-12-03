Shader "Custom/Bump"
{
    Properties
    {
        _baseMap ("Base Map", 2D) = "white" {}
        _normalMap ("Normal Map", 2D) = "bump" {}
        _normalFactor ("Normal Factor", Range(0,10)) = 1
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD1;
                float3 tangentWS : TEXCOORD2;
                float2 uv : TEXCOORD0;
                float3 bitangentWS : TEXCOORD3;
                float3 viewDirWS : TEXCOORD4;
            };

            TEXTURE2D(_baseMap);
            SAMPLER(sampler_baseMap);

            TEXTURE2D(_normalMap);
            SAMPLER(sampler_normalMap);

            CBUFFER_START(UnityPerMaterial)
                float _normalFactor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.tangentWS = normalize(TransformObjectToWorldNormal(IN.tangentOS.xyz));
                OUT.bitangentWS = cross(OUT.normalWS, OUT.tangentWS) * IN.tangentOS.w;
                float3 worldPosWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.viewDirWS = normalize(GetCameraPositionWS() - worldPosWS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 albedo = SAMPLE_TEXTURE2D(_baseMap, sampler_baseMap, IN.uv);
                half3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_normalMap, sampler_normalMap, IN.uv)) * _normalFactor;
                half3x3 TBN = half3x3(IN.tangentWS, IN.bitangentWS, IN.normalWS);
                half3 normalWS = normalize(mul(normalTS, TBN));
                half3 lightDirWS = normalize(GetMainLight().direction);
                half NdotL = saturate(dot(normalWS, lightDirWS));
                half3 diffuse = albedo.rgb * NdotL;

                half3 ambientSH = SampleSH(normalWS);
                half3 ambient = albedo.rgb * ambientSH;
                return half4(diffuse + ambient, albedo.a);
            }

            ENDHLSL
        }
    }
}
