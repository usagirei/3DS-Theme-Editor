sampler2D Input : register(s0);
sampler2D MovingTexture : register(s1);
sampler2D FixedTexture : register(s2);

float Enable: register(c0);
/// <defaultValue>0</defaultValue>
float GrayLevel : register(c1);
/// <defaultValue>3.5</defaultValue>
float Times : register(c2);
/// <defaultValue>0.15</defaultValue>
float Pinch : register(c3);
/// <defaultValue>0.5</defaultValue>
float Offset : register(c4);
/// <defaultValue>1.667</defaultValue>
float Aspect : register(c5);
/// <defaultValue>1</defaultValue>
float Gradient : register(c6);
/// <defaultValue>1</defaultValue>
float Opacity : register(c7);
/// <defaultValue>0.5</defaultValue>
float FixedOpacity : register(c8);


float4 main(float2 uv : TEXCOORD) : COLOR
{
	float3 baseCol = tex2D(Input, uv).xyz;
	float3 outCol;

	if (Enable > 0.5) {
		float4 outputColor;

		float halfX = 0.5 - uv.x;
		float halfY = uv.y - 0.5;

		float yMod = halfY / (1 - (sin(uv.x * 3.14) * Pinch));
		float xMod = 1 - ((abs(halfX)) * Pinch);

		float2 warp = float2(100 + (xMod * halfX * Aspect), 100 + (0.5 + yMod)) * (Times);
		float2 warpFixed = warp + float2(0.5, -Pinch);
		float2 warpMoving = warpFixed + float2(Offset, -Offset);

		warpMoving = fmod(warpMoving, 1);
		warpFixed = fmod(warpFixed, 1);

		float2 moving = warpMoving;
		float2 fixed = warpFixed;

		float overlayMoving = tex2D(MovingTexture, moving).x;
		float overlayFixed = tex2D(FixedTexture, fixed).x;

		float overlay = saturate(overlayMoving + overlayFixed*FixedOpacity) * Opacity;

		float rate = (1 - uv.y);
		float3 backCol = lerp(baseCol, 1.0 * GrayLevel, 0.0 + rate * Gradient);
		float3 foreCol = lerp(baseCol, 1.2 * GrayLevel, 0.5 + rate * Gradient);

		outCol = lerp(backCol, foreCol, overlay.x);
	}
	else {
		outCol = baseCol;
	}

	return float4(outCol,1);
}