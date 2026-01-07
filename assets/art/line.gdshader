shader_type canvas_item;

uniform vec4 outline_color1 : source_color = vec4(1.0, 0.0, 0.0, 1.0);  // 内层描边
uniform float outline_width1 : hint_range(0.0, 2.0) = 1;
uniform vec4 outline_color2 : source_color = vec4(0.0, 0.0, 0.0, 1.0);  // 外层描边
uniform float outline_width2 : hint_range(0.0, 2.0) = 2;

void fragment() {
    vec2 uv = UV;
    
    // 第一层描边（内层）
    vec2 uv_up1 = uv + vec2(0, TEXTURE_PIXEL_SIZE.y) * outline_width1;
    vec2 uv_down1 = uv + vec2(0, -TEXTURE_PIXEL_SIZE.y) * outline_width1;
    vec2 uv_left1 = uv + vec2(TEXTURE_PIXEL_SIZE.x, 0) * outline_width1;
    vec2 uv_right1 = uv + vec2(-TEXTURE_PIXEL_SIZE.x, 0) * outline_width1;
    
    vec4 color_up1 = texture(TEXTURE, uv_up1);
    vec4 color_down1 = texture(TEXTURE, uv_down1);
    vec4 color_left1 = texture(TEXTURE, uv_left1);
    vec4 color_right1 = texture(TEXTURE, uv_right1);
    
    bool has_content_nearby1 = (color_up1.a > 0.1 || color_down1.a > 0.1 || 
                               color_left1.a > 0.1 || color_right1.a > 0.1);
    
    // 第二层描边（外层）
    vec2 uv_up2 = uv + vec2(0, TEXTURE_PIXEL_SIZE.y) * outline_width2;
    vec2 uv_down2 = uv + vec2(0, -TEXTURE_PIXEL_SIZE.y) * outline_width2;
    vec2 uv_left2 = uv + vec2(TEXTURE_PIXEL_SIZE.x, 0) * outline_width2;
    vec2 uv_right2 = uv + vec2(-TEXTURE_PIXEL_SIZE.x, 0) * outline_width2;
    
    vec4 color_up2 = texture(TEXTURE, uv_up2);
    vec4 color_down2 = texture(TEXTURE, uv_down2);
    vec4 color_left2 = texture(TEXTURE, uv_left2);
    vec4 color_right2 = texture(TEXTURE, uv_right2);
    
    bool has_content_nearby2 = (color_up2.a > 0.1 || color_down2.a > 0.1 || 
                               color_left2.a > 0.1 || color_right2.a > 0.1);
    
    vec4 original_color = texture(TEXTURE, UV);
    
    // 判断当前像素属于哪一层
    if (original_color.a > 0.1) {
        // 原始内容
        COLOR = original_color;
    } else if (has_content_nearby1) {
        // 内层描边
        COLOR = vec4(outline_color1.rgb, outline_color1.a);
    } else if (has_content_nearby2) {
        // 外层描边
        COLOR = vec4(outline_color2.rgb, outline_color2.a);
    } else {
        // 透明区域
        COLOR = original_color;
    }

}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
