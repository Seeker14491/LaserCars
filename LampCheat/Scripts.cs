using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LampCheat
{
    class Scripts
    {
        public class LampController : MonoBehaviour
        {
            Material lamp_light_material = null;
            Material lamp_panel_material = null;
            Light lamp_light = null;

            Color color = Color.white;
            
            CarColors car_colors;

            public GameObject lamp = null;

            CarLogic car_logic = null;

            void Start()
            {
                lamp = MakeLamp();
                lamp.transform.parent = transform.parent;

                foreach (Collider collider in lamp.GetComponentsInChildren<MeshCollider>())
                    collider.Destroy();
                foreach (MeshRenderer renderer in lamp.GetComponentsInChildren<MeshRenderer>())
                    foreach (Material material in renderer.materials)
                        switch(material.name.ToLower().Split(' ')[0])
                        {
                            case "empire_light_strip":
                                lamp_light_material = material;
                                break;
                            case "empire_panel":
                                lamp_panel_material = material;
                                break;
                        }
   
                lamp.GetComponentInChildren<LensFlareFadeWithDistance>().Destroy();
                lamp_light = lamp.GetComponentInChildren<Light>();

                lamp_light.intensity = 0.75f;
                lamp_light.range = 20;

                lamp.SetActive(Entry.GetKey("lamp.cheat", false));

                Events.Scene.LoadFinish.Subscribe(OnSceneLoaded);
            }

            CustomizeCarColorsMenuLogic customize_menu;
            string scene_name = "";
            void OnSceneLoaded(Events.Scene.LoadFinish.Data data)
            {
                customize_menu = FindObjectOfType<CustomizeCarColorsMenuLogic>();
                scene_name = SceneManager.GetActiveScene().name;
            }

            void LateUpdate()
            {
                car_colors = G.Sys.ProfileManager_.CurrentProfile_.CarColors_;

                if (customize_menu != null && (customize_menu.editColorsPanel_.IsTop_ || customize_menu.colorPickerPanel_.IsTop_ || customize_menu.colorPresetPanel_.IsTop_))
                    car_colors = customize_menu.colorPickerPanel_.IsTop_ ? customize_menu.pickerCarColor_.Colors_ : customize_menu.CurrentCarColor_.Colors_;

                if (scene_name != "MainMenu" && car_logic != null)
                    car_colors = car_logic.PlayerData_.originalColors_.Colors_;

                color = car_colors.glow_;
                color.a = 255;
                lamp_light_material.SetColor("_EmitColor", color);
                lamp_light.color = color;

                color = car_colors.primary_;
                color.a = 255;
                lamp_panel_material.SetColor("_Color", color);
                lamp_panel_material.SetColor("_SpecColor", color);


                //lamp.transform.position = transform.position;
                //lamp.transform.rotation = transform.rotation;

                
            }

            GameObject MakeLamp()
            {
                GameObject lampprefab = Resources.Load<GameObject>("Prefabs/LevelEditor/Decorations/Empirelamp");

                GameObject lamp = Instantiate(lampprefab, transform.position, transform.rotation);
                lamp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                return lamp;
            }

            public void SetCarLogic(CarLogic value)
            {
                car_logic = value;
            }
        }
    }
}
