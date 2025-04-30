using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricShockProjectileBehaviour : ProjectileBehaviour
{
    [SerializeField] protected float hitOffset = 0f;
    [SerializeField] protected bool UseFirePointRotation;
    [SerializeField] protected Vector3 rotationOffset = new Vector3(0, 0, 0);
    [SerializeField] protected GameObject hit;
    [SerializeField] protected ParticleSystem hitPS;
    [SerializeField] protected GameObject flash;
    //[SerializeField] protected Rigidbody rb;
    //[SerializeField] protected Collider col;
    [SerializeField] protected Light lightSourse;
    [SerializeField] protected GameObject[] Detached;
    [SerializeField] protected ParticleSystem projectilePS;
    private bool startChecker = false;
    [SerializeField] protected bool notDestroy = false;

    //protected virtual void Start()
    //{
    //    if (!startChecker)
    //    {
    //        /*lightSourse = GetComponent<Light>();
    //        rb = GetComponent<Rigidbody>();
    //        col = GetComponent<Collider>();
    //        if (hit != null)
    //            hitPS = hit.GetComponent<ParticleSystem>();*/
    //        if (flash != null)
    //        {
    //            flash.transform.parent = null;
    //        }
    //    }
    //    if (notDestroy)
    //        StartCoroutine(DisableTimer(5));
    //    else
    //        Destroy(gameObject, 5);
    //    startChecker = true;
    //}

    //protected virtual IEnumerator DisableTimer(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    if (gameObject.activeSelf)
    //        gameObject.SetActive(false);
    //    yield break;
    //}

    //protected virtual void OnEnable()
    //{
    //    if (startChecker)
    //    {
    //        if (flash != null)
    //        {
    //            flash.transform.parent = null;
    //        }
    //        if (lightSourse != null)
    //            lightSourse.enabled = true;
    //        col.enabled = true;
    //        rb.constraints = RigidbodyConstraints.None;
    //    }
    //}

    //protected virtual void FixedUpdate()
    //{
    //    if (speed != 0)
    //    {
    //        rb.velocity = transform.forward * speed;
    //    }
    //}

    ////https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html
    public override void OnArrival()
    {
        if (projectilePS)
        {
            projectilePS.Stop();
            projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, direction.normalized*-1f);
        Vector3 pos = target;


            //if (UseFirePointRotation) { hit.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            //else if (rotationOffset != Vector3.zero) { hit.transform.rotation = Quaternion.Euler(rotationOffset); }
            //else { hit.transform.LookAt(contact.point + contact.normal); }
            //hitPS.Play();


        //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                ParticleSystem detachedPS = detachedPrefab.GetComponent<ParticleSystem>();
                detachedPS.Stop();
            }
        }
        //if (notDestroy)
        //    StartCoroutine(DisableTimer(hitPS.main.duration));
        //else
        //{
        //    if (hitPS != null)
        //    {
        //        Destroy(gameObject, hitPS.main.duration);
        //    }
        //    else
        //        Destroy(gameObject, 1);
        //}

        base.OnArrival();
    }
}
