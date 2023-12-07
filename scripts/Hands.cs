using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class Hands : Node3D
{
    public Player player;
    RayCast3D ray;
    Camera3D cam;
    HudController hud;

    public override void _Ready()
    {
        base._Ready();
        cam = GetParent() as Camera3D;
        ray = cam.GetNode("ray") as RayCast3D;
        player = cam.GetParent().GetParent() as Player;
        hud = player.GetNode("Hud") as HudController;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Interact();
    }

    public bool Interact() {
        ray.TargetPosition = cam.Transform.Basis * Vector3.Forward * 10f;
        ray.ForceRaycastUpdate();
        Interactable i;
        string interacted;
        if(!ray.IsColliding()) {
            hud.UpdateInteractLabel(false);
            return false;
        }

        if (ray.GetCollider().HasMethod("Interact") && !IsAncestorOf(ray.GetCollider() as Interactable)) {
            i = ray.GetCollider() as Interactable;
            hud.UpdateInteractLabel(true);
            interacted = i.Interact();
            EquipItem(interacted.Capitalize());
        }

        return true;
    }

    public void EquipItem(string item) {
        GD.Print(item);
        string itemPath = "res://assets/weapons/" + item + ".tscn";
        PackedScene itemScene = GD.Load(itemPath) as PackedScene;
        AddChild(itemScene.Instantiate());
        Gun gun = GetNode("shotgun") as Gun;
    }

    public void Action1() {
        if (GetChildren().Count != 0) {
            var child = GetChild(0) as Gun;
            child.Action1();
        }
    }

    public void Action2() {
        if (GetChildren().Count != 0) {
            var child = GetChild(0) as Gun;
            child.Action2();
        }
    }
}

