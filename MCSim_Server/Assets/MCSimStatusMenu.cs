using System;
using MilitaryCombatSimulator;
using UnityEngine;
using System.Linq;
using System.Collections;

public class MCSimStatusMenu : MonoBehaviour {

    public GameObject ActiveList;
    public GameObject DestroyList;
    UILabel activeLabel;
    UILabel destroyLabel;
    public GameObject placeStrela; 
    String str;
    String str2="";

	// Use this for initialization
	void Start () {
        activeLabel = ActiveList.GetComponent<UILabel>();
        destroyLabel = DestroyList.GetComponent<UILabel>();
        destroyLabel.text = str2;
        placeStrela = GameObject.Find("Placeholder");
	}
	
	// Update is called once per frame


	void Update () {
        str = "";
       // try
       // {
            //   var obj=MCSGlobalSimulation.Weapons.List;
            //foreach(Key)
            GameObject[] findO = GameObject.FindGameObjectsWithTag("Weaponry");
            if (findO.Length != 0)
            {
                int i = 1;
                foreach (GameObject cnt in findO)
                {
                    Boolean cc = cnt.name.Contains("strela_10") || cnt.name.Contains("Cloud") || cnt.name.Contains("LieTarget");
                    if (!cc && !isDestroy(cnt))
                    {
                        if (isAlive(cnt))
                        {
                            str += "\n\nЦель № " + i;
                            i++;
                            str += "\nНазвание: " + cnt.name.Substring(7, cnt.name.Length - 7);
                           // str += "\nРасстояние до цели: " + Convert.ToInt32((cnt.transform.position - placeStrela.transform.position).sqrMagnitude / 1000f) + " м";
                            str += "\nРасстояние до цели: " + Vector3.Distance(cnt.transform.position,placeStrela.transform.position) + " м";
                            str += "\nСкорость цели: " + Convert.ToInt32(cnt.rigidbody.velocity.sqrMagnitude / 100f) + " м/с";
                        }
                        else
                        {
                           // Debug.LogError("ЗАшёл");
                            str2 += "\n\nЦель № " + i;
                            i++;
                            str2 += "\nНазвание: " + cnt.name.Substring(7, cnt.name.Length - 7);
                            str2 += "\nРасстояние до цели: " + Convert.ToInt32(Vector3.Distance(cnt.transform.position, placeStrela.transform.position)) + " м";
                            str2 += "\nСкорость цели: " + Convert.ToInt32(cnt.rigidbody.velocity.sqrMagnitude / 100f) + " м/с";
                            str2 += "\nВремя поражения: " + DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
                            destroyLabel.text = str2;
                           // Debug.LogError("1\n"+str2);
                            cnt.gameObject.AddComponent<WeaponryDaemonDestroy>(); 
                            
                        }
                       // Debug.LogError("2\n" + str2);
                    }

                }

            }
            else str += "Цели отсутствуют!";
      //  }
        /*catch (Exception e1)
        {
            str = "Ошибка!\n"+e1.Message;
            str2 = "Ошибка!\n" + e1.Message;
        }*/
        activeLabel.text = str;
       // Debug.LogError("3\n" + str2);
	
	}

    private bool isAlive(GameObject weaponry)
    {
        if (weaponry.GetComponent<AirCraftAI>() == null)
        {
            return false;
        }
        else return true;
    }

    private bool isDestroy(GameObject weaponry)
    {
        if (weaponry.GetComponent<WeaponryDaemonDestroy>() == null)
        {
            return false;
        }
        else return true;
    }

    public void clearListDestroy()
    {
        destroyLabel.text = "";
    }

    public void destroyAllTarget()
    {
        GameObject[] findO = GameObject.FindGameObjectsWithTag("Weaponry");

        var planes = findO.Where(w => w.GetComponent<WeaponryPlane>() != null).Select(w => w.GetComponent<WeaponryPlane>()).ToList();

        foreach (var plane in planes)
        {
            if (!plane.name.Contains("Cloud"))
            {
                MCSGlobalSimulation.Destroy(plane);
            }
        }
    }   

   
}
