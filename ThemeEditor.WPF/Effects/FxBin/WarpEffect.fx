Texture2D Sampler : register(S0);

float Scale : register(C0);

float Pinch : register(C1);

SamplerState WrapSampler;

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 outputColor;

	float dx = 1 - sin(uv.x * 3.14) * Pinch;
	float dy = uv.y - 0.5;
	float dydx = dy / dx;
	float2 warp = float2(uv.x, (0.5 + (dydx) / Scale));

	//outputColor = tex2D(Sampler, warp);
	outputColor = Sampler.Sample(WrapSampler, warp);
	
	return outputColor;
}