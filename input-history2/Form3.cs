using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SlimDX.DirectInput;

namespace input_history2
{
    public partial class Form3 : Form
    {
        DirectInput Input = new DirectInput();
        Joystick stick;

        Form2 parentForm;
        bool[] buttons;
        string bind = "";

        public Form3(string str,Form2 form)
        {
            InitializeComponent();
            bind = str;
            parentForm = form;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Joystick[] joystick = GetSticks();

            label1.Text = "Press the button for " +bind;
        }

        public Joystick[] GetSticks()
        {

            List<SlimDX.DirectInput.Joystick> sticks = new List<SlimDX.DirectInput.Joystick>(); // Creates the list of joysticks connected to the computer via USB.

            foreach (DeviceInstance device in Input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                // Creates a joystick for each game device in USB Ports
                try
                {
                    stick = new SlimDX.DirectInput.Joystick(Input, device.InstanceGuid);
                    stick.Acquire();

                    // Gets the joysticks properties and sets the range for them.
                    foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                    {
                        if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                            stick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-100, 100);
                    }

                    // Adds how ever many joysticks are connected to the computer into the sticks list.
                    sticks.Add(stick);
                }
                catch (DirectInputException)
                {
                }
            }
            Console.WriteLine(sticks.Count);
            return sticks.ToArray();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            JoystickState state1 = new JoystickState();
            state1 = stick.GetCurrentState();
            buttons = state1.GetButtons();
            for(int i = 0; i < buttons.Length; ++i)
            {
                if (buttons[i])
                {
                    switch (bind)
                    {
                        case "1+2":
                            parentForm.ids[0] = i;
                            break;
                        case "1+3":
                            parentForm.ids[1] = i;
                            break;
                        case "1+4":
                            parentForm.ids[2] = i;
                            break;
                        case "2+3":
                            parentForm.ids[3] = i;
                            break;
                        case "2+4":
                            parentForm.ids[4] = i;
                            break;
                        case "3+4":
                            parentForm.ids[5] = i;
                            break;
                        case "1+2+3+4":
                            parentForm.ids[6] = i;
                            break;
                        case "1":
                            parentForm.ids[7] = i;
                            break;
                        case "2":
                            parentForm.ids[8] = i;
                            break;
                        case "3":
                            parentForm.ids[9] = i;
                            break;
                        case "4":
                            parentForm.ids[10] = i;
                            break;
                    }
                    this.Close();
                }
            }
            
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            GC.Collect();
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F3")
            {
                this.Close();
            }
            if (e.KeyCode.ToString() == "F4")
            {
                switch (bind)
                {
                    case "1+2":
                        parentForm.ids[0] = -1;
                        break;
                    case "1+3":
                        parentForm.ids[1] = -1;
                        break;
                    case "1+4":
                        parentForm.ids[2] = -1;
                        break;
                    case "2+3":
                        parentForm.ids[3] = -1;
                        break;
                    case "2+4":
                        parentForm.ids[4] = -1;
                        break;
                    case "3+4":
                        parentForm.ids[5] = -1;
                        break;
                    case "1+2+3+4":
                        parentForm.ids[6] = -1;
                        break;
                    case "1":
                        parentForm.ids[7] = -1;
                        break;
                    case "2":
                        parentForm.ids[8] = -1;
                        break;
                    case "3":
                        parentForm.ids[9] = -1;
                        break;
                    case "4":
                        parentForm.ids[10] = -1;
                        break;
                }
                this.Close();
            }
        }
    }
}
