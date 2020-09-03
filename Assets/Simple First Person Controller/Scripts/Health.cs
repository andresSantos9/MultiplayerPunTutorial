using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    Renderer [] visuals;
    public int myHealth = 100;
    public int enemyHealth;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync health
        if (stream.IsWriting)
        {
            stream.SendNext(myHealth);
        }
        else
        {
            //we are reading
            enemyHealth =(int)stream.ReceiveNext();

        }
    }

    public void TakeDamage(int damage)
    {
        myHealth -= damage;
    }
    
    IEnumerator Respawn()
    {
        SetRenderers(false);
        myHealth = 100;
        GetComponent<CharacterController>().enabled = false;
        transform.position = new Vector3(0, 10,0);
        yield return new WaitForSeconds(1);
        GetComponent<CharacterController>().enabled = true;
        SetRenderers(true);
    }

    void SetRenderers(bool state)
    {
        foreach(var renderer in visuals)
        {
            renderer.enabled = state;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        visuals = GetComponentsInChildren<Renderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (myHealth <=0)
        {
            StartCoroutine(Respawn());
        }
        
    }
}
