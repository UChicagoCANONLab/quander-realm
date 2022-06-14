using UnityEngine;
using System.Collections;
using Costume = AssetCostumeUtilities;

/* Button manager (experiment mode) */

public class ExperimentButtonController : ButtonController
{
    public new static ButtonController Instance { get; protected set; }

    private new void Awake()
    {
        Instance = this;
        UpdateButtonState(ButtonState.CanNotBePressed);
    }

    // Subscriber to CakeOnBeltTracker
    public override void OnCakesReady()
    {
        UpdateButtonState(ButtonState.CanBePressed);
    }

    // Subscriber to CakeOnBeltTracker
    public override void OnCakesRemovedFromBelt()
    {
        UpdateButtonState(ButtonState.CanNotBePressed);
    }
}
