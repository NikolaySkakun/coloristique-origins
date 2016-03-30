// Copyright (C) Stanislaw Adaszewski, 2013
// http://algoholic.eu

#pragma strict

function Start () {

}

function Update () {

}

// this script pushes all rigidbodies that the character touches
var pushPower = 100.0;
var weight = 60.0;

function OnControllerColliderHit (hit : ControllerColliderHit)
{
    var body : Rigidbody = hit.collider.attachedRigidbody;
    var force : Vector3;

    // no rigidbody
    if (body == null || body.isKinematic) { return; }

    // We use gravity and weight to push things down, we use
    // our velocity and push power to push things other directions
    if (hit.moveDirection.y < -0.3) {
       force = Physics.gravity * weight;
    } else {
        force = hit.controller.velocity * pushPower;
    }

    // Apply the push
    body.AddForceAtPosition(force, hit.point);
}
