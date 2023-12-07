using Godot;
using System;

public partial class Attachment : Node3D
{
    public enum AttachmentType { sight, magazine, barrel, grip, foregrip };
    public MeshInstance3D mesh;
    [Export] public AttachmentType type;
    
    /* Weapon Stat modifiers */
    // Aim in position and rotation
    public Vector3 aimPosition;
    public Vector3 aimRotation;
    
    // Damage and range modifications
    public float damagePerc;
    public float rangePerc;
    
    // mag modifications
    public float newAmmoCapacity;
    public float newAmmo;
}
