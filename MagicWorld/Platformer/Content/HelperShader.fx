sampler firstSampler;

float4 PS_COLOR(float2 texCoord: TEXCOORD0) : COLOR
{
   float4 color = tex2D(firstSampler, texCoord);   

   color.rgb = dot(color.rgb, float3(0.3f, 0.59f, 0.11f));   
   return color;
} 

technique LangweiligerShader
{
   pass pass0
   {
      PixelShader = compile ps_2_0 PS_COLOR();
   }
} 