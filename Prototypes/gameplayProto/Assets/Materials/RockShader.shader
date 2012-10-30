// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "RockShader" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _Shininess ("Shininess", Range (0.03, 1)) = 0.078125
    _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
    _BumpMap ("Bumpmap (RGB)", 2D) = "bump" {}
    _SpecMap ("Spec map (RGB)", 2D) = "white" {}
}

Category {
    /* Upgrade NOTE: commented out, possibly part of old style per-pixel lighting: Blend AppSrcAdd AppDstAdd */
    Fog { Color [_AddFog] }
    Tags { "RenderType"="Opaque" }
    
    #warning Upgrade NOTE: SubShader commented out; uses Unity 2.x per-pixel lighting. You should rewrite shader into a Surface Shader.
/*SubShader { 
        // Ambient pass
        Pass {
            Name "BASE"
            Tags {"LightMode" = "Always" /* Upgrade NOTE: changed from PixelOrNone to Always */}
            /* Upgrade NOTE: commented out, possibly part of old style per-pixel lighting: Blend AppSrcAdd AppDstAdd */
            Color [_PPLAmbient]
            SetTexture [_MainTex] {constantColor [_Color] Combine texture * primary DOUBLE, texture * constant}
        }
        
        // Vertex lights
        Pass {
            Name "BASE"
            Tags {"LightMode" = "Vertex"}
            Lighting On
            Material {
                Diffuse [_Color]
                Emission [_PPLAmbient]
            }

CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
#pragma exclude_renderers gles
#pragma fragment frag
#pragma fragmentoption ARB_fog_exp2
#pragma fragmentoption ARB_precision_hint_fastest

#include "UnityCG.cginc"

uniform sampler2D _MainTex;

half4 frag (v2f_vertex_lit i) : COLOR {
    return VertexLight( i, _MainTex );
} 
ENDCG
        }
        
        // Pixel lights
        Pass { 
            Name "PPL"    
            Tags { "LightMode" = "Pixel" }
CGPROGRAM
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members uvK,uv2,viewDirT,lightDirT)
#pragma exclude_renderers xbox360
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_builtin
#pragma fragmentoption ARB_fog_exp2
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"
#include "AutoLight.cginc" 

struct v2f {
    float4 pos : SV_POSITION;
    LIGHTING_COORDS
    float3    uvK; // xy = UV, z = specular K
    float4    uv2; // bumpmap UV, specmap UV
    float3    viewDirT;
    float3    lightDirT;
}; 

uniform float4 _MainTex_ST, _BumpMap_ST, _SpecMap_ST;
uniform float _Shininess;


v2f vert (appdata_tan v)
{    
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    o.uvK.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
    o.uvK.z = _Shininess * 128;
    o.uv2.xy = TRANSFORM_TEX(v.texcoord, _BumpMap);
    o.uv2.zw = TRANSFORM_TEX(v.texcoord, _SpecMap);

    TANGENT_SPACE_ROTATION;
    o.lightDirT = mul( rotation, ObjSpaceLightDir( v.vertex ) );    
    o.viewDirT = mul( rotation, ObjSpaceViewDir( v.vertex ) );    

    TRANSFER_VERTEX_TO_FRAGMENT(o);    
    return o;
}

uniform sampler2D _BumpMap;
uniform sampler2D _MainTex;
uniform sampler2D _SpecMap;
uniform float4 _LightColor0;

// Calculates Blinn-Phong (specular) lighting model
inline half4 SpecularColorLight( half3 lightDir, half3 viewDir, half3 normal, half4 color, half4 specColor, float specK, half atten )
{
    #ifndef USING_DIRECTIONAL_LIGHT
    lightDir = normalize(lightDir);
    #endif
    viewDir = normalize(viewDir);
    half3 h = normalize( lightDir + viewDir );
    
    half diffuse = dot( normal, lightDir );
    
    float nh = saturate( dot( h, normal ) );
    float spec = pow( nh, specK ) * color.a;
    
    half4 c;
    c.rgb = (color.rgb * _ModelLightColor0.rgb * diffuse + _LightColor0.rgb * specColor.rgb * spec) * (atten * 2);
    c.a = _LightColor0.a * specColor.a * spec * atten; // specular passes by default put highlights to overbright
    return c;
}


half4 frag (v2f i) : COLOR
{        
    half4 texcol = tex2D( _MainTex, i.uvK.xy );
    half4 speccol = tex2D( _SpecMap, i.uv2.zw );
    float3 normal = tex2D(_BumpMap, i.uv2.xy).xyz * 2.0 - 1.0;
    
    half4 c = SpecularColorLight( i.lightDirT, i.viewDirT, normal, texcol, speccol, i.uvK.z, LIGHT_ATTENUATION(i) );
    return c;
}
ENDCG  
        }
    }*/    
}

FallBack "Diffuse"

}