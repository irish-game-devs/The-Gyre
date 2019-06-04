using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    // This is the base class from which all the weapons (or different types of attack)
    // will inherit. In future, Weapon might become itself subclass of something like a 
    // more generic object/powerup class.
    
    private float range; //attack distance
    private float damage;

    public virtual void Attack() { }
}
