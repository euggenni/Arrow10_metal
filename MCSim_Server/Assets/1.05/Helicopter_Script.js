/* 	
============================================================================================================
	 _	 _  ____  _      _  ____  _____  _____  _______  ____  _____
	| | | || ___|| |    | || ___||  _  || ___ ||__   __|| ___|| ___ |
	| |_| || |__ | |    | || |   | | | || |_| |   | |   | |__ | |_| |
	|  _  || ___|| |    | || |   | | | || ____|   | |   | ___|| __  |
	| | | || |__ | |___ | || |__ | |_| || |       | |   | |__ | | \ \
	|_| |_||____||_____||_||____||_____||_|       |_|   |____||_|  \_\
	
		 _______  _   _  _______  _____  _____   _  _____  _
		|__   __|| | | ||__   __||  _  || ___ | | ||  _  || |
		   | |   | | | |   | |   | | | || |_| | | || |_| || |
		   | |   | | | |   | |   | | | ||  _  | | ||  _  || |
		   | |   | |_| |   | |   | |_| || | \ \ | || | | || |___
		   |_|   |_____|   |_|   |_____||_|  \_\|_||_| |_||_____|
	   
				   	 _________________________
					|_____by: Andrew Gotow____|
			
============================================================================================================
	   
	This is the basic scipt for controlling the player helicopter. It works by adding forces to a rigidbody in order to
simulate the forces a helicopter actually applies in-flight. You can easily find a good description of how helicopters
fly by looking at the "HowStuffWorks" article, or reading the included PDF.
	
	http://science.howstuffworks.com/helicopter5.htm

	Essentially, helicopters fly by spinning minniature airfoils (airplane wing shapes) very quickly. This creates lift the
same way an airplane does, the pressure over the wing surface (or rotor in this case) is lower than under it. The way helicopters
move is by altering the angle of the rotor blades, thus changing the pressure difference on either side of the helicopter.
When the blades are angled steeper towards the back of the helicopter, more lift is created towards the tail, tilting the body
and pushing the helicopter forward.	

	This tutorial is an attempt to simplify this and implement it in the Unity3D physics engine. All of the default values in this
tutorial are taken from a real helicopter, the BO105. Doing this ensures that the helicopter has relatively realistic abilities
in the game. You can read the rest of this tutorial, and a more detailed description in the included PDF.
*/


var 			main_Rotor_GameObject 				: GameObject;			// gameObject to be animated
var				tail_Rotor_GameObject 				: GameObject;			// gameObject to be animated

var				max_Rotor_Force 					: float = 22241.1081;	// newtons
static var 		max_Rotor_Velocity 					: float = 7200;			// degrees per second
private var 	rotor_Velocity 						: float = 0.0;			// value between 0 and 1
private var 	rotor_Rotation 						: float = 0.0; 			// degrees... used for animating rotors

var 			max_tail_Rotor_Force 				: float = 15000.0; 		// newtons
var 			max_Tail_Rotor_Velocity 			: float = 2200.0; 		// degrees per second
private var 	tail_Rotor_Velocity 				: float = 0.0; 			// value between 0 and 1
private var 	tail_Rotor_Rotation 				: float = 0.0; 			// degrees... used for animating rotors
	
var 			forward_Rotor_Torque_Multiplier 	: float = 0.5;			// multiplier for control input
var 			sideways_Rotor_Torque_Multiplier	: float = 0.5;			// multiplier for control input
var             speed                               : float = 0.5;

static var 		main_Rotor_Active					: boolean = true;		// boolean for determining if a prrop is active
static var 		tail_Rotor_Active					: boolean = true;		// boolean for determining if a prop is active

