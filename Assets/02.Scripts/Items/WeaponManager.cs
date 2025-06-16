using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform weaponAnchor;
    [Header("무기 프리팹들")]
    [SerializeField] private GameObject[] weaponPrefabs;
    private int curWeaponInedex = -1;

    private GameObject currentWeaponGO;
    private Weapon currentWeapon;
    



    private void Awake()
    {
    
    }

    private void Start()
    {
        PlayerEvent.Swap  += EquipWeapon;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
          //  EquipWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //EquipWeapon(1);
        }
    }

    private void SwapWeapon()
    { 
        
    }


    private void EquipWeapon(int index)
    {
        if (index == curWeaponInedex || index >= weaponPrefabs.Length)
        {
            return;
        }

        if (currentWeaponGO != null)
        {
            Destroy(currentWeaponGO);
            currentWeaponGO = null;
            currentWeapon = null;
        }

       

        GameObject newWeaponGO = Instantiate(weaponPrefabs[index], weaponAnchor,false);
        newWeaponGO.transform.localPosition = weaponPrefabs[index].transform.localPosition;
        newWeaponGO.transform.localRotation = weaponPrefabs[index].transform.localRotation;


        currentWeaponGO = newWeaponGO;
        currentWeapon = newWeaponGO.GetComponent<Weapon>();
        curWeaponInedex = index;

        Debug.Log($"무기변경{weaponPrefabs[index].name}");
        //필요 한것들
        //이전무기 끄기 다음무기 켜기 


     }
    
    
    public GameObject GetCurWeapon()
    {
        return weaponPrefabs[curWeaponInedex];
    }
}
