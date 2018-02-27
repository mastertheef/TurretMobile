Shader "Custom/InvisibleMask" {
  SubShader {
    // draw after all opaque objects (queue = 2001):
    Tags { "Queue"="Geometry+501" }
	ColorMask 0
	ZWrite On
	
    Pass {
      
    }
  } 
}