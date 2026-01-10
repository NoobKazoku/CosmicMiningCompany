shader_type canvas_item;

uniform vec4 color1 : source_color = vec4(0.984, 0.961, 0.937, 1.0); // 最亮
uniform vec4 color2 : source_color = vec4(0.949, 0.827, 0.671, 1.0);
uniform vec4 color3 : source_color = vec4(0.776, 0.624, 0.647, 1.0);
uniform vec4 color4 : source_color = vec4(0.545, 0.427, 0.612, 1.0);
uniform vec4 color5 : source_color = vec4(0.286, 0.302, 0.494, 1.0); 
uniform vec4 color6 : source_color = vec4(0.075, 0.078, 0.18, 1.0); // 最暗

void fragment() {
    // 直接使用最暗的颜色作为背景，不依赖屏幕纹理
    COLOR = color6;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
