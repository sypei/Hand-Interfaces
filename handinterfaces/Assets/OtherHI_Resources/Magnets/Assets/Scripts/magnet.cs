using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class magnet : MonoBehaviour {

    public float magnetPower;
    [Tooltip("Speed of the magnet movement")]
    public float speed;
    public float radius = 5f;

    private void Start()
    {
        if (!GetComponent<SphereCollider>().isTrigger)
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }

        GetComponent<SphereCollider>().radius = radius;
    }

    private void OnTriggerStay(Collider other)
    {
        //if the object have attractable tag
        if (other.CompareTag("attractable"))
        {
            //execute attract method and insert 2 variables: the rigidbody of the object that must be attracted and the rigidbody of the magnet
            attract(other.GetComponent<Rigidbody>(), GetComponent<Rigidbody>());
        }
    }

    void attract(Rigidbody rbToAttract, Rigidbody magnetRb)
    {
        //if the magnet is smaller than the object that must be attracted
        if (rbToAttract.mass > magnetRb.mass)
        {
            //add force to the rigidbody of the magnet
            magnetRb.AddForce(-(magnetRb.position - rbToAttract.position) * magnetPower);
            //returns
            return;
        }

        //add force to the rigidbody that must be attracted
        rbToAttract.AddForce((magnetRb.position - rbToAttract.position) * magnetPower * rbToAttract.mass * Time.deltaTime);
    }

    private void Update()
    {
        //this code moves the magnet with WSAD and arrows keys
        transform.Translate(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);
    }
}