// Forces are applied in a fixed update function so that they are consistent no matter what the frame rate of the game is. This is 
// important to keeping the helicopter stable in the air. If the forces were applied at an inconsistent rate, the helicopter would behave 
// irregularly.
function FixedUpdate () {
	
	// First we must compute the torque values that are applied to the helicopter by the propellers. The "Control Torque" is used to simulate
	// the variable angle of the blades on a helicopter and the "Torque Value" is the final sum of the torque from the engine attached to the 
	// main rotor, and the torque applied by the tail rotor.
	var torqueValue : Vector3;
	var controlTorque : Vector3 = Vector3( speed * forward_Rotor_Torque_Multiplier, 1.0, -Input.GetAxis( "Horizontal" ) * sideways_Rotor_Torque_Multiplier );
	
	// Now check if the main rotor is active, if it is, then add it's torque to the "Torque Value", and apply the forces to the body of the 
	// helicopter.
	if ( main_Rotor_Active == true ) {
		torqueValue += (controlTorque * max_Rotor_Force * rotor_Velocity);
		
		// Now the force of the prop is applied. The main rotor applies a force direclty related to the maximum force of the prop and the 
		// prop velocity (a value from 0 to 1)
		rigidbody.AddRelativeForce( Vector3.up * max_Rotor_Force * rotor_Velocity );
		
		// This is simple code to help stabilize the helicopter. It essentially pulls the body back towards neutral when it is at an angle to
		// prevent it from tumbling in the air.
		if ( Vector3.Angle( Vector3.up, transform.up ) < 80 ) {
			transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.Euler( 0, transform.rotation.eulerAngles.y, 0 ), Time.deltaTime * rotor_Velocity * 2 );
		}
	}
	
	// Now we check to make sure the tail rotor is active, if it is, we add it's force to the "Torque Value"
	if ( tail_Rotor_Active == true ) {
		torqueValue -= (Vector3.up * max_tail_Rotor_Force * tail_Rotor_Velocity);
	}
	
	// And finally, apply the torques to the body of the helicopter.
	rigidbody.AddRelativeTorque( torqueValue );
}

function Update () {
	// This line simply changes the pitch of the attached audio emitter to match the speed of the main rotor.
	//audio.pitch = rotor_Velocity;
	
	// Now we animate the rotors, simply by setting their rotation to an increasing value multiplied by the helicopter body's rotation.
	if ( main_Rotor_Active == true ) {
		main_Rotor_GameObject.transform.rotation = transform.rotation * Quaternion.Euler( 0, rotor_Rotation, 0 );
	}
	if ( tail_Rotor_Active == true ) {
		tail_Rotor_GameObject.transform.rotation = transform.rotation * Quaternion.Euler( tail_Rotor_Rotation, 0, 0 );
	}
		
	// this just increases the rotation value for the animation of the rotors.
	rotor_Rotation += max_Rotor_Velocity * rotor_Velocity * Time.deltaTime;
	tail_Rotor_Rotation += max_Tail_Rotor_Velocity * rotor_Velocity * Time.deltaTime;
	
	// here we find the velocity required to keep the helicopter level. With the rotors at this speed, all forces on the helicopter cancel 
	// each other out and it should hover as-is.
	var hover_Rotor_Velocity = (rigidbody.mass * Mathf.Abs( Physics.gravity.y ) / max_Rotor_Force);
	var hover_Tail_Rotor_Velocity = (max_Rotor_Force * rotor_Velocity) / max_tail_Rotor_Force;
	
	// Now check if the player is applying any throttle control input, if they are, then increase or decrease the prop velocity, otherwise, 
	// slowly LERP the rotor speed to the neutral speed. The tail rotor velocity is set to the neutral speed plus the player horizontal input. 
	// Because the torque applied by the main rotor is directly proportional to the velocity of the main rotor and the velocity of the tail rotor,
	// so when the tail rotor velocity decreases, the body of the helicopter rotates.
	if ( speed != 0.0 ) {
		rotor_Velocity += speed * 0.001;
	}else{
		rotor_Velocity = Mathf.Lerp( rotor_Velocity, hover_Rotor_Velocity, Time.deltaTime * Time.deltaTime * 5 );
	}
	tail_Rotor_Velocity = hover_Tail_Rotor_Velocity - Input.GetAxis( "Horizontal" );
	
	// now we set velocity limits. The multiplier for rotor velocity is fixed to a range between 0 and 1. You can limit the tail rotor velocity 
	// too, but this makes it more difficult to balance the helicopter variables so that the helicopter will fly well.
	if ( rotor_Velocity > 1.0 ) {
		rotor_Velocity = 1.0;
	}else if ( rotor_Velocity < 0.0 ) {
		rotor_Velocity = 0.0;
	}
}