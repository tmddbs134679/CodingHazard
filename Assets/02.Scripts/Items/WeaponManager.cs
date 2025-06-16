using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;
    private int curWeaponInedex = 0;


    private void Awake()
    {
     //   weapons = new Weapon[3];
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(1);
        }
    }


    private void EquipWeapon(int index)
    {
        if (index == curWeaponInedex || index >= weapons.Length)
        {
            return;
        }

        if (weapons[curWeaponInedex] != null)
        {
            weapons[curWeaponInedex].SwapWeaponOff();
        }
        weapons[index].SwapWeaponON();
        curWeaponInedex = index;

        Debug.Log($"무기변경{weapons[index].name}");
        //필요 한것들
        //이전무기 끄기 다음무기 켜기 


     }
    public void AddWeapon(Weapon weapon)
    {
        int index = (int)weapon.GetWeaponType();

        if (weapons[index] != null)
        {
            //스왑
            weapons[index].DropItem();
        }

        weapons[index] = weapon;
        weapon.gameObject.SetActive(false);
        

    }
    
    public Weapon GetCurWeapon()
    {
        return weapons[curWeaponInedex];
    }
}
