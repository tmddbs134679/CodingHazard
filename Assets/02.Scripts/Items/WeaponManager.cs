using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon CurrentWpeaWeapon => _currentWeapon;

    
    [SerializeField] private Transform weaponAnchor;
    
    
    [Header("무기 프리팹들")]
    [SerializeField] private GameObject[] weaponPrefabs;
    

    private int _curWeaponIndex = -1;

    private GameObject _currentWeaponGO;
    
    private Weapon _currentWeapon;

    private Dictionary<WeaponType, Weapon> _weaponSlots = new();

    private List<GameObject> _weapons = new();
    
    private void Awake()
    {
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            GameObject newWeaponGO = Instantiate(weaponPrefabs[i], weaponAnchor,false);
            newWeaponGO.transform.localPosition = weaponPrefabs[i].transform.localPosition;
            newWeaponGO.transform.localRotation = weaponPrefabs[i].transform.localRotation;
            
            newWeaponGO.SetActive(false);
            
            _weapons.Add(newWeaponGO);
            
            _weaponSlots[(WeaponType)i] = newWeaponGO.GetComponentInChildren<Weapon>();
        }
    }


    private void Start()
    {
        PlayerEvent.Swap  += EquipWeapon;
        

        EquipWeapon(0);
    }


    private void EquipWeapon(int index)
    {
        if (index == _curWeaponIndex || index >= weaponPrefabs.Length)
        {
            return;
        }

        if (_currentWeaponGO != null)
        {
            _currentWeaponGO.SetActive(false);
            
            //Destroy(_currentWeaponGO);
            //_currentWeaponGO = null;
        }

        _currentWeaponGO = _weapons[index];
        _currentWeaponGO.SetActive(true);
        
        _curWeaponIndex = index;
        _currentWeapon = _weaponSlots[(WeaponType)_curWeaponIndex];
        
        /*GameObject newWeaponGO = Instantiate(weaponPrefabs[index], weaponAnchor,false);
        newWeaponGO.transform.localPosition = weaponPrefabs[index].transform.localPosition;
        newWeaponGO.transform.localRotation = weaponPrefabs[index].transform.localRotation;

        _currentWeaponGO = newWeaponGO;*/
      

        
        //_currentWeapon = newWeaponGO.GetComponentInChildren<Weapon>();

        /*else
        {
            _currentWeapon = newWeaponGO.GetComponentInChildren<Weapon>();

            _weaponSlots[_curWeaponType] = _currentWeapon;
        }*/

        //필요 한것들
        //이전무기 끄기 다음무기 켜기 
     }

    public Weapon GetWeapon(WeaponType weaponType)
    {
        return _weaponSlots[weaponType];
    }

}
