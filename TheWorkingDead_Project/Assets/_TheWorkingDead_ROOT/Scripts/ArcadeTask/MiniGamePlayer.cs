using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class MiniGamePlayer : MonoBehaviour
{

    public float speed = 5f;
    public float minX = -4f;
    public float maxX = 4f;
    bool canshoot=true;
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Update()
    {
        Move();
    }

    void Move()
    {
        float input = 0f;
        
        if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A))
            input = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D))
            input = 1f;
        
        Vector3 pos = transform.localPosition;
        pos.x += input * speed * Time.unscaledDeltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.localPosition = pos;
    }

    void Shoot()
    {
        if (canshoot) 
        {
            canshoot = false;
            StartCoroutine(CanShootCoroutine());
            GameObject bullet = Instantiate(bulletPrefab);

            bullet.transform.SetParent(firePoint.parent, false);
            
            RectTransform bulletRect = bullet.GetComponent<RectTransform>();
            RectTransform fireRect = firePoint.GetComponent<RectTransform>();
            if (bulletRect != null && fireRect != null)
            {
                bulletRect.anchoredPosition = fireRect.anchoredPosition;
            }

            bullet.transform.SetSiblingIndex(firePoint.GetSiblingIndex() + 1);
        }
    }
    IEnumerator CanShootCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        canshoot = true;
    }
}
