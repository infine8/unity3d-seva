Shader "Custom/NewShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ForSeconds ("ForSeconds", float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			//clip (frac((IN.worldPos.y+IN.worldPos.z*0.1) * 5) - 0.5);
			clip(c.r - 1);
			o.Albedo = c.rgb;
			//o.Albedo = o.Albedo * 2/ c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
