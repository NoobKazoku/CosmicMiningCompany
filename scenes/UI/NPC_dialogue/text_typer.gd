extends RichTextLabel

##打字机文本->想要显示的实际文本
@onready var typer_text :String = "????";

# 打字机进度
@onready var progress_index:int = 0;

#打字的速度
@onready var speed:float = 0.075;
@onready var timer :float = 0.0;  #计时器



# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass

func _process(delta: float) -> void:
	# 判断是否完成打字
	if (progress_index >= typer_text.length()):
		pass;
	else :
		#未完成打字逻辑
		if (timer <= 0.0):
			timer = speed;
			print_char();
			progress_index += 1
		else :
			timer -= delta;

#实现打字逻辑
func print_char() -> void:
	var solo_char : String = typer_text[progress_index];
	#当前实际显示的文本
	self.text += solo_char
