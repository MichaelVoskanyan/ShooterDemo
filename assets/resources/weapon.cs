using Godot;
using System;

public partial class weapon : Resource
{
    public bool isAiming, isAutomatic;
    [ExportCategory("Weapon Stats")]
    [Export] public float Damage;
    [Export] public float ReloadSpeed;
    [Export] public float FireRate;
    [Export] public float Range;
    [Export] public int AmmoCapacity;
    [Export] public int Ammo;
    [Export] public string AmmoType;

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
    // bool readyShoot = true;
    // float shootTimer = 0f;

    // public void Action1() {
    //     Shoot();
    // }

    // public void Action2() {
    //     Aim();
    // }
    // public void Sway () {
    //     Vector2 _pos = SwayPosition();
    //     Vector3 pos = new Vector3(_pos.X, _pos.Y, 0);
    //     targetPosition = targetPosition.Lerp(targetPosition + pos, returnSpeed * (float)GetProcessDeltaTime());
    // }

    // public void Inertia(float weight) {
    //     Vector3 inertia;
    //     if (mouse != lastMouse)
    //         inertia = new Vector3(-mouse.Y, mouse.X, 0);
    //     else
    //         inertia = Vector3.Zero;
        
    //     var mag = inertia.Length();
    //     inertia = inertia.Normalized();
    //     inertia *= Mathf.Clamp(mag, -weight, weight);
    //     targetRotation = targetRotation.Lerp(targetRotation - inertia, returnSpeed * (float)GetProcessDeltaTime());
    // }

    // public void ReturnToCenter() {
    //     if (isAiming == true) {
    //         targetRotation = targetRotation.Lerp(aimInRotation, returnSpeed * (float)GetProcessDeltaTime());
    //         targetPosition = targetPosition.Lerp(aimInPosition, returnSpeed * (float)GetProcessDeltaTime());
    //     }
    //     else {
    //         targetRotation = targetRotation.Lerp(restRotation, returnSpeed * (float)GetProcessDeltaTime());
    //         targetPosition = targetPosition.Lerp(restPosition, returnSpeed * (float)GetProcessDeltaTime());
    //     }

    //     currentRotation = currentRotation.Lerp(targetRotation, snappiness * (float)GetPhysicsProcessDeltaTime());
    //     currentPosition = currentPosition.Lerp(targetPosition, snappiness * (float)GetPhysicsProcessDeltaTime());

    //     Rotation = currentRotation;
    //     Position = currentPosition; 
    // }

    // public void WeaponPosition() {
    //     if (GetParent().Name != "EquipSlot")
    //         return;

    //     ReturnToCenter();
    //     if (isAiming)
    //         Inertia(aimInertiaWeight);
    //     else
    //         Inertia(inertiaWeight);
        
    //     Sway();
    // }

    // public void RecoilFire() {
    //     targetRotation += new Vector3(recoilX, (float)GD.RandRange(-recoilY, recoilY) / 2, (float)GD.RandRange(-recoilZ, recoilZ) / 2);
    // }

    // public virtual void Shoot() {
    //     // GD.Print("Shoot");
    //     if (!readyShoot || shootTimer > 0)
    //         return;
    //     GD.Print("Shoot");
    //     if (!isAutomatic)
    //         readyShoot = false;
    //     shootTimer = FireRate;
    //     RecoilFire();
    // }

    public virtual void Aim() {
        isAiming = !isAiming;
        // GD.Print("Aiming: " + isAiming.ToString());
    }

    public Vector2 SwayPosition() {
        float x = swayAmount * Mathf.Sin(swayTimer);
        float y = swayAmount * Mathf.Sin(swayTimer) * Mathf.Cos(swayTimer);

        return new Vector2(x, y);
    }

    
}
