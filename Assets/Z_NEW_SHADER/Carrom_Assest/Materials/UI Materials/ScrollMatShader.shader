Shader "Leo Shaders/ScrollMatShader"
{
    Properties
    {
        [NoScaleOffset]_MainTex("Main Texture", 2D) = "white" {}
        [NoScaleOffset]_MaskTex("Mask Texture", 2D) = "white" {}
        _Tiling("Tiling", Vector) = (1, 0.2, 0, 0)
        _Offset("Offset", Vector) = (0, 0, 0, 0)
        [HideInInspector]_BUILTIN_QueueOffset("Float", Float) = 0
        [HideInInspector]_BUILTIN_QueueControl("Float", Float) = -1
    }
    SubShader
    {
        Tags
        {
            // RenderPipeline: <None>
            "RenderType"="Transparent"
            "BuiltInMaterialType" = "Unlit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="BuiltInUnlitSubTarget"
        }
        Pass
        {
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        ColorMask RGB
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile_fwdbase
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define BUILTIN_TARGET_API 1
        #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _MaskTex_TexelSize;
        float2 _Tiling;
        float2 _Offset;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MaskTex);
        SAMPLER(sampler_MaskTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 Color_d3efff30c1ca4bd093813983915a1108 = IsGammaSpace() ? float4(0, 0, 0, 0) : float4(SRGBToLinear(float3(0, 0, 0)), 0);
            UnityTexture2D _Property_682d2a0aa23243c6bc63f7718f623b86_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_8723080fd1664f938f015fb056016351_Out_0 = _Tiling;
            float2 _Property_ad267e1a6c4247b88dde66a3d9ff7947_Out_0 = _Offset;
            float2 _TilingAndOffset_a7a5379278274c22b74590a84bbee4c5_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_8723080fd1664f938f015fb056016351_Out_0, _Property_ad267e1a6c4247b88dde66a3d9ff7947_Out_0, _TilingAndOffset_a7a5379278274c22b74590a84bbee4c5_Out_3);
            float4 _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0 = SAMPLE_TEXTURE2D(_Property_682d2a0aa23243c6bc63f7718f623b86_Out_0.tex, _Property_682d2a0aa23243c6bc63f7718f623b86_Out_0.samplerstate, _Property_682d2a0aa23243c6bc63f7718f623b86_Out_0.GetTransformedUV(_TilingAndOffset_a7a5379278274c22b74590a84bbee4c5_Out_3) );
            float _SampleTexture2D_b8e64565920443dcac662d27d0944232_R_4 = _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0.r;
            float _SampleTexture2D_b8e64565920443dcac662d27d0944232_G_5 = _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0.g;
            float _SampleTexture2D_b8e64565920443dcac662d27d0944232_B_6 = _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0.b;
            float _SampleTexture2D_b8e64565920443dcac662d27d0944232_A_7 = _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0.a;
            UnityTexture2D _Property_dfb4efc0ef484578b299eb21a4edf875_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTex);
            float4 _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0 = SAMPLE_TEXTURE2D(_Property_dfb4efc0ef484578b299eb21a4edf875_Out_0.tex, _Property_dfb4efc0ef484578b299eb21a4edf875_Out_0.samplerstate, _Property_dfb4efc0ef484578b299eb21a4edf875_Out_0.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_R_4 = _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0.r;
            float _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_G_5 = _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0.g;
            float _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_B_6 = _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0.b;
            float _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_A_7 = _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0.a;
            float4 _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3;
            Unity_Lerp_float4(Color_d3efff30c1ca4bd093813983915a1108, _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0, (_SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_A_7.xxxx), _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3);
            float _Split_a2f986dea1bb4a63a5219397d9d6c86b_R_1 = _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3[0];
            float _Split_a2f986dea1bb4a63a5219397d9d6c86b_G_2 = _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3[1];
            float _Split_a2f986dea1bb4a63a5219397d9d6c86b_B_3 = _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3[2];
            float _Split_a2f986dea1bb4a63a5219397d9d6c86b_A_4 = _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3[3];
            surface.BaseColor = (_Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3.xyz);
            surface.Alpha = _Split_a2f986dea1bb4a63a5219397d9d6c86b_A_4;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if UNITY_SHOULD_SAMPLE_SH
            #if !defined(LIGHTMAP_ON)
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if UNITY_SHOULD_SAMPLE_SH
            #if !defined(LIGHTMAP_ON)
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_shadowcaster
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define BUILTIN_TARGET_API 1
        #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _MaskTex_TexelSize;
        float2 _Tiling;
        float2 _Offset;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_MaskTex);
        SAMPLER(sampler_MaskTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 Color_d3efff30c1ca4bd093813983915a1108 = IsGammaSpace() ? float4(0, 0, 0, 0) : float4(SRGBToLinear(float3(0, 0, 0)), 0);
            UnityTexture2D _Property_682d2a0aa23243c6bc63f7718f623b86_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float2 _Property_8723080fd1664f938f015fb056016351_Out_0 = _Tiling;
            float2 _Property_ad267e1a6c4247b88dde66a3d9ff7947_Out_0 = _Offset;
            float2 _TilingAndOffset_a7a5379278274c22b74590a84bbee4c5_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_8723080fd1664f938f015fb056016351_Out_0, _Property_ad267e1a6c4247b88dde66a3d9ff7947_Out_0, _TilingAndOffset_a7a5379278274c22b74590a84bbee4c5_Out_3);
            float4 _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0 = SAMPLE_TEXTURE2D(_Property_682d2a0aa23243c6bc63f7718f623b86_Out_0.tex, _Property_682d2a0aa23243c6bc63f7718f623b86_Out_0.samplerstate, _Property_682d2a0aa23243c6bc63f7718f623b86_Out_0.GetTransformedUV(_TilingAndOffset_a7a5379278274c22b74590a84bbee4c5_Out_3) );
            float _SampleTexture2D_b8e64565920443dcac662d27d0944232_R_4 = _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0.r;
            float _SampleTexture2D_b8e64565920443dcac662d27d0944232_G_5 = _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0.g;
            float _SampleTexture2D_b8e64565920443dcac662d27d0944232_B_6 = _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0.b;
            float _SampleTexture2D_b8e64565920443dcac662d27d0944232_A_7 = _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0.a;
            UnityTexture2D _Property_dfb4efc0ef484578b299eb21a4edf875_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTex);
            float4 _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0 = SAMPLE_TEXTURE2D(_Property_dfb4efc0ef484578b299eb21a4edf875_Out_0.tex, _Property_dfb4efc0ef484578b299eb21a4edf875_Out_0.samplerstate, _Property_dfb4efc0ef484578b299eb21a4edf875_Out_0.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_R_4 = _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0.r;
            float _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_G_5 = _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0.g;
            float _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_B_6 = _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0.b;
            float _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_A_7 = _SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_RGBA_0.a;
            float4 _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3;
            Unity_Lerp_float4(Color_d3efff30c1ca4bd093813983915a1108, _SampleTexture2D_b8e64565920443dcac662d27d0944232_RGBA_0, (_SampleTexture2D_69b61bc0c9564d099289ab111af19cb4_A_7.xxxx), _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3);
            float _Split_a2f986dea1bb4a63a5219397d9d6c86b_R_1 = _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3[0];
            float _Split_a2f986dea1bb4a63a5219397d9d6c86b_G_2 = _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3[1];
            float _Split_a2f986dea1bb4a63a5219397d9d6c86b_B_3 = _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3[2];
            float _Split_a2f986dea1bb4a63a5219397d9d6c86b_A_4 = _Lerp_a6557cf3173443d6a3d6432a17bd09ad_Out_3[3];
            surface.Alpha = _Split_a2f986dea1bb4a63a5219397d9d6c86b_A_4;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if UNITY_SHOULD_SAMPLE_SH
            #if !defined(LIGHTMAP_ON)
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if UNITY_SHOULD_SAMPLE_SH
            #if !defined(LIGHTMAP_ON)
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "UnityEditor.Rendering.BuiltIn.ShaderGraph.BuiltInUnlitGUI" ""
    FallBack "Hidden/Shader Graph/FallbackError"
}