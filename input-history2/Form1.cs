using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimDX.XInput;
using SlimDX.DirectInput;
using System.IO;
using System.Reflection;

namespace input_history2
{
    public partial class Form1 : Form
    {
        Controller controller = new Controller(UserIndex.One);
        //State state;
        //GamepadButtonFlags GamepadButtonFlags;
        public Pad pad = new Pad();

        DirectInput Input = new DirectInput();
        Joystick stick;
        //Joystick[] Sticks;

        string directions = "";
        List<char> direction = new List<char>();
        string butttons = "";
        List<string> buttton = new List<string>();
        

        public class Pad
        {
            public bool Dup = false;
            public bool Ddown = false;
            public bool Dleft = false;
            public bool Dright = false;
            public bool one = false;
            public bool two = false;
            public bool three = false;
            public bool four = false;
            public bool stateChanged = false;
            public bool DstateChanged = false;//dpad state changed
            public bool BstateChanged = false;//buttons state changed
            public bool BindstateChanged = false;
            public bool[] binds = new bool[7];//if bind #i from ids is pressed
            public bool[] buttons;//every button state
            public int[] ids= new int[7];//size 7

            private bool mem = false;

            public Pad()
            {
                for(int i = 0; i < 7; ++i)
                {
                    binds[i] = false;
                    ids[i] = -1;
                }


                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Binds.txt");
                if (File.Exists(path))
                {
                    StreamReader stream = new StreamReader(path);
                    for (int i = 0; i < 7; ++i)
                    {
                        ids[i] = Convert.ToInt32(stream.ReadLine());
                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    StreamWriter stream = new StreamWriter(path, false);
                    for(int i = 0; i < 7; ++i)
                    {
                        stream.WriteLine(-1);
                    }
                    stream.Close();
                    stream.Dispose();
                }
            }

            public bool BindsPressed(Joystick stickk)
            {
                JoystickState state1 = new JoystickState();
                state1 = stickk.GetCurrentState();
                buttons = state1.GetButtons();

                for (int i = 0; i < 7; ++i)
                {
                    if(binds[i]==true){ return true; }
                }
                return false;
            }

            public int searchid(int[] arr, int id)
            {
                for (int i = 0; i < arr.Length; ++i)
                {
                    if (arr[i] == id) return i;
                }
                return -1;
            }

            public void Checkstate(Controller controller, Joystick stickk)
            {
                State state = controller.GetState();
                GamepadButtonFlags gamepadButtonFlags = state.Gamepad.Buttons;
                stateChanged = false;
                DstateChanged = false;
                BstateChanged = false;
                BindstateChanged = false;

                JoystickState state1 = new JoystickState();
                state1 = stickk.GetCurrentState();
                buttons = state1.GetButtons();

                mem = Dup;
                if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.DPadUp)) Dup = true;
                else Dup = false;
                if (mem != Dup) { stateChanged = true;DstateChanged = true; }

                mem = Ddown;
                if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.DPadDown)) Ddown = true;
                else Ddown = false;
                if (mem != Ddown) { stateChanged = true; DstateChanged = true; }

