extends Camera2D
class_name PanningCamera2D

const MIN_ZOOM: float = 0.1
const MAX_ZOOM: float = 2
const ZOOM_RATE: float = 8.0
const ZOOM_INCREMENT: float = 0.1
var CAN_PAN: bool = true
var _target_zoom: float = 2.0
var alarm_timer
func _ready():
	alarm_timer = $Timer
	#alarm_timer.start(0)
	target_position = Vector2(260,170)
	position=target_position;

	# Get the screen count and set the display to the second screen (screen 1, assuming you have two monitors)




	
# Constants
const PAN_SPEED: float = 300  # Speed at which the camera pans
const PAN_SMOOTHING: float = 0.8  # Smoothing factor for panning
const EDGE_PAN_THRESHOLD: float = 40  # Distance from edge to start panning
const MOUSE_MARGIN: float = 20.0
# Variables
var target_position: Vector2

func _process(delta):
	if CAN_PAN and Input.get_mouse_mode() == Input.MOUSE_MODE_VISIBLE:
		var viewport_size = get_viewport_rect().size
		var mouse_pos = get_viewport().get_mouse_position()

		if mouse_pos.x < 0 or mouse_pos.y < 0 or mouse_pos.x > viewport_size.x or mouse_pos.y > viewport_size.y:
			return 
		mouse_pos.x = clamp(mouse_pos.x, 0, viewport_size.x)
		mouse_pos.y = clamp(mouse_pos.y, 0, viewport_size.y)

		var motion = Vector2(0,0)
		
			# Check if the mouse is near the edges of the viewport
		if mouse_pos.x < EDGE_PAN_THRESHOLD:
			motion.x = -1
		elif mouse_pos.x > viewport_size.x - EDGE_PAN_THRESHOLD:
			motion.x = 1

		if mouse_pos.y < EDGE_PAN_THRESHOLD:
			motion.y = -1
		elif mouse_pos.y > viewport_size.y - EDGE_PAN_THRESHOLD:
			motion.y = 1

	# Update the target position
	
		target_position += motion * PAN_SPEED * delta
	# Smoothly move the camera to the target position
		position = position.lerp(target_position, PAN_SMOOTHING)
		
		position.x = clamp(position.x, 0, 10000)
		position.y = clamp(position.y, 0, 100000)
		
		position.x= roundi(position.x)
		position.y= roundi(position.y)
	
func _physics_process(delta: float) -> void:
	zoom = lerp(zoom, _target_zoom * Vector2.ONE, ZOOM_RATE * delta)
	set_physics_process(not is_equal_approx(zoom.x, _target_zoom))
	

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		if event.is_pressed():
			if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
				zoom_in()
			if event.button_index == MOUSE_BUTTON_WHEEL_UP:
				zoom_out()

	if event is InputEventMouseMotion:
		if event.button_mask == MOUSE_BUTTON_MASK_MIDDLE:
			position -= event.relative * (zoom/2)


func zoom_in() -> void:
	_target_zoom = max(_target_zoom - ZOOM_INCREMENT, MIN_ZOOM)
	_target_zoom = roundi(_target_zoom)
	set_physics_process(true)


func zoom_out() -> void:
	_target_zoom = min(_target_zoom + ZOOM_INCREMENT, MAX_ZOOM)
	_target_zoom = roundi(_target_zoom)
	set_physics_process(true)




			



func _on_timer_timeout() -> void:
	
	CAN_PAN = true
	alarm_timer.stop()
