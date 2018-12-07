using System;
using System.IO.IsolatedStorage;
using MilitaryCombatSimulator;
//using UnityEditor;
using UnityEngine;
using System.Collections;

/// <summary>
/// ���������� ������� ���������
/// </summary>
public class MCSSimulationHandler : MonoBehaviour
{

    public GameObject Cloud;

    void OnPlayerConnected(NetworkPlayer player)
    {
        if (Network.isServer)
        {
            // �������������� ����� ���������
            MCSGlobalSimulation.CommandCenter.Execute(player, new MCSCommand(MCSCommandType.Simulation, "SynhronizeEnvironmentTime", false,
                                                MCSGlobalEnvironment.UniSky.TIME));
        }
    }

    public void Execute(NetworkPlayer sender, MCSCommand cmd)
	{
		Weaponry weaponry;

        switch (cmd.Command)
        {
            case "Instantiate":
                string type = cmd.Args[0].ToString(); // ��� �������
                // ���� ������ - ����������� ViewID
                if (Network.isClient)
                {
                    MCSGlobalSimulation.ClientInstantiate((int) cmd.Args[1], type);
                }
                else // ���� ������� �� ������, ������� ������ � ������ - �������� ViewID
                {
                    MCSGlobalSimulation.Instantiate(type);
                }
                break;
                
            // ������ ��������, ��� Weaponry ��������
            case "WeaponryPosted":
                if (Network.isServer)
                {
                    
                    //Debug.Log("Weaponry [" + cmd.Args[0] + "] Posted by " + sender);

                    // ���������� � Weaponry �� ��� ID
                    if (MCSGlobalSimulation.Weapons.List.ContainsKey((int) cmd.Args[0]))
                    {
                        try
                        {
                            // �������� ������ NetworkView �� ���
                            NetworkView[] nwlist =
                                MCSGlobalSimulation.Weapons.List[(int) cmd.Args[0]].gameObject.GetComponentsInChildren
                                    <NetworkView>();

                            // ��� ������� networkView - �������� �� ������ ��� viewID � �������� ���
                            foreach (NetworkView networkView in nwlist)
                            {
                                if (networkView.observed is NetworkInterpolatedRotation || networkView.observed is NetworkInterpolatedTransform)
                                {
                                    if (networkView.gameObject.name.Contains("Core")) continue;

                                    MCSGlobalSimulation.CommandCenter.networkView.RPC("RPC_AssignViewID", sender,
                                                                                      (int) cmd.Args[0],
                                                                                      networkView.viewID);
                                    //Debug.Log("���� ������� [" + sender + "] c wID [" + (int) cmd.Args[0] + "] viewID " +
                                    //          networkView.viewID + " c go [" + networkView.gameObject.name + "]");
                                    networkView.enabled = true;
                                    networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
                                }
                            }

                            //MCSGlobalSimulation.CommandCenter.Execute(new MCSCommand(MCSCommandType.Weaponry, "SetRole", false, new Strela10_Operator_PanelLibrary().GetName()));
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("������ ��� ��������� ������� [WeaponryPosted]:" + e.Message);
                        }
                        //OnWeaponryInstantiatedEvent(MCSGlobalSimulation.Weapons.List[(int)cmd.Args[0]]);
                    }
                }
                break;

            case "WeaponrySynchronized":
                int id = (int)cmd.Args[0];


                if (Network.isServer)
                {

                        if (MCSGlobalSimulation.Weapons.Synchronizations.ContainsKey(id))
                            MCSGlobalSimulation.Weapons.Synchronizations[id]++;
                        else MCSGlobalSimulation.Weapons.Synchronizations[id] = 1;

                        weaponry = MCSGlobalSimulation.Weapons.List[id];
                        // ���� ������ �� ���� ����� ���
                        if (MCSGlobalSimulation.Weapons.Synchronizations[id] == Network.connections.Length)
						{
                            weaponry.OnWeaponryInstantiate(); // �������� ������� �������������

                            if (weaponry.ImplementedBy<IWeaponryControl>()) // ���� ��������� ������ ���������
                            {
                                foreach (var crew in (weaponry as IWeaponryControl).Crew)
                                {
                                    if (crew.Value != null)
                                    {
                                        Debug.Log("����� [" + crew.Key + "] - [" + crew.Value + "]");
                                        (weaponry as IWeaponryControl).SetRole(crew.Key, crew.Value);

                                    }
                                }

                                // �������� ���������� � ������� ����������
                                MCSGlobalSimulation.CommandCenter.WeaponryHandler.SynchronizeWeaponryControls(weaponry);
                            }
                            else {

                            }

                            weaponry.gameObject.GetComponent<NetworkInterpolatedTransform>()._oldPosition = Vector3.zero;
                        }
                }
                else
                {
                    try
                    {
                        weaponry = MCSGlobalSimulation.Weapons.List[id]; 
                        weaponry.OnWeaponryInstantiate();
                    }
                    catch (Exception)
                    {
                        Debug.Log("�� ������� ���������� Weaponry c ID [" + (int)cmd.Args[0] + "]");
                    }
                }
                break;

            case "SynhronizeEnvironmentTime":
                MCSGlobalEnvironment.UniSky.TIME = (float) cmd.Args[0];
                break;

			case "DestroyWeaponry":
		        weaponry = MCSGlobalSimulation.Weapons.List[(int) cmd.Args[0]];

				if (weaponry != null)
				{
					foreach (var player in weaponry.GetOwners())
					{
						player.Weaponry = null;
						player.Role = null;
					}

					Network.Destroy(weaponry.gameObject);
				}

		        break;

			case "ClientInstantiateRequest":
		        if (Network.isServer)
		        {
			        if ((int) cmd.Args[1] == -1)
			        {
				        weaponry = MCSGlobalSimulation.Instantiate(cmd.Args[0].ToString());
			        }
			        else weaponry = MCSGlobalSimulation.Weapons.List[(int) cmd.Args[1]];

			        if (weaponry != null)
			        {
				        var ti = weaponry.GetComponent<MCSTrainingBehaviour>();
				        IWeaponryControl iwc = (IWeaponryControl) weaponry;
				        MCSTrainingOrder trainingInfo = null;

                        Transform placeholder = GameObject.Find("Placeholder").transform;
                        weaponry.transform.position = placeholder.position;
                        placeholder.position += Vector3.right * weaponry.collider.bounds.size.x * 2f;

				        weaponry.OnWeaponryInstantiated += delegate()
						{
						        if (weaponry.ImplementedBy<IWeaponryControl>())
						        {
							        // ��������� ������ � ������
							        MCSTrainingCenter.InitializeTraining(weaponry, MCSGlobalSimulation.Players.List[sender], trainingInfo);
						        }
						        else Debug.LogError("Weaponry �� ��������� ��������� IWeaponryControl");

					        };

				        foreach (var inf in ti.GetInfoObject().GetAllOrders())
				        {
					        if (inf.OrderName.Equals(cmd.Args[2].ToString()))
					        {
						        trainingInfo = inf;
						        iwc.SetRole(sender, inf.PerformerName); // ��������� ����� � �����
					        }
				        }
			            if (trainingInfo != null && trainingInfo.OrderName.Equals("��������. ��������"))
			            {
			                Cloud = Resources.Load("Target_Cloud") as GameObject;
			                Instantiate(Cloud);
			            }
			        }
			        else Debug.LogError("There is no weaponry with id [" + (int) cmd.Args[1] + "]");


		        }

		        break;

			

            default:
                Debug.Log("������� [" + cmd.Command + "], ����������� � [" + this.GetType() + "] �� ����������");
                break;
        }

    }
}
