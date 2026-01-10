shader_type canvas_item;

#define M_PI 3.14

float rand(vec2 co)
{
    return fract(sin(dot(co.xy, vec2(12.9898, 78.233))) * 43758.5453);
}

void fragment()
{
    // ========== 可调整参数 ==========
    float background_prob = 0.997; // 背景光点生成概率，值越大光点越少（降低这个值增加白点数量）
    float flicker_speed = 1.0;     // 闪烁速度系数
    float flicker_range = 720.0;   // 闪烁相位偏移范围
    float pixel_size = 16.0;       // 像素大小，用于生成网格（增加这个值减少白点数量）

    // ========== 背景光点逻辑 ==========
    float brightness = 0.0;
    float alpha = 0.0;

    // 使用世界坐标，因为根节点是Node2D
    vec2 world_pos = VERTEX;
    
    // 生成网格坐标，用于创建固定位置的光点
    vec2 grid_pos = floor(world_pos / pixel_size);
    
    // 生成背景光点
    if (rand(grid_pos) > background_prob)
    {
        // 为每个网格生成随机值
        float r = rand(grid_pos);

        // 生成随机闪烁效果
        brightness = r * (0.25 * sin(TIME * (r * flicker_speed) + flicker_range * r) + 0.75);
        alpha = brightness;
    }

    // 直接设置颜色，不使用预乘alpha
    COLOR = vec4(vec3(brightness), alpha);
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
