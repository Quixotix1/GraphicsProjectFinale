# GraphicsProjectFinale

Planning Document: https://docs.google.com/document/d/1j1Vlys8K8MM30H_L5y3Hyf9bGk3sDZiXhChqciQ19A0/edit?usp=sharing

## Reference Information
Skybox: https://assetstore.unity.com/packages/2d/textures-materials/sky/city-street-skyboxes-vol-1-157401
Textures created using Adobe Substance 3D Painter

## Effects and Shader Information

### Texture Toggle

Textures can easily be toggled using the 'R' or 'space' keys. Toggling the textures makes all of them use a default lit shader with no materials, making it easy to see the models themselves in the scene.

#### Before
<img width="2559" height="1599" alt="image" src="https://github.com/user-attachments/assets/8f11b7d5-8e02-4c18-94c2-b77268769244" />

#### After

<img width="2559" height="1599" alt="image" src="https://github.com/user-attachments/assets/028765b7-8610-4eb5-a7ed-e4a67d0dfd02" />

#### Script

<img width="953" height="904" alt="image" src="https://github.com/user-attachments/assets/02fbd10f-cc34-4acc-b5af-ef18191614f9" />

### Retro Shader

There is a retro shader that toggles the resolution and color channels of the screen using the [F] key. The retro shader is a render texture of size 384x216 with a point filter and R8G8B8A8_UNORM color format.

<img width="2559" height="1599" alt="image" src="https://github.com/user-attachments/assets/a92b8309-4cfd-4905-8ef4-3e8109e648f1" />

The shader is applied as follows:

<img width="970" height="159" alt="image" src="https://github.com/user-attachments/assets/ec6efa88-cd58-4474-942b-4f2f61cdb4be" />

The above two functions are contained within the update function of ChangeResolution, found in Assets/Scripts/Utility/ChangeResolution.cs

### Keyboard Key

The floating 'E' key appears when standing in front of an interactable object (i.e a door or staircase) and disappears when leaving. It uses a bump shader to emulate the slight roughness of older keyboard keys and to make it look more realistic. 

<img width="2559" height="1599" alt="image" src="https://github.com/user-attachments/assets/e5488e12-7fc1-4b3a-8169-a74db70e241d" />

Update function for floating object:

void Update()
{
    yPos = (m_MinY - m_MaxY) / 2f * Mathf.Cos(Mathf.PI * (Time.time * m_Speed)) + (m_MinY + m_MaxY) / 2f;
    transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
}

### Lock Object

The lock appears when trying to access a locked door with the 'E' key without a key. It floats in the air briefly before disappearing. The script to do this is contained in Movement.cs, Interact.cs, and Inventory.cs.

<img width="2559" height="1599" alt="image" src="https://github.com/user-attachments/assets/35f41697-f65c-41a6-a6c1-0c34e959b608" />

### Window Shader

The following is a simple shader used by the window to make the non-metallic components transparent. It is done this way since the bars crossing the windows are metallic and should be opaque.

<img width="1470" height="769" alt="image" src="https://github.com/user-attachments/assets/ace3f842-b4d6-454f-af5e-eb6225c9ae32" />

### Silhouette Shader

The following is a flat silhouette shader used by the quest marker to draw over walls while not being affected by light sources. This makes it clear and easy to see from anywhere in the map.

<img width="1128" height="645" alt="image" src="https://github.com/user-attachments/assets/dd31b5eb-c0f3-4020-ace0-cd1fa5627372" />

### Bump Shader

The following shader uses a normal map and a factor to generate slight inconsistencies in how light reflects off the object, giving it a slightly more realistic look. It is used by the lockers, desk, and floating keyboard key.

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