                mem = Dleft;
                if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.DPadLeft)) Dleft = true;
                else Dleft = false;
                if (mem != Dleft) { stateChanged = true; DstateChanged = true; }

                mem = Dright;
                if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.DPadRight)) Dright = true;
                else Dright = false;
                if (mem != Dright) { stateChanged = true; DstateChanged = true; }

                mem = three;
                if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.A)) three = true;
                else three = false;
                if (mem != three) { stateChanged = true; BstateChanged = true; }

                mem = four;
                if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.B)) four = true;
                else four = false;
                if (mem != four) { stateChanged = true; BstateChanged = true; }

                mem = one;
                if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.X)) one = true;
                else one = false;
                if (mem != one) { stateChanged = true; BstateChanged = true; }

                mem = two;
                if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.Y)) two = true;
                else two = false;
                if (mem != two) { stateChanged = true; BstateChanged = true; }

                for (int i = 0; i < buttons.Length; ++i)
                {
                    if (ids.Contains(i)) { mem = binds[searchid(ids, i)]; if (buttons[i] == true) { binds[searchid(ids, i)] = true; } else { binds[searchid(ids, i)] = false; } if (mem != binds[searchid(ids, i)]) { stateChanged = true; BindstateChanged = true; } }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < 20; ++i)
            {
                direction.Insert(0, ' ');
                buttton.Insert(0, " ");
            }
        }

        //public int searchid(int[] arr, int id)
        //{
        //    for(int i = 0; i < arr.Length; ++i)
        //    {
        //        if (arr[i] == id) return id;
        //    }
        //    return -1;
        //}

        public bool equal(List<string> list1, List<string> list2)
        {
            for(int i = 0; i < list1.Count; ++i)
            {
                if (list1[i] != list2[i]) return false;
            }
            return true;
        }

        static private bool CheckForm()
        {
            DialogResult dialogResult = MessageBox.Show("Подтверждение действия", "Подтверждение действия", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                return true;
            }
            else if (dialogResult == DialogResult.No)
            {
                return false;
            }
            return false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //state = controller.GetState();
            //GamepadButtonFlags = state.Gamepad.Buttons;

            //label1.Text = Convert.ToString(GamepadButtonFlags.HasFlag(GamepadButtonFlags.DPadUp));
            //label2.Text = Convert.ToString(GamepadButtonFlags.HasFlag(GamepadButtonFlags.DPadDown));

            GC.Collect();

            List<string> temp = new List<string>();
            for (int i = 0; i < buttton.Count; ++i)
            {
                temp.Add(buttton[i]);
            }

            pad.Checkstate(controller,stick);



            if (pad.Dleft)
            {
                if (!pad.Dup&&!pad.Ddown)
                {
                    if (direction[0] != '←')
                    {
                        direction.Insert(0, '←');
                        direction.RemoveAt(direction.Count - 1);
                    }
                }
                else if (pad.Dup)
                {
                    if (direction[0] != '↖')
                    {
                        direction.Insert(0, '↖');
                        direction.RemoveAt(direction.Count - 1);
                    }
                }
                else
                {
                    if (direction[0] != '↙')
                    {
                        direction.Insert(0, '↙');
                        direction.RemoveAt(direction.Count - 1);
                    }
                }
            }
            else if (pad.Dright)
            {
                if (!pad.Dup && !pad.Ddown)
                {
                    if (direction[0] != '→')
                    {
                        direction.Insert(0, '→');
                        direction.RemoveAt(direction.Count - 1);
                    }
                }
                else if (pad.Dup)
                {
                    if (direction[0] != '↗')
                    {
                        direction.Insert(0, '↗');
                        direction.RemoveAt(direction.Count - 1);
                    }
                }
                else
                {
                    if (direction[0] != '↘')
                    {
                        direction.Insert(0, '↘');
                        direction.RemoveAt(direction.Count - 1);
                    }
                }
            }
            else if (pad.Ddown)
            {
                if (direction[0] != '↓')
                {
                    direction.Insert(0, '↓');
                    direction.RemoveAt(direction.Count - 1);
                }
            }
            else if (pad.Dup)
            {
                if (direction[0] != '↑')
                {
                    direction.Insert(0, '↑');
                    direction.RemoveAt(direction.Count - 1);
                }
            }

            if (!pad.Dup&&!pad.Ddown&&!pad.Dright&&!pad.Dleft)
            {
                if (direction[0] != '☆')
                {
                    direction.Insert(0, '☆');
                    direction.RemoveAt(direction.Count - 1);
                }
            }


            if (pad.BindsPressed(stick))//1+2 1+3 1+4 2+3 2+4 3+4 1+2+3+4
            {
                if(pad.binds[6])
                {
                    if (buttton[0] != "1+2+3+4")
                    {
                        buttton.Insert(0, "1+2+3+4");
                        buttton.RemoveAt(buttton.Count - 1);
                    }
                }else if (pad.binds[0])//1+2 !all
                {
                    if (pad.binds[5])//1+2+3+4  !all
                    {
                        if (buttton[0] != "1+2+3+4")
                        {
                            buttton.Insert(0, "1+2+3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else if (pad.three||pad.binds[1]||pad.binds[3])//1+2+3  !all
                    {
                        if (pad.four|| pad.binds[4]|| pad.binds[2])//1+2+3+4  !all
                        {
                            if (buttton[0] != "1+2+3+4")
                            {
                                buttton.Insert(0, "1+2+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                        else//1+2+3
                        {
                            if (buttton[0] != "1+2+3")
                            {
                                buttton.Insert(0, "1+2+3");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                    }
                    else if (pad.four||pad.binds[4]|| pad.binds[2])//1+2+4 !3 !1+3 !2+3 !3+4 !all
                    {
                        if (buttton[0] != "1+2+4")
                        {
                            buttton.Insert(0, "1+2+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else//1+2 !3 !4  !all
                    {
                        if (buttton[0] != "1+2")
                        {
                            buttton.Insert(0, "1+2");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                }else if (pad.binds[1])//1+3 !1+2  !all
                {
                    if (pad.binds[4])//1+2+3+4 !1+2  !all
                    {
                        if (buttton[0] != "1+2+3+4")
                        {
                            buttton.Insert(0, "1+2+3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }else if (pad.four|| pad.binds[2]|| pad.binds[5])//1+3+4 !1+2 !2+4 !all
                    {
                        if(pad.two|| pad.binds[3])//1+2+3+4 !1+2 !2+4 !all
                        {
                            if (buttton[0] != "1+2+3+4")
                            {
                                buttton.Insert(0, "1+2+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                        else if (buttton[0] != "1+3+4")
                        {
                            buttton.Insert(0, "1+3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else if (pad.two || pad.binds[3])//1+2+3 !4 !1+2 !1+4 !2+4 !3+4 !all 
                    {
                        if (buttton[0] != "1+2+3")
                        {
                            buttton.Insert(0, "1+2+3");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }else//1+3
                    {
                        if (buttton[0] != "1+3")
                        {
                            buttton.Insert(0, "1+3");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                }
                else if (pad.binds[2])//1+4 !1+2 !1+3  !all
                {
                    if(pad.binds[3])//1+2+3+4
                    {
                        if (buttton[0] != "1+2+3+4")
                        {
                            buttton.Insert(0, "1+2+3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }else if(pad.two||pad.binds[4])//1+2+4 !1+2 !1+3 !2+3 !all
                    {
                        if (pad.three || pad.binds[5])//1+2+3+4
                        {
                            if (buttton[0] != "1+2+3+4")
                            {
                                buttton.Insert(0, "1+2+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                        else
                        {
                            if (buttton[0] != "1+2+4")
                            {
                                buttton.Insert(0, "1+2+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                    }else if(pad.three||pad.binds[5])//1+3+4 !2 !1+2 !1+3 !2+3 !2+4 !all
                    {
                        if (buttton[0] != "1+3+4")
                        {
                            buttton.Insert(0, "1+3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else
                    {
                        if (buttton[0] != "1+4")
                        {
                            buttton.Insert(0, "1+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                }
                else if (pad.binds[3])//2+3 !1+2 !1+3 !1+4 !all
                {
                    if(pad.one)//1+2+3 !1+2 !1+3 !1+4 !all
                    {
                        if(pad.four||pad.binds[4] || pad.binds[5])//1+2+3+4
                        {
                            if (buttton[0] != "1+2+3+4")
                            {
                                buttton.Insert(0, "1+2+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                        else
                        {
                            if (buttton[0] != "1+2+3")
                            {
                                buttton.Insert(0, "1+2+3");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                    }else if(pad.four || pad.binds[4] || pad.binds[5])//2+3+4 !1 !1+2 !1+3 !1+4 !all
                    {
                        if (buttton[0] != "2+3+4")
                        {
                            buttton.Insert(0, "2+3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else
                    {
                        if (buttton[0] != "2+3")
                        {
                            buttton.Insert(0, "2+3");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                }
                else if (pad.binds[4])//2+4 !1+2 !1+3 !1+4 !2+3 !all
                {
                    if(pad.one)//1+2+4 !1+2 !1+3 !1+4 !2+3 !all
                    {
                        if(pad.three || pad.binds[5])//1+2+3+4 !1+2 !1+3 !1+4 !2+3 !all
                        {
                            if (buttton[0] != "1+2+3+4")
                            {
                                buttton.Insert(0, "1+2+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                        else
                        {
                            if (buttton[0] != "1+2+4")
                            {
                                buttton.Insert(0, "1+2+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                    }
                    else if(pad.three||pad.binds[5])//2+3+4 !1 !1+2 !1+3 !1+4 !2+3 !all
                    {
                        if (buttton[0] != "2+3+4")
                        {
                            buttton.Insert(0, "2+3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else
                    {
                        if (buttton[0] != "2+4")
                        {
                            buttton.Insert(0, "2+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                }
                else if (pad.binds[5])//3+4 !1+2 !1+3 !1+4 !2+3 !2+4 !all
                {
                    if(pad.one)//1+3+4 !1+2 !1+3 !1+4 !2+3 !2+4 !all
                    {
                        if (pad.two)//1+2+3+4
                        {
                            if (buttton[0] != "1+2+3+4")
                            {
                                buttton.Insert(0, "1+2+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                        else
                        {
                            if (buttton[0] != "1+3+4")
                            {
                                buttton.Insert(0, "1+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                    }else if(pad.two)//2+3+4 !1 !1+2 !1+3 !1+4 !2+3 !2+4 !all
                    {
                        if (buttton[0] != "2+3+4")
                        {
                            buttton.Insert(0, "2+3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else
                    {
                        if (buttton[0] != "3+4")
                        {
                            buttton.Insert(0, "3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                }
            }
            else
            {
                if (pad.one)//1
                {
                    if (pad.two)//1+2
                    {
                        if (pad.three)//1+2+3
                        {
                            if (pad.four)//1+2+3+4
                            {
                                if (buttton[0] != "1+2+3+4")
                                {
                                    buttton.Insert(0, "1+2+3+4");
                                    buttton.RemoveAt(buttton.Count - 1);
                                }
                            }
                            else//1+2+3
                            {
                                if (buttton[0] != "1+2+3")
                                {
                                    buttton.Insert(0, "1+2+3");
                                    buttton.RemoveAt(buttton.Count - 1);
                                }
                            }
                        }
                        else if (pad.four)//1+2+4 !3
                        {
                            if (buttton[0] != "1+2+4")
                            {
                                buttton.Insert(0, "1+2+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                        else//1+2 !3 !4
                        {
                            if (buttton[0] != "1+2")
                            {
                                buttton.Insert(0, "1+2");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                    }
                    else if (pad.three)//1+3 !2
                    {
                        if (pad.four)//1+3+4
                        {
                            if (buttton[0] != "1+3+4")
                            {
                                buttton.Insert(0, "1+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                        else//1+3
                        {
                            if (buttton[0] != "1+3")
                            {
                                buttton.Insert(0, "1+3");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                    }
                    else if (pad.four)//1+4 !2 !3
                    {
                        if (buttton[0] != "1+4")
                        {
                            buttton.Insert(0, "1+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else//1
                    {
                        if (buttton[0] != "1")
                        {
                            buttton.Insert(0, "1");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                }
                else if (pad.two)//2 !1
                {
                    if (pad.three)//2+3 !1
                    {
                        if (pad.four)//2+3+4
                        {
                            if (buttton[0] != "2+3+4")
                            {
                                buttton.Insert(0, "2+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                        else//2+3
                        {
                            if (buttton[0] != "2+3")
                            {
                                buttton.Insert(0, "2+3");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                    }
                    else if (pad.four)//2+4 !1 !3
                    {
                        if (buttton[0] != "2+4")
                        {
                            buttton.Insert(0, "2+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else//2
                    {
                        if (buttton[0] != "2")
                        {
                            buttton.Insert(0, "2");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                }
                else if (pad.three)//3 !2 !1
                {
                    if (pad.four)//3+4
                    {
                        if (buttton[0] != "3+4")
                        {
                            buttton.Insert(0, "3+4");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                    else//3
                    {
                        if (buttton[0] != "3")
                        {
                            buttton.Insert(0, "3");
                            buttton.RemoveAt(buttton.Count - 1);
                        }
                    }
                }
                else if (pad.four)//4 !3 !2 !1
                {
                    if (buttton[0] != "4")
                    {
                        buttton.Insert(0, "4");
                        buttton.RemoveAt(buttton.Count - 1);
                    }
                }
                else
                {
                    if (buttton[0] != "☆")
                    {
                        buttton.Insert(0, "☆");
                        buttton.RemoveAt(buttton.Count - 1);
                    }
                }
            }

            if (pad.stateChanged)
            {
                if (pad.DstateChanged)
                {
                    label1.Text = listtostring(direction);
                    buttton.Insert(0, buttton[0]);
                    buttton.RemoveAt(buttton.Count - 1);
                    label2.Text = strlisttostr(buttton);
                }
                else if (pad.BindsPressed(stick))
                {
                    if (!equal(temp, buttton))
                    {
                        label2.Text = strlisttostr(buttton);
                        direction.Insert(0, direction[0]);
                        direction.RemoveAt(direction.Count - 1);
                        label1.Text = listtostring(direction);
                    }
                }
                else if (pad.BstateChanged)
                {
                    label2.Text = strlisttostr(buttton);
                    direction.Insert(0, direction[0]);
                    direction.RemoveAt(direction.Count - 1);
                    label1.Text = listtostring(direction);
                }
            }

            //if (pad.BindsPressed(stick)) label1.Text = string.Join(".", pad.buttons);
        }

        public string listtostring(List<char> list)
        {
            string test = "";
            for (int i = 0; i < list.Count; ++i)
            {
                test += "   "+list[i];
            }
            return test;
        }

        public string strlisttostr(List<string> list)
        {
            string test = "";
            for (int i = 0; i < list.Count; ++i)
            {
                test += " " + list[i];
            }
            return test;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F2")
            {//hand over array of buttons for binds
                Form2 form = new Form2(this);
                form.ShowDialog();
            }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            Joystick[] joystick = GetSticks();
        }
    }
}
