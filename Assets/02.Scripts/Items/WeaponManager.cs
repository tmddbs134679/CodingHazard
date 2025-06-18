using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapon CurrentWpeaWeapon => _currentWeapon;

    
    [SerializeField] private Transform weaponAnchor;
    
    
    [SerializeField] private GameObject[] weapons;

    private int _curWeaponIndex = -1;

    private GameObject _currentWeaponGO;
    
    private Weapon _currentWeapon;
    
    private Dictionary<WeaponType, Weapon> _weaponSlots = new();

    private bool _isStart;


    private void Awake()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);

            _weaponSlots[(WeaponType)i] = weapons[i].GetComponentInChildren<Weapon>();
        }
    }


    private void Start()
    {
        PlayerEvent.Swap  += EquipWeapon;
        
        _isStart = true;

        EquipWeapon(0);
    }


    private void EquipWeapon(int index)
    {
        if (!_isStart)
        {
            AudioManager.Instance.PlayAudio(AudioID.WeaponSwap, 1f);
        }
        else
        {
            _isStart = false;
        }

        if (index == _curWeaponIndex || index >= weapons.Length)
        {
            return;
        }

        if (_currentWeaponGO != null)
        {
            _currentWeaponGO.SetActive(false);
            
            //Destroy(_currentWeaponGO);
            //_currentWeaponGO = null;
        }

        _currentWeaponGO = weapons[index];
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
