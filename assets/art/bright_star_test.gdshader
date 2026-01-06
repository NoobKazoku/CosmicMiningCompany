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
    float size = 30.0;        // 星星网格大小，值越大星星越稀疏
    float star_prob = 0.95;   // 星星生成概率阈值，值越大星星越少
    float pulse_freq = 0.5;  // 脉动频率系数，值越大脉动越快
    float base_brightness = 1.2;  // 基础亮度
    float pulse_amplitude = 0.25;  // 脉动幅度
    
    // ========== 主要星星逻辑 ==========
    vec2 pos = floor(1.0 / size * FRAGCOORD.xy);
    
    float brightness = 0.0;
    float starValue = rand(pos);
    
    // 生成主要星星
    if (starValue > star_prob)
    {
        
		vec2 center = size * pos + vec2(size, size) * 0.5;
        float t = base_brightness + pulse_amplitude * sin(TIME + (starValue - star_prob) / (1.0 - star_prob) * pulse_freq+center.x+center.y);
     
        brightness = 1.0 - distance(FRAGCOORD.xy, center) / (10.0* size);
        brightness = brightness * t / (abs(FRAGCOORD.y - center.y)) * t / (abs(FRAGCOORD.x - center.x));
        brightness = max(0.1, brightness);  // 确保亮度值在合理范围内
    }
    
    // 根据《class_color.pdf》文档，a=0.0为完全透明，a=1.0为完全不透明
    // 星星部分：alpha = brightness（星星越亮越不透明）
    // 非星星部分：alpha = 0.0（完全透明）
    float alpha = brightness;
    
    // 根据《using_viewport_as_texture.pdf》文档，使用预乘alpha混合
    // 颜色值乘以alpha实现预乘
    vec3 color = vec3(brightness) * alpha;
    
    // 输出颜色，alpha通道控制透明度
    COLOR = vec4(color, alpha);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
