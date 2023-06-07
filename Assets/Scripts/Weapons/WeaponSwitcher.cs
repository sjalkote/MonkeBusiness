using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    // Start is called before the first frame update

    public int selectedWeapon;

    private void Start()
    {
        SelectedWeapon();
    }

    // Update is called once per frame
    private void Update()
    {
        var previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (previousSelectedWeapon != selectedWeapon) SelectedWeapon();
    }

    private void SelectedWeapon()
    {
        var i = 0;

        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);

            i++;
        }
    }
}