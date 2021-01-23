using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

[RequireComponent(typeof(WheelVehicle))]
public class Drivable : Interactable
{
    [SerializeField] private MainCharacterController driver;
    
    private bool _isDriving;
    private WheelVehicle _wheelVehicle;
    
    void Start()
    {
        _wheelVehicle = GetComponent<WheelVehicle>();
        _wheelVehicle.IsPlayer = _isDriving;
    }
    
    public override void Interact()
    {
        _isDriving = !_isDriving;
        _wheelVehicle.IsPlayer = _isDriving;
        _wheelVehicle.Handbrake = !_isDriving;

        if (_isDriving)
        {
            driver.getIntoVehicle(_wheelVehicle);
        }

        if (!_isDriving)
        {
            var vehicleTransform = _wheelVehicle.transform;
            Quaternion doorDirection = Quaternion.Euler(Quaternion.Euler(0, 90, 0) * vehicleTransform.forward);
            Vector3 getOut = vehicleTransform.position + (Quaternion.Euler(0, -90, 0) * vehicleTransform.forward * 2);
            
            Debug.Log(driver);
            driver.getOutOfVehicle(getOut, doorDirection);
        }
    }
}
