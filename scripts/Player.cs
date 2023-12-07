using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class Player : CharacterBody3D
{
    /* On-Ready variables */
    public Camera3D cam;    // Camera
    public Node3D neck;     // Pivot point for camera
    public RayCast3D ray;
    public Hands hands;

    [ExportCategory("Movement Settings")]
    [Export] public float move_speed;
    [Export] public float mouse_sensitivity;
    [Export] public float grounded_move_rate;
    [Export] public float air_move_rate;
    [Export] public float jump_height;

    Vector2 move_input;
    Vector3 move_direction;

    [ExportCategory("Physics Settings")]
    [Export] public float gravity = 9.81f;
    [Export] public float mass = 75f;   // kg
    
    public List<string> Items;

    float dTime;
    Vector3 movement;   // Handles the actual math with movement + Vector mutation
    public Vector2 mouse_movement; // Relative mouse input is passed in here


    public override void _Ready()
    {
        base._Ready();
        Input.MouseMode = Input.MouseModeEnum.Captured;
        neck = GetNode<Node3D>("Neck");
        cam = neck.GetChild<Camera3D>(0);
        hands = cam.GetNode("EquipSlot") as Hands;
        Items = new List<string>();
    }

    public void InputHandler() {
        // Movement input handler
        move_input = Input.GetVector("move_left", "move_right", "move_front", "move_back").Normalized();

        // Equipped item actions
        if (Input.IsActionPressed("shoot"))
            hands.Action1();
        if (Input.IsActionJustPressed("aim"))
            hands.Action2();
        
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        dTime = (float)delta;   // class wide float for delta so I don't have to pass it around like a Tacoma whore gets passed around
        InputHandler();
        Movement();      
        // hands.Interact();
        if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
            Jump();
        }
        Velocity = movement;
        MoveAndSlide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        // check if the input event is mouse movement
        if (@event is InputEventMouseMotion mouseMotion) {
            mouse_movement = mouseMotion.Relative;
            RotateY (-mouse_movement.X * mouse_sensitivity * dTime);
            neck.RotateX (-mouse_movement.Y * mouse_sensitivity * dTime);

            // Clamp vertical rotation
            neck.Rotation = new Vector3(
                Mathf.Clamp(neck.Rotation.X, -Mathf.DegToRad(70), Mathf.DegToRad(70)),
                neck.Rotation.Y,
                neck.Rotation.Z
            );
        }
    }

    void Movement() {
        
        move_direction = Transform.Basis * new Vector3(move_input.X, 0, move_input.Y);

        if (IsOnFloor()) {
            // Clamps the grounded_move_rate to a max of 1 since Mathf.Lerp breaks if the delta is > 1
            float ground_move = Mathf.Clamp(grounded_move_rate * dTime, 0, 1);
            movement.X = Mathf.Lerp(movement.X, move_direction.X * move_speed, ground_move);
            movement.Z = Mathf.Lerp(movement.Z, move_direction.Z * move_speed, ground_move);
        } else {
            movement.X = Mathf.Lerp(movement.X, move_direction.X * move_speed, air_move_rate * dTime);
            movement.Z = Mathf.Lerp(movement.Z, move_direction.Z * move_speed, air_move_rate * dTime);
            Gravity();
        }
    }

    void Gravity() {
        movement.Y -= gravity * dTime;
    }

    void Jump() {
        GD.Print("Jumping");
        movement.Y = Mathf.Sqrt(2 * jump_height * gravity);
    }

    public void AddItem(string item) {
        Items.Add(item);
        hands.EquipItem(item);
        return;
    }

    public void RemoveItem(string item) {
        Items.Remove(item);
        return;
    }

}
