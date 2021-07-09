// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NewSurface"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ColorCenter("Couleur centre", Color) = (1,1,1,0.5)
		_ColorBorder("Couleur brdure", Color) = (0.5,0.5,0.5,1)
		_Pos1("Position 1", Vector) = (0,0,0,0)
		_Pos2("Position 2", Vector) = (0,0,0,0)
		_Pos3("Position 3", Vector) = (0,0,0,0)
		_Pos4("Position 4", Vector) = (0,0,0,0)
		_Pos5("Position 5", Vector) = (0,0,0,0)
			
		_Size("Size" , float) = 1
		_TresholdMin("Inner Treshold", Range(0,0.5)) = 0.3
		_TresholdMax("Outer Treshold", Range(0,0.5)) = 0.5

		_BlobMin("Blob min", Range(0,1)) = 0.3
		_BlobBorder("Blob border", Range(0,1)) = 0.35

	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			LOD 100

			Pass
			{
				CGPROGRAM

				#pragma vertex vert alpha
				#pragma fragment frag alpha

				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex  : SV_POSITION;
					half2 uv : TEXCOORD0;
				};


				sampler2D _MainTex;
				float4 _MainTex_ST;

				float4 _ColorCenter;
				float4 _ColorBorder;
				float4 _Pos1;
				float4 _Pos2;
				float4 _Pos3;
				float4 _Pos4;
				float4 _Pos5;

				float _Size;
				float _TresholdMax;
				float _TresholdMin;

				float _BlobMin;
				float _BlobBorder;

				v2f vert(appdata_t v)
				{
					v2f o;

					o.vertex = UnityObjectToClipPos(v.vertex);
					v.texcoord.x = 1 - v.texcoord.x;
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv) * _ColorCenter; // multiply by _Color
				_TresholdMax = _TresholdMax * _Size;
				_TresholdMin = _TresholdMin * _Size;

				float valSum = 0;

				float distSquare1 = ( _Pos1.x - i.uv.x) * (_Pos1.x - i.uv.x ) 
								  + ( _Pos1.y - i.uv.y) * (_Pos1.y - i.uv.y );
				float distSquare2 = ( _Pos2.x - i.uv.x) * (_Pos2.x - i.uv.x )
							      + ( _Pos2.y - i.uv.y) * (_Pos2.y - i.uv.y );
				float distSquare3 = ( _Pos3.x - i.uv.x) * (_Pos3.x - i.uv.x )
							      + ( _Pos3.y - i.uv.y) * (_Pos3.y - i.uv.y );
				float distSquare4 = ( _Pos4.x - i.uv.x) * (_Pos4.x - i.uv.x )
							      + ( _Pos4.y - i.uv.y) * (_Pos4.y - i.uv.y );
				float distSquare5 = ( _Pos5.x - i.uv.x) * (_Pos5.x - i.uv.x )
								  + ( _Pos5.y - i.uv.y) * (_Pos5.y - i.uv.y );
				
				distSquare1 = (distSquare1 - _TresholdMin) / (_TresholdMax - _TresholdMin);
				distSquare2 = (distSquare2 - _TresholdMin) / (_TresholdMax - _TresholdMin);
				distSquare3 = (distSquare3 - _TresholdMin) / (_TresholdMax - _TresholdMin);
				distSquare4 = (distSquare4 - _TresholdMin) / (_TresholdMax - _TresholdMin);
				distSquare5 = (distSquare5 - _TresholdMin) / (_TresholdMax - _TresholdMin);

				valSum +=  1 - clamp(distSquare1, 0, 1);
				valSum +=  1 - clamp(distSquare2, 0, 1);
				valSum +=  1 - clamp(distSquare3, 0, 1);
				valSum +=  1 - clamp(distSquare4, 0, 1);
				valSum +=  1 - clamp(distSquare5, 0, 1);
				valSum = valSum / 5;

				col.a = valSum;// (valSum - _BlobMin) / (_BlobMax - _BlobMin);
				if (valSum > _BlobMin * _BlobMin)
					col.a = _ColorCenter.a;
				else if (valSum > _BlobBorder * _BlobBorder)
					col = _ColorBorder;
				else
					col.a = 0;
				
				return col;
				}

				ENDCG
			}
		}
}
