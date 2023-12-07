using Godot;
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

public partial class Gun : Node3D
{
    public bool isAiming, isAutomatic;
    [ExportCategory("Gun Stats")]
    [Export] public string WeaponName;

    [Export] public float Damage;
    [Export] public float Range;
    [Export] public float FireRate;
    [Export] public float ReloadSpeed;
    [Export] public int Ammo;
    [Export] public int AmmoCapacity;
    [Export] public string AmmoType;

    /* Attachment points */
    // if null, no attachment possible for that type of attachment
    [Export] public Attachment sight, barrel, magazine, foregrip, grip;

    /* Hipfire recoil */
    [Export] float recoilX, recoilY, recoilZ;

    // Settings
    [Export] float snappiness, returnSpeed;

    [ExportCategory("Gun References")]
    [Export] public Vector3 shootPoint;

    [Export] public Vector3 aimInPosition, aimInRotation, restPosition, restRotation;

    [ExportCategory("Gun Movement")]
    [Export] public float inertiaWeight = 0.25f;
    [Export] public float aimInertiaWeight = 0.05f;
    [Export] public float idleSwayAmount = 0.01f;
    [Export] public float idleSwaySpeed = 1f;
    [Export] public float moveSwaySpeed = 5f;
    
    public float swayAmount = 0.005f;
    public float swaySpeed = 2f;
    public float swayTimer = 0f;

    /* References */
    Vector2 mouse, lastMouse;
    Vector2 movement;
    AnimationPlayer animation;

    /* Rotation */
    Vector3 targetRotation, currentRotation;

    /* Position */
    Vector3 targetPosition, currentPosition;

    /* Shooting */
    bool readyShoot = true;
    float shootTimer = 0f;

    public void Action1() {
        Shoot();
    }

    public void Action2() {
        Aim();
    }

    public void Attachments() {
        
    }

    public void Sway () {
        Vector2 _pos = SwayPosition();
        Vector3 pos = new Vector3(_pos.X, _pos.Y, 0);
        targetPosition = targetPosition.Lerp(targetPosition + pos, returnSpeed * (float)GetProcessDeltaTime());
    }

    public void Inertia(float weight) {
        Vector3 inertia;
        if (mouse != lastMouse)
            inertia = new Vector3(-mouse.Y, mouse.X, 0);
        else
            inertia = Vector3.Zero;
        
        var mag = inertia.Length();
        inertia = inertia.Normalized();
        inertia *= Mathf.Clamp(mag, -weight, weight);
        targetRotation = targetRotation.Lerp(targetRotation - inertia, returnSpeed * (float)GetProcessDeltaTime());
    }

    public void ReturnToCenter() {
        if (isAiming == true) {
            targetRotation = targetRotation.Lerp(aimInRotation, returnSpeed * (float)GetProcessDeltaTime());
            targetPosition = targetPosition.Lerp(aimInPosition, returnSpeed * (float)GetProcessDeltaTime());
        }
        else {
            targetRotation = targetRotation.Lerp(restRotation, returnSpeed * (float)GetProcessDeltaTime());
            targetPosition = targetPosition.Lerp(restPosition, returnSpeed * (float)GetProcessDeltaTime());
        }

        currentRotation = currentRotation.Lerp(targetRotation, snappiness * (float)GetPhysicsProcessDeltaTime());
        currentPosition = currentPosition.Lerp(targetPosition, snappiness * (float)GetPhysicsProcessDeltaTime());

        Rotation = currentRotation;
        Position = currentPosition; 
    }

    public void WeaponPosition() {
        if (GetParent().Name != "EquipSlot")
            return;

        ReturnToCenter();
        if (isAiming)
            Inertia(aimInertiaWeight);
        else
            Inertia(inertiaWeight);
        
        Sway();
    }

    public void RecoilFire() {
        targetRotation += new Vector3(recoilX, (float)GD.RandRange(-recoilY, recoilY) / 2, (float)GD.RandRange(-recoilZ, recoilZ) / 2);
    }

    public void Shoot() {
        // GD.Print("Shoot");
        if (!readyShoot || shootTimer > 0)
            return;
        GD.Print("Shoot");
        if (!isAutomatic)
            readyShoot = false;
        shootTimer = FireRate;
        RecoilFire();
    }

    public void Aim() {
        isAiming = !isAiming;
    }

    public Vector2 SwayPosition() {
        float x = swayAmount * Mathf.Sin(swayTimer);
        float y = swayAmount * Mathf.Sin(swayTimer) * Mathf.Cos(swayTimer);

        return new Vector2(x, y);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (@event is InputEventMouseMotion e) {
            mouse = e.Relative;
        }
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        WeaponPosition();

        shootTimer -= (float)delta;

        if (Input.IsActionJustReleased("shoot") && !readyShoot)
            readyShoot = !readyShoot;

        movement = Input.GetVector("move_left", "move_right", "move_front", "move_back").Normalized();
        
        if (movement.Length() <= 0) {
            swayTimer += (float)delta * idleSwaySpeed;
            swayAmount = idleSwayAmount;
        }
        else {
            swayTimer += (float)delta * moveSwaySpeed * movement.Length();
            swayAmount = idleSwayAmount * 2;
        }
        if (swayTimer >= Mathf.Pi * 2) {
            swayTimer = 0;
            GD.Print("Reset timer");
        }

        lastMouse = mouse;
    }
}
