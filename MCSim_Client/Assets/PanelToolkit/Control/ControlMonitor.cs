using UnityEngine;

public class ControlMonitor : MonoBehaviour {
    private PanelControl _control;

    /// <summary>
    /// Отслеживаемый контрол
    /// </summary>
    public PanelControl Control {
        get { return _control; }
        set {
            _control = value;
            switch (value.ControlType) {
                case ControlType.Tumbler:
                    _oldState = (int) value.State;
                    break;

                case ControlType.Joystick:
                    _oldState = Vector3.zero;
                    break;

                case ControlType.Spinner:
                    _oldState = (int) value.State;
                    break;
            }
        }
    }

    /// <summary>
    /// NetworkView, использующийся для синхронизации
    /// </summary>
    public new NetworkView networkView;

    private object _oldState;

    // Use this for initialization
    void Awake() {
        networkView = gameObject.AddComponent<NetworkView>();
        networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
        networkView.observed = this;
    }


    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            SerializeState(stream, Control.State);
        } else {
            Control.State = DeserializeState(stream);
        }
    }

    private void SerializeState(BitStream stream, object State) {
        switch (Control.ControlType) {
            case ControlType.Tumbler:
                int tumb = (int) State;

                if ((int) _oldState != tumb)
                    stream.Serialize(ref tumb);

                _oldState = tumb;
                break;

            case ControlType.Joystick:
                int[] args = (int[]) State;
                Vector3 vect = new Vector3(args[0], args[1], args[2]);

                if ((Vector3) _oldState != vect) {
                    stream.Serialize(ref vect);
                }

                _oldState = vect;
                break;

            case ControlType.Spinner:
                int val = (int) State;

                if ((int) _oldState != val)
                    stream.Serialize(ref val);

                _oldState = val;
                break;
        }
    }

    private object DeserializeState(BitStream stream) {
        switch (Control.ControlType) {
            case ControlType.Tumbler:
                int tumbler_val = -1;
                stream.Serialize(ref tumbler_val);
                return tumbler_val;

            case ControlType.Joystick:
                Vector3 vect = Vector3.zero;
                stream.Serialize(ref vect);
                return vect;

            case ControlType.Spinner:
                int spinner_val = -1;
                stream.Serialize(ref spinner_val);
                return spinner_val;
        }

        return null;
    }
}