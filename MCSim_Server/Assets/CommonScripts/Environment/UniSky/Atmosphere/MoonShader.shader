Shader "Moon Shader" 
{
	Properties
   {
      _MainTexture ("Base (RGB)", RECT) = "white" {}
	  _v4LightDir("Sun Direction", Vector) = (0,0,0)
   }
   
   SubShader
   {
	Tags { "RenderType"="Opaque" "Queue" = "Geometry+1"}
	
	 Cull Off 
	 Lighting Off 
	 Fog { Mode Off }
	
     Pass
      {
		 Blend SrcAlpha OneMinusSrcAlpha

         CGPROGRAM

         #pragma multi_compile_builtin
		 #pragma vertex vert
         #pragma fragment frag
         #include "UnityCG.cginc"
			
		sampler2D _MainTexture;	
		float3 _v4LightDir;
		
		struct v2f 
		 {
			float4	pos : POSITION;
			float2 texCoord : TEXCOORD0;
			float2 depth : TEXCOORD1;
			float color : COLOR;
		};
		 
		
		 v2f vert (appdata_base v)
		{
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			o.texCoord = v.texcoord;

			return o;
		}

		 half4 frag(v2f v) : COLOR0
         {
			float4 color;
			color = tex2D(_MainTexture, v.texCoord);
			
			color.a = lerp(0, color.a, abs(_v4LightDir.y-0.3));

			return color;
         }
		 
         ENDCG
      } 
	}
}

