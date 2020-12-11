using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LaserCars
{
    [UsedImplicitly]
    internal class Scripts
    {
        public class LaserController : MonoBehaviour
        {
            internal GameObject Laser;

            private void Update()
            {
                if (transform.parent.name != "MyCarSplit") return;

                var laser = transform.parent.transform.Find("CarLaser");

                if (laser != null)
                {
                    Destroy(laser.gameObject);
                }
                
                Destroy(this);
            }

            private void Start()
            {
                var existingLaser = transform.parent.Find("CarLaser");
                if (existingLaser != null)
                {
                    return;
                }
                
                var laserPrefab =
                    Resources.Load<GameObject>("Prefabs/LevelEditor/Obstacles/Infected/VirusLaserCannonHead");
                
                Laser = Instantiate(laserPrefab, transform);
                Laser.name = "CarLaser";

                var collider = Laser.GetComponentInChildren<MeshCollider>(true);
                DestroyImmediate(collider);
                
                Laser.transform.Rotate(Vector3.right * 90f);
                Laser.transform.Rotate(Vector3.up * 60f);
                Laser.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                Laser.transform.localPosition += new Vector3(0.0f, 1.6f, -2.3f);
                
                Laser.transform.parent = transform.parent;

                Laser.SetActive(Entry.GetKey("lamp.cheat", false));
            }
        }
    }
}