using System;
using System.Collections.Generic;
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

        //string directions = "";
        List<char> direction = new List<char>();
        //string butttons = "";
        List<string> buttton = new List<string>();

        int timeout = 0;

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
            public bool[] binds = new bool[11];//if bind #i from ids is pressed
            public bool[] buttons;//every button state
            public int[] ids= new int[11];//size 11
            public int zValue = 0;

            private bool mem = false;

            public Pad()
            {
                for(int i = 0; i < 11; ++i)
                {
                    binds[i] = false;
                    ids[i] = -1;
                }


                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Binds.txt");
                if (File.Exists(path))
                {
                    StreamReader stream = new StreamReader(path);
                    for (int i = 0; i < 11; ++i)
                    {
                        ids[i] = Convert.ToInt32(stream.ReadLine());
                    }
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    StreamWriter stream = new StreamWriter(path, false);

                    for(int i = 0; i < 11; ++i)
                    {
                        stream.WriteLine(-1);
                    }
                    stream.Close();
                    stream.Dispose();
                }
            }//test

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

            //public int searchid(int[] arr, int id)
            //{
            //    for (int i = 0; i < arr.Length; ++i)
            //    {
            //        if (arr[i] == id) return i;
            //    }
            //    return -1;
            //}

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
                zValue = state1.Z;
                //Console.WriteLine(zValue);

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

                if (ids[9] != -1)
                {
                    mem = three;//9
                //if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.A)) three = true;
                    if (buttons[ids[9]]) three = true;
                    else three = false;
                    if (mem != three) { stateChanged = true; BstateChanged = true; }
                }

                if (ids[10] != -1)
                {
                    mem = four;//10
                               //if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.B)) four = true;
                    if (buttons[ids[10]]) four = true;
                    else four = false;
                    if (mem != four) { stateChanged = true; BstateChanged = true; }
                }

                if (ids[7] != -1)
                {
                    mem = one;//7
                              //if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.X)) one = true;
                    if (buttons[ids[7]]) one = true;
                    else one = false;
                    if (mem != one) { stateChanged = true; BstateChanged = true; }
                }

                if (ids[8] != -1)
                {
                    mem = two;//8
                              //if (gamepadButtonFlags.HasFlag(GamepadButtonFlags.Y)) two = true;
                    if (buttons[ids[8]]) two = true;
                    else two = false;
                    if (mem != two) { stateChanged = true; BstateChanged = true; }
                }

                for (int i = 0; i < 7; ++i)
                {
                    if (ids[i] == -1) continue;

                    mem = binds[i];

                    if (ids[i] == 50)
                    {
                        if (zValue >= 50) { binds[i] = true; }
                        else { binds[i] = false; }
                    }
                    else if (ids[i] == -50)
                    {
                        if (zValue <= -50) { binds[i] = true; }
                        else { binds[i] = false; }
                    }
                    else
                    {
                        if (buttons[ids[i]] == true) { binds[i] = true; }
                        else { binds[i] = false; }
                    }
                    //Console.WriteLine(binds[i] + " " + i);

                    if (mem != binds[i]) {  stateChanged = true; BstateChanged = true; } 

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
            timeout += 1;
            if (timeout > 5000) { this.Close(); }
            //Console.WriteLine(timeout);
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
                        if ( pad.four|| pad.binds[4]|| pad.binds[2])//1+2+3+4  !all
                        {
                            if (buttton[0] != "1+2+3+4")
                            {
                                buttton.Insert(0, "1+2+3+4");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }else//1+2+3
                        {
                            if (buttton[0] != "1+2+3")
                            {
                                buttton.Insert(0, "1+2+3");
                                buttton.RemoveAt(buttton.Count - 1);
                            }
                        }
                    }
                    else if (pad.four||pad.binds[4]|| pad.binds[2])//1+2+4 !1+3 !2+3 !3+4 !all
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
                }else
                {
                    if (buttton[0] != "☆")
                    {
                        buttton.Insert(0, "☆");
                        buttton.RemoveAt(buttton.Count - 1);
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

            //todo datagridview
            if (pad.stateChanged)
            {
                if (pad.DstateChanged)
                {
                    dataGridView1.Rows.Clear();
                    //label1.Text = listtostring(direction);
                    buttton.Insert(0, buttton[0]);
                    buttton.RemoveAt(buttton.Count - 1);
                    //label2.Text = strlisttostr(buttton);

                    dataGridView1.Rows.Add(direction[0], direction[1], direction[2], direction[3], direction[4], direction[5], direction[6], direction[7], direction[8], direction[9], direction[10], direction[11], direction[12], direction[13], direction[14], direction[15], direction[16], direction[17], direction[18], direction[19]);
                    dataGridView1.Rows.Add(buttton[0], buttton[1], buttton[2], buttton[3], buttton[4], buttton[5], buttton[6], buttton[7], buttton[8], buttton[9], buttton[10], buttton[11], buttton[12], buttton[13], buttton[14], buttton[15], buttton[16], buttton[17], buttton[18], buttton[19]);
                }
                else if (pad.BindsPressed(stick))
                {
                    if (!equal(temp, buttton))
                    {
                        dataGridView1.Rows.Clear();
                        //label2.Text = strlisttostr(buttton);
                        direction.Insert(0, direction[0]);
                        direction.RemoveAt(direction.Count - 1);
                        //label1.Text = listtostring(direction);

                        dataGridView1.Rows.Add(direction[0], direction[1], direction[2], direction[3], direction[4], direction[5], direction[6], direction[7], direction[8], direction[9], direction[10], direction[11], direction[12], direction[13], direction[14], direction[15], direction[16], direction[17], direction[18], direction[19]);
                        dataGridView1.Rows.Add(buttton[0], buttton[1], buttton[2], buttton[3], buttton[4], buttton[5], buttton[6], buttton[7], buttton[8], buttton[9], buttton[10], buttton[11], buttton[12], buttton[13], buttton[14], buttton[15], buttton[16], buttton[17], buttton[18], buttton[19]);

                    }
                }
                else if (pad.BstateChanged)
                {
                    dataGridView1.Rows.Clear();
                    //label2.Text = strlisttostr(buttton);
                    direction.Insert(0, direction[0]);
                    direction.RemoveAt(direction.Count - 1);
                    //label1.Text = listtostring(direction);

                    dataGridView1.Rows.Add(direction[0], direction[1], direction[2], direction[3], direction[4], direction[5], direction[6], direction[7], direction[8], direction[9], direction[10], direction[11], direction[12], direction[13], direction[14], direction[15], direction[16], direction[17], direction[18], direction[19]);
                    dataGridView1.Rows.Add(buttton[0], buttton[1], buttton[2], buttton[3], buttton[4], buttton[5], buttton[6], buttton[7], buttton[8], buttton[9], buttton[10], buttton[11], buttton[12], buttton[13], buttton[14], buttton[15], buttton[16], buttton[17], buttton[18], buttton[19]);

                }
            }

            GC.Collect();
            //if (pad.BindsPressed(stick)) label1.Text = string.Join(".", pad.buttons);
        }

        //public string listtostring(List<char> list)
        //{
        //    string test = "";
        //    for (int i = 0; i < list.Count; ++i)
        //    {
        //        test += "   "+list[i];
        //    }
        //    return test;
        //}

        //public string strlisttostr(List<string> list)
        //{
        //    string test = "";
        //    for (int i = 0; i < list.Count; ++i)
        //    {
        //        test += " " + list[i];
        //    }
        //    return test;
        //}

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
            //Console.WriteLine(sticks.Count);
            return sticks.ToArray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Joystick[] joystick = GetSticks();

            this.KeyPreview = true;
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {//hand over array of buttons for binds
                Form2 form = new Form2(this);
                form.ShowDialog();
            }else if(e.KeyCode == Keys.F4)
            {
                for (int i = 0; i < 20; ++i)
                {
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
            }else if (e.KeyCode == Keys.F5)
            {
                for (int i = 0; i < 20; ++i)
                {
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }else if (e.KeyCode == Keys.F6)
            {
                for (int i = 0; i < 20; ++i)
                {
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
            }


        }
    }
}
