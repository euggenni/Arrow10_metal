// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'

Shader "Rain Image Effect" {
	
	Properties 
    {
		// refraction
        _DropBuffer ("Drop Buffer", 2D) = "bump" {}
		_MainTex("Main Texture", 2D) = "white" {}
	}

   Subshader 
   {
      Fog{Mode off}
   
	  // This pass just clears the buffer to rgb: 128,128,255
	  Pass 
	  {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
    		#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _DropBuffer;
			
			struct appdata_v
			{
				float4	vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 color : COLOR0;
			};

			struct v2f 
			{
				float4	pos : POSITION;
				float2 texCoord : TEXCOORD0;
				float3 color : COLOR0;
			};
			
			v2f vert (appdata_v v)
			{
				v2f o;

				o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
				o.texCoord.x = v.texcoord.x;
				o.texCoord.y = v.texcoord.y;

				return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				float4 color;
				color.r = 0.5f;
				color.g = 0.5f;
				color.b = 1.0f;
				color.a = 1.0f;
				
				return color;
			}

			ENDCG
		}
		
		
      // This pass fades the color to simulate evaporation
	  Pass 
	  {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
    		#pragma target 3.0
			#include "UnityCG.cginc"
			
			sampler2D _DropBuffer;
			
			struct appdata_v
			{
				float4	vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 color : COLOR0;
			};

			struct v2f 
			{
				float4	pos : POSITION;
				float2 texCoord : TEXCOORD0;
				float3 color : COLOR0;
			};
			
			v2f vert (appdata_v v)
			{
				v2f o;

				o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
				
				o.texCoord = v.texcoord;
			
				return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				float4 color = tex2D(_DropBuffer, i.texCoord);
				
				float4 fade;
				fade.r = 0.5f;
				fade.g = 0.5f;
				fade.b = 1.0f;
				fade.a = 1.0f;

				float fadeSpeed = 0.003;
				
				if(color.r-fade.r > 0)
					color.r -= fadeSpeed;
					
				else if(color.r-fade.r < 0)
					color.r +=fadeSpeed;
				
				if(color.g-fade.g > 0)
					color.g -= fadeSpeed;
					
				else if(color.g-fade.g < 0)
					color.g += fadeSpeed;
					
				if(color.b-fade.b > 0)
					color.b -= fadeSpeed;
					
				else if(color.b-fade.b < 0)
					color.b += fadeSpeed;
				
				return color;
			}

			ENDCG
		}
		
	   // This pass uses normals to create refraction and specular shading
	   // on the rain drops
	   Pass
	  {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest 
            #include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _DropBuffer;
			float2 _MainTex_TexelSize;
			
			struct appdata_v
			{
				float4	vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 color : COLOR0;
			};

			struct v2f 
			{
				float4	pos : POSITION;
				float2 texCoord : TEXCOORD0;
				float3 color : COLOR0;
			};
			
			v2f vert (appdata_v v)
			{
				v2f o;

				o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
				o.texCoord = v.texcoord;

				return o;
			}

            float4 frag (v2f i) : COLOR
            {
				#if SHADER_API_D3D9
					if (_MainTex_TexelSize.y < 0)
							 i.texCoord.y = 1 -  i.texCoord.y;
				#endif 
				
               float3 refractNormal = tex2D(_DropBuffer, i.texCoord).rgb * 2.0f - 1.0f;
			   
			   #if SHADER_API_D3D9
					if (_MainTex_TexelSize.y < 0)
							 i.texCoord.y = 1 -  i.texCoord.y;
				#endif 
			   
			   float2 newTexCoords = i.texCoord + refractNormal.rg * 0.05;
				
			   newTexCoords.x = min(1.0f, max(0.0, newTexCoords.x));
			   newTexCoords.y = min(1.0f, max(0.0, newTexCoords.y));
			   
			   float3 lightDir = normalize(float3(1.0f, -1.0f, 0.0f));
			   float3 R = normalize(reflect(lightDir, refractNormal));
			   float specular = 0.0f;
			   
			   if(dot(lightDir, refractNormal) > 0.0)
					specular = pow(max(0.0f, R.z), 64.0) * 0.1f;
			   
			   float4 color;
				
			   color.rgba = tex2D(_MainTex, newTexCoords).rgba + specular;
				
			   return color;
            }
            ENDCG
	   }
	}
}