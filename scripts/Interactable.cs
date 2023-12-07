using Godot;
using System;

public partial class Interactable : StaticBody3D
{
    bool canInteract = true;
    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public string Interact() {
        if (!canInteract)
            return "";

        canInteract = false;
        GD.Print("Interactable class");
        
        GD.Print(GetParent().Name);
        GetParent().QueueFree();
        return GetParent().Name;
    }
}
