#ifndef ANIMATED_BOOK_INCLUDED
#define ANIMATED_BOOK_INCLUDE


half _Pages, _ProgressThreshold;
float _Progress, _FlipPageOffset;
half _Rows, _Columns;
half _Open, _AlphaBlend;
half _WaveLength, _WaveHeight, _WaveAntecipation;

half4 _ColorR, _ColorG, _ColorB,_ColorA;

sampler2D _PaperContent;

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

void CalculateVertices (inout appdata_full v, out float4 data){
	_Progress = clamp(_Progress,0,_Pages - 0.001);
	_Progress = floor(_Progress) + clamp( Mod01(_Progress) * (1 + _ProgressThreshold * 2) - _ProgressThreshold, 0, 1);
			
	///Flip pages
	half _Flip = Mod01(_Progress);
				
	half rotateAngles = 0;

				
	float4 vert = v.vertex;

	float4 defaultVert = v.vertex;
			
	float4 color = v.color;

	float i = 1 - _Flip;
				
			
	half a = _Flip * _Flip * _Flip;
	half b = 3 * _Flip * i * i + 3 * _Flip * _Flip * i + a;
				
	half flipLerp = lerp(a,b,(1 + v.vertex.z * _WaveAntecipation) * 0.5);
			
	half pageRotateAmmount = 180 * flipLerp;

	half y = vert.y;
	vert.y = 0;
	vert = mul(GetRotationZMatrix(pageRotateAmmount), vert);
	vert.y *= _Open;
				
	rotateAngles += color.r * pageRotateAmmount;
				
	_WaveHeight *= _Open;

	half waveVal = sin(abs(v.vertex.x) * 2 * UNITY_PI / _WaveLength) * _WaveHeight * color.b;
				
	rotateAngles -= cos(abs(v.vertex.x) * 2 * UNITY_PI / _WaveLength) * 360 * color.b * _WaveHeight;
				
	defaultVert.y += waveVal;

	vert.y += y + waveVal + _FlipPageOffset * color.r;

	v.vertex = lerp(defaultVert, vert, color.r);

	float2 uv2 = v.texcoord1;
	uv2.y /= _Rows;
	uv2.y = 1 - uv2.y;
	uv2.x /= _Columns * 0.5;
			

	uv2.x += (1 / _Columns)*(floor(_Progress) % _Columns);
	uv2.y -= (floor( uv2.x ) / _Rows) + floor(_Progress / _Columns) * 1 / _Rows;

	data = float4(uv2, color.b, 0);
				
	float4x4 rotateMatrix = GetRotationZMatrix(rotateAngles);

	v.normal = mul(rotateMatrix, v.normal);
	v.tangent = mul(rotateMatrix, v.tangent);
}

fixed4 CalculateColors(fixed4 col, float3 data){
	//data: [xy: page UV  |  z: isPaper]


	data.x = Mod01(data.x);
	data.y = Mod01(data.y);

	fixed4 paperCol = tex2D(_PaperContent, data.xy);
	half lerpValue = 0;

	#ifdef _IMAGEMODE_BLACKANDWHITE
	lerpValue = 1 - paperCol.r;
	col.rgb = lerp(lerp(col.rgb, _ColorA.rgb, _ColorA.a * data.z), paperCol.rgb, lerpValue);
	#endif
				
	#ifdef _IMAGEMODE_ALPHABLEND
	col.rgb = lerp(lerp(col.rgb, _ColorA.rgb, _ColorA.a * data.z), paperCol.rgb, paperCol.a);
	lerpValue = paperCol.a;
	#endif
				
	#ifdef _IMAGEMODE_RGB_INDEPENDENT

	

	_ColorR.a = lerp(_ColorR.a, ClampSign((1 - _ColorR.a) - paperCol.r * paperCol.a), _AlphaBlend);
	_ColorG.a = lerp(_ColorG.a, ClampSign((1 - _ColorG.a) - paperCol.g * paperCol.a), _AlphaBlend);
	_ColorB.a = lerp(_ColorB.a, ClampSign((1 - _ColorB.a) - paperCol.b * paperCol.a), _AlphaBlend);
	
	paperCol.r = lerp(paperCol.r, _ColorR.a, _AlphaBlend * ClampSign(paperCol.r - 0.02));
	paperCol.g = lerp(paperCol.g, _ColorG.a, _AlphaBlend * ClampSign(paperCol.g - 0.02));
	paperCol.b = lerp(paperCol.b, _ColorB.a, _AlphaBlend * ClampSign(paperCol.b - 0.02));

	half4 pageCol = _ColorR * paperCol.r * _ColorR.a;
	pageCol += _ColorG * paperCol.g * _ColorG.a;
	pageCol += _ColorB * paperCol.b * _ColorB.a;

	col.rgb = lerp(lerp(col.rgb, _ColorA.rgb, _ColorA.a * data.z), pageCol.rgb, paperCol.a * pageCol.a * data.z);
	

	#endif
	//col.rgb = lerp(col.rgb, float3(data.x,data.y,0), 0.85);

	col.a = lerpValue;

	return col;
}




#endif // ANIMATED_BOOK_INCLUDED