using Godot;

public partial class HudController : Node
{
    /* References */
    // Need reference to "Hands" (weapon controller)
    Hands weaponController;
    Gun weapon;
    // References to HUD elements
    Label ammoCounter;
    Label interactLabel;
    public TextureRect crosshair;

    public override void _Ready()
    {
        base._Ready();
        weaponController = GetParent().GetNode("Neck/Camera3D/EquipSlot") as Hands;
        ammoCounter = GetNode("Ammo") as Label;
        interactLabel = GetNode("Interact") as Label;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        UpdateAmmoCounter();
    }

    public void UpdateAmmoCounter() {
        // GD.Print(weaponController.ToString());
        if(weaponController.GetChildren().Count == 0) {
            return;
        }
        weapon = weaponController.GetChild(0) as Gun;
        
        string text = weapon.Ammo.ToString() + " / " + weapon.AmmoCapacity.ToString();

        ammoCounter.Text = text;
    }

    public void UpdateInteractLabel(bool canInteract) {
        if(!canInteract) {
            interactLabel.Text = "";
            return;
        }

        interactLabel.Text = "Press __ to Interact";
    }

    public void UpdateCrosshairPosition() {

    }
}
