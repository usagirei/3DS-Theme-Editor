sampler2D Input : register(s0);

sampler2D Overlay : register(s1);

float Scale : register(c0);

float Pinch : register(c1);

float Blend : register(c2);


float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 outputColor;

	float dx = 1 - sin(uv.x * 3.14) * Pinch;
	float dy = uv.y - 0.5;
	float dydx = dy / dx;
	float2 warp = float2(uv.x, (0.5 + (dydx) / Scale));

	float3 inputColor = tex2D(Input, uv).xyz;
	float3 overlayColor = tex2D(Overlay, warp).xyz * Blend;
	
	return float4(inputColor + overlayColor, 1);
}