extends Camera2D
class_name PanningCamera2D

const MIN_ZOOM: float = 0.1
const MAX_ZOOM: float = 2
const ZOOM_RATE: float = 8.0
const ZOOM_INCREMENT: float = 0.1

var _target_zoom: float = 1.0



# Constants
const PAN_SPEED: float = 300  # Speed at which the camera pans
const PAN_SMOOTHING: float = 0.8  # Smoothing factor for panning
const EDGE_PAN_THRESHOLD: float = 40  # Distance from edge to start panning
const MOUSE_MARGIN: float = 20.0
# Variables
var target_position: Vector2

func _process(delta):
	var viewport_size = get_viewport_rect().size
	var mouse_pos = get_viewport().get_mouse_position()


	mouse_pos.x = clamp(mouse_pos.x, 0, viewport_size.x)
	mouse_pos.y = clamp(mouse_pos.y, 0, viewport_size.y)

	var motion = Vector2()
	
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
	set_physics_process(true)


func zoom_out() -> void:
	_target_zoom = min(_target_zoom + ZOOM_INCREMENT, MAX_ZOOM)
	set_physics_process(true)



			

