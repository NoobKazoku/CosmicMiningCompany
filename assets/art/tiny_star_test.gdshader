shader_type canvas_item;
render_mode blend_premul_alpha;  // 添加预乘alpha混合模式，根据《using_viewport_as_texture.pdf》

#define M_PI 3.14

float rand(vec2 co)
{
    return fract(sin(dot(co.xy, vec2(12.9898, 78.233))) * 43758.5453);
}

void fragment()
{
    // ========== 可调整参数 ==========
    float background_prob = 0.996; // 背景光点生成概率，值越大光点越少
    float flicker_speed = 1.0;     // 闪烁速度系数
    float flicker_range = 720.0;   // 闪烁相位偏移范围

    // ========== 背景光点逻辑 ==========
    float brightness = 0.0;  // 重命名为brightness，更清晰

    // 使用归一化坐标计算随机值
    // SCREEN_PIXEL_SIZE：像素大小，用于坐标归一化（参考《screen-reading_shaders.pdf》）
    vec2 screen_size = 1.0 / SCREEN_PIXEL_SIZE;
    vec2 normalized_coord = FRAGCOORD.xy / screen_size;

    // 生成背景光点
    if (rand(normalized_coord) > background_prob)
    {
        // 为每个像素生成随机值
        float r = rand(FRAGCOORD.xy);

        // 生成随机闪烁效果：
        // TIME * (r * flicker_speed)：时间乘以随机系数，使每个光点闪烁频率不同
        // flicker_range * r：相位偏移，使光点闪烁不同步
        // 0.25 * sin(...) + 0.75：将sin值映射到0.5-1.0范围
        brightness = r * (0.25 * sin(TIME * (r * flicker_speed) + flicker_range * r) + 0.75);
    }

    // 根据《spatial_shader.pdf》文档，ALPHA的范围应为[0.0, 1.0]
    // 光点部分：alpha = brightness（光点越亮越不透明）
    // 非光点部分：alpha = 0.0（完全透明）
    float alpha = brightness;

    // 根据《using_viewport_as_texture.pdf》文档，使用预乘alpha混合
    // 颜色值乘以alpha实现预乘
    vec3 color = vec3(brightness) * alpha;

    // 输出颜色，alpha通道控制透明度
    // 根据《class_color.pdf》文档，a=0.0为完全透明，a=1.0为完全不透明
    COLOR = vec4(color, alpha);
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
