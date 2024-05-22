using UnityEngine;
using UnityEngine.Events;

public class CombatController : MonoBehaviour
{
    public UnityEvent<CombatModel.WeaponType> OnWeaponChange = new UnityEvent<CombatModel.WeaponType>();
    public UnityEvent OnAttack = new UnityEvent();

    private CombatModel combatModel;

    void Start()
    {
        combatModel = new CombatModel();
        InitializeEvents();
    }

    void Update()
    {
        HandleCombatInput();
    }

    private void InitializeEvents()
    {
        OnWeaponChange.AddListener(HandleWeaponChange);
        OnAttack.AddListener(PerformAttack);
    }

    private void HandleCombatInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnWeaponChange?.Invoke(CombatModel.WeaponType.Fist);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnWeaponChange?.Invoke(CombatModel.WeaponType.Trumpet);
        }
        if (Input.GetMouseButtonDown(0)) // Left mouse button for attack
        {
            OnAttack?.Invoke();
        }
    }

    private void HandleWeaponChange(CombatModel.WeaponType weapon)
    {
        combatModel.SetWeapon(weapon);
    }

    public void PerformAttack()
    {
        CombatModel.WeaponType currentWeapon = combatModel.GetCurrentWeapon();
        Debug.Log("Performing attack with weapon: " + currentWeapon);
    }

    public CombatModel.WeaponType GetCurrentWeapon()
    {
        return combatModel.GetCurrentWeapon();
    }
}
