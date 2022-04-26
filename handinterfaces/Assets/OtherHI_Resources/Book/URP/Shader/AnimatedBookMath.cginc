#ifndef ANIMATED_BOOK_INCLUDED
#define ANIMATED_BOOK_INCLUDED

#ifndef UNITY_PI
#define UNITY_PI 3.14159265359
#endif


float4x4 GetRotationZMatrix(float angles){
	//https://open.gl/transformations

	angles *= UNITY_PI/180;

	return float4x4(
		cos(angles),-sin(angles),0,0,
		sin(angles),cos(angles),0,0,
		0,0,1,0,
		0,0,0,1);
}

half ClampSign(half value){
	return clamp(sign(value),0,1);
}
 
half Mod01(half value){
	return value - floor(value);
}

void CalculateVertices_float(
	//Property inputs
	float _Pages, float _ProgressThreshold, float _Progress, float _FlipPageOffset, float _Rows, float _Columns, float _Open, float _WaveLength, float _WaveHeight, float _WaveAntecipation,
	//Vertex inputs
	float3 in_vertex, float4 in_color, float4 in_texcoord1, float4 in_normal, float4 in_tangent,
	//Outputs
	out float3 out_vertex, out float4 out_normal, out float4 out_tangent, out float3 data) { 
//void CalculateVertices (inout appdata_full v, out float4 data){
	_Progress = clamp(_Progress,0,_Pages - 0.001);
	_Progress = floor(_Progress) + clamp( Mod01(_Progress) * (1 + _ProgressThreshold * 2) - _ProgressThreshold, 0, 1);
	///Flip pages
	half _Flip = Mod01(_Progress);

	half rotateAngles = 0;
				
	float3 vert = in_vertex;

	float3 defaultVert = in_vertex; 
			
	float4 color = in_color;

	float i = 1 - _Flip;
			
	half a = _Flip * _Flip * _Flip;
	half b = 3 * _Flip * i * i + 3 * _Flip * _Flip * i + a;
				
	half flipLerp = lerp(a,b,(1 + in_vertex.z * _WaveAntecipation) * 0.5);
			
	half pageRotateAmmount = 180 * flipLerp;

	half y = vert.y;
	vert.y = 0;
	vert = mul(GetRotationZMatrix(pageRotateAmmount), vert);
	vert.y *= _Open;
				
	rotateAngles += color.r * pageRotateAmmount;
				
	_WaveHeight *= _Open;

	half waveVal = sin(abs(in_vertex.x) * 2 * UNITY_PI / _WaveLength) * _WaveHeight * color.b;
				
	rotateAngles -= cos(abs(in_vertex.x) * 2 * UNITY_PI / _WaveLength) * 360 * color.b * _WaveHeight;
				
	defaultVert.y += waveVal;

	vert.y += y + waveVal + _FlipPageOffset * color.r;

	out_vertex = lerp(defaultVert, vert, color.r);

	float2 uv2 = in_texcoord1;
	uv2.y /= _Rows;
	uv2.y = 1 - uv2.y;
	uv2.x /= _Columns * 0.5;
			

	uv2.x += (1 / _Columns)*(floor(_Progress) % _Columns);
	uv2.y -= (floor( uv2.x ) / _Rows) + floor(_Progress / _Columns) * 1 / _Rows;

	data = float3(uv2, color.b);
				
	float4x4 rotateMatrix = GetRotationZMatrix(rotateAngles);

	out_normal = mul(rotateMatrix, in_normal);
	out_tangent = mul(rotateMatrix, in_tangent);
}

//half _AlphaBlend

void CalculateColors_float(
	//Properties
	float _AlphaBlend,
	//Input values
	float4 color, float4 paperColor, float isPaper, 
	//Outputs
	out float4 out_color) {
	//data: [xy: page UV  |  z: isPaper]

	out_color = color;

	half lerpValue = isPaper;

	#ifdef _IMAGEMODE_BLACK_AND_WHITE
	lerpValue *= (1 - paperColor.r);
	out_color.rgb = lerp(color.rgb, float3(0,0,0) , lerpValue);
	#endif
				
	#ifdef _IMAGEMODE_ALPHA_BLEND
	lerpValue *= paperColor.a;
	out_color.rgb = lerp(color.rgb, paperColor.rgb, lerpValue);
	#endif
				
	#ifdef _IMAGEMODE_RGB_INDEPENDENT
	_ColorR.a = lerp(_ColorR.a, ClampSign((1 - _ColorR.a) - paperColor.r * paperColor.a), _AlphaBlend);
	_ColorG.a = lerp(_ColorG.a, ClampSign((1 - _ColorG.a) - paperColor.g * paperColor.a), _AlphaBlend);
	_ColorB.a = lerp(_ColorB.a, ClampSign((1 - _ColorB.a) - paperColor.b * paperColor.a), _AlphaBlend);
	
	paperColor.r = lerp(paperColor.r, _ColorR.a, _AlphaBlend * ClampSign(paperColor.r - 0.02));
	paperColor.g = lerp(paperColor.g, _ColorG.a, _AlphaBlend * ClampSign(paperColor.g - 0.02));
	paperColor.b = lerp(paperColor.b, _ColorB.a, _AlphaBlend * ClampSign(paperColor.b - 0.02));

	half4 pageCol = _ColorR * paperColor.r * _ColorR.a;
	pageCol += _ColorG * paperColor.g * _ColorG.a;
	pageCol += _ColorB * paperColor.b * _ColorB.a;

	lerpValue *= paperColor.a * pageCol.a;

	out_color.rgb = lerp(lerp(color.rgb, _ColorA.rgb, _ColorA.a * isPaper), pageCol.rgb, lerpValue);
	#endif
	//col.rgb = lerp(col.rgb, float3(data.x,data.y,0), 0.85);

	//color.a = lerpValue;

	//out_color = color;// paperColor * isPaper;
}




#endif // ANIMATED_BOOK_INCLUDED