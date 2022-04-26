using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skeleton_animator : MonoBehaviour
{
    [SerializeField] private Animator ani;
    [SerializeField] private RawImage heart_1;
    [SerializeField] private RawImage heart_2;
    [SerializeField] private RawImage heart_3;
    [SerializeField] private wand_i wand_interaction;
    private int DamageCount = 0;
    private Color32 newColor = new Color32(255,255,225,100); 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (wand_interaction.IsSpell)
        {
            //Send the message to the Animator to activate the trigger parameter named "Jump"
            ani.SetTrigger("IsSpell");
            DamageCount += 1;
            // Debug.Log(DamageCount);
            ani.SetInteger("DamageCount", DamageCount);
        }
        // Debug.Log("damage"+ DamageCount);
        // Debug.Log("bool"+ wand_interaction.IsSpell);

        switch(3-DamageCount) 
        {
        case 0:
            heart_1.GetComponent<RawImage>().color = newColor;
            break;
        case 1:
            heart_2.GetComponent<RawImage>().color = newColor;
            break;
        case 2:
            heart_3.GetComponent<RawImage>().color = newColor;
            break;
        default:
            break;
        }
    }
}
