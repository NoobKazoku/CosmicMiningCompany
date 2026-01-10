shader_type canvas_item;

#define M_PI 3.14

float rand(vec2 co)
{
    return fract(sin(dot(co.xy, vec2(12.9898, 78.233))) * 43758.5453);
}

void fragment()
{
    // ========== 可调整参数 ==========
    float size = 30.0;        // 星星网格大小，值越大星星越稀疏
    float star_prob = 0.90;   // 星星生成概率阈值，值越大星星越少（降低这个值增加星星数量）
    float pulse_freq = 0.5;  // 脉动频率系数，值越大脉动越快
    float base_brightness = 1.2;  // 基础亮度
    float pulse_amplitude = 0.25;  // 脉动幅度
    
    // ========== 主要星星逻辑 ==========
    // 使用世界坐标，因为根节点是Node2D
    vec2 world_pos = VERTEX;
    vec2 pos = floor(1.0 / size * world_pos);
    
    float brightness = 0.0;
    float alpha = 0.0;
    float starValue = rand(pos);
    
    // 生成主要星星
    if (starValue > star_prob)
    {
        vec2 center = size * pos + vec2(size, size) * 0.5;
        float t = base_brightness + pulse_amplitude * sin(TIME + (starValue - star_prob) / (1.0 - star_prob) * pulse_freq+center.x+center.y);
      
        
        brightness = 1.0 - distance(world_pos, center) / (10.0 * size);
        brightness = brightness * t / (abs(world_pos.y - center.y)) * t / (abs(world_pos.x - center.x));
        brightness = max(0.1, brightness);  // 确保亮度值在合理范围内
        
        alpha = brightness;
    }
    
    // 直接设置颜色，不使用预乘alpha
    COLOR = vec4(vec3(brightness), alpha);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
