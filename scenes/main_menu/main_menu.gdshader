shader_type canvas_item;

uniform vec4 color1 : source_color = vec4(0.984, 0.961, 0.937, 1.0); // 最亮
uniform vec4 color2 : source_color = vec4(0.949, 0.827, 0.671, 1.0);
uniform vec4 color3 : source_color = vec4(0.776, 0.624, 0.647, 1.0);
uniform vec4 color4 : source_color = vec4(0.545, 0.427, 0.612, 1.0);
uniform vec4 color5 : source_color = vec4(0.286, 0.302, 0.494, 1.0); 
uniform vec4 color6 : source_color = vec4(0.075, 0.078, 0.18, 1.0); // 最暗
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;
void vertex() {
    // Called for every vertex the material is visible on.
}

void fragment() {
    // 获取屏幕纹理颜色
    vec4 original_color = textureLod(screen_texture, SCREEN_UV, 0.0);

    // 计算亮度（根据文档中的亮度公式）
    float luminance = dot(original_color.rgb, vec3(0.2125, 0.7154, 0.0721));

    // 根据亮度选择对应的颜色
    vec4 selected_color;

    if (luminance >= 0.95) {
        selected_color = color1;
    } else if (luminance >= 0.65) {
        selected_color = color2;
    } else if (luminance >= 0.5) {
        selected_color = color3;
    } else if (luminance >= 0.35) {
        selected_color = color4;
    } else if(luminance >= 0.2){
        selected_color = color5;
    }else {
        selected_color = color6;
    }

     COLOR = selected_color;
}

//void light() {
//    // Called for every pixel for every light affecting the CanvasItem.
//    // Uncomment to replace the default light processing function with this one.
//}
