/*
 * The MIT License (MIT)
 * Copyright (c) 2014 Baifang Lu (baifang@gmail.com)
 * 08/20/2014: Initial version.
 */

Shader "depth based compositor" {
    Properties {
        _MainTex ("", 2D) = "" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    struct v2f {
        float4 pos : POSITION;
        float2 uv : TEXCOORD0;
    };

    sampler2D _MainTex;
    sampler2D maskDepthTexture;
    sampler2D sceneDepthTexture;

    v2f vert( appdata_img v ) 
    {
        v2f o;
        o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
        o.uv = v.texcoord.xy;
        return o;
    }

    float4 depthMask(v2f pixelData) : COLOR0
    {
        float maskDepth = DecodeFloatRGBA(tex2D(maskDepthTexture, pixelData.uv));
        float sceneDepth = DecodeFloatRGBA(tex2D(sceneDepthTexture, pixelData.uv));
        float4 color = tex2D(_MainTex, pixelData.uv);
        if (sceneDepth < maskDepth) 
            return color;
        else
            return float4(0, 1, 0, 1);
    }
    ENDCG 


Subshader {
 Pass {
      ZTest Always Cull Off ZWrite Off
      Fog { Mode off }

      CGPROGRAM
      #pragma glsl
      #pragma fragmentoption ARB_precision_hint_fastest 
      #pragma vertex vert
      #pragma fragment depthMask
      #pragma target 3.0
      ENDCG
  }
}

 

Fallback off
}