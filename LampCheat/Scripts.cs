using System.Linq;
using UnityEngine;

namespace LampCheat
{
    class Scripts
    {
        public class LampController : MonoBehaviour
        {
            Material light_material = null;
            Material panel_material = null;
            Color color = Color.white;
            Light light = null;

            CarColors car_colors;

            void Start()
            {
                foreach (Collider collider in gameObject.GetComponentsInChildren<MeshCollider>())
                    collider.Destroy();
                foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
                    foreach (Material material in renderer.materials)
                        switch(material.name.ToLower().Split(' ')[0])
                        {
                            case "empire_light_strip":
                                light_material = material;
                                break;
                            case "empire_panel":
                                panel_material = material;
                                break;
                        }
   
                GetComponentInChildren<LensFlareFadeWithDistance>().Destroy();
                light = GetComponentInChildren<Light>();

                light.intensity = 0.75f;
                light.range = 20;
            }


            void Update()
            {
                car_colors = G.Sys.ProfileManager_.CurrentProfile_.CarColors_;

                CustomizeCarColorsMenuLogic menu = FindObjectOfType<CustomizeCarColorsMenuLogic>();
                if (menu != null && (menu.editColorsPanel_.IsTop_ || menu.colorPickerPanel_.IsTop_ || menu.colorPresetPanel_.IsTop_))
                    car_colors = menu.colorPickerPanel_.IsTop_ ? menu.pickerCarColor_.Colors_ : menu.CurrentCarColor_.Colors_;

                color = car_colors.glow_;
                color.a = 255;
                light_material.SetColor("_EmitColor", color);
                light.color = color;


                color = car_colors.primary_;
                color.a = 255;
                panel_material.SetColor("_Color", color);
                panel_material.SetColor("_SpecColor", color);


                foreach (GameObject car_object in from car in FindObjectsOfType<CarVisuals>().Cast<CarVisuals>() where car.isActiveAndEnabled select car.gameObject)
                {
                    transform.position = car_object.transform.position;
                    transform.rotation = car_object.transform.rotation;
                }
            }
        }
    }
}
