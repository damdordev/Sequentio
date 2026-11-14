Shader "Hidden/Sequentio/MaterialStepTest"
{
    Properties
    {
        [MainColor] _Tint ("Main Color", Color) = (1,1,1,1)
        [MainTexture] _BaseMap ("Main Texture", 2D) = "white" {}
        _CustomColor ("Custom Color", Color) = (1,1,1,1)
        _CustomTexture ("Custom Texture", 2D) = "white" {}
        _Vector ("Vector", Vector) = (0,0,0,0)
        _Float ("Float", Float) = 0
        _Int ("Int", Float) = 0
    }
    SubShader
    {
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ SEQUENTIO_TEST
            float4 vert(float4 vertex : POSITION) : SV_POSITION { return vertex; }
            float4 frag() : SV_Target { return float4(1,1,1,1); }
            ENDHLSL
        }
    }
}
