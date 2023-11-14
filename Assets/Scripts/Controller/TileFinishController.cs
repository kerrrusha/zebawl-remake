using Assets.Scripts.Util;
using UnityEngine;

public class TileFinishController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.Instance.FinishLevel();
        }
    }
}
