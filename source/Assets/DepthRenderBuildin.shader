/*
 * The MIT License (MIT)
 * Copyright (c) 2014 Baifang Lu (baifang@gmail.com)
 * 08/20/2014: Initial version.
 */

Shader "Custom/buildin depth shader" {
SubShader {
    Tags { "RenderType"="Opaque" }
    Pass {
        Fog { Mode Off }
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

struct v2f {
    float4 pos : SV_POSITION;
    float2 depth : TEXCOORD0;
};

v2f vert (appdata_base v) {
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.depth.x = COMPUTE_DEPTH_01;
    return o;
}


float4 frag(v2f i) : COLOR {
	return EncodeFloatRGBA(LinearEyeDepth(i.depth.x));
}
ENDCG
    }
}
}