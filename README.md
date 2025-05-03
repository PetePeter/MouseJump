# MouseJump
windows app for the mouse cursor jumping to bookmarked or centre position 

# (coming soon - it's an old app i wrote, ill upload to github  this weekend when i get time)

# Instructions.

MouseJump adds an icon near the clock thingo of your computer.
This lets you right click it to:
 - Enable/disable
 - Lock/unlock
 - Exit

Enable stops/starts the mouse/keyboard monitoring
Lock   allows/disallows changing of the points after they've been added.
Exit  .. exits.
                
Keyboard
-	Double shift: Push current cursor location to stack
-	Double ctrl: Pop stack location and jump to the popped location.
-	Double Alt: Moves the mouse to the centre of the currently active window
-	Double caps lock: Walk through the stored positions without consuming.
-	Alt+Scrollwheel: Navigate through saved points forward and backward.

Mouse
-	Double middleclick to open up overlay. Once overlay is open:
	o	Any keyboard key to exit.
	o	Long single middle click on overlay: add the cursor position to stack.
	o	Scroll wheel spin: select another bookmark.
	o	Left click: jump to (but don’t consume) selected bookmark and close overlay
	o	Right click: jump to and consume selected bookmark and close overlay
	o	Right click on selected bookmark: remove selected bookmark but don’t close  overlay.
	o	Middle click: Save current cursor  position as top of the stack and jump to the selected position. 
