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

namespace input_history2
{
    public partial class Form1 : Form
    {
        Controller controller = new Controller(UserIndex.One);
        //State state;
        //GamepadButtonFlags GamepadButtonFlags;
        Pad pad = new Pad();

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
            public bool DstateChanged = false;
            public bool BstateChanged = false;

            private bool mem = false;

            public void Checkstate(Controller controller)
            {
                State state = controller.GetState();
                GamepadButtonFlags gamepadButtonFlags = state.Gamepad.Buttons;
                stateChanged = false;
                DstateChanged = false;
                BstateChanged = false;

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            //state = controller.GetState();
            //GamepadButtonFlags = state.Gamepad.Buttons;

            //label1.Text = Convert.ToString(GamepadButtonFlags.HasFlag(GamepadButtonFlags.DPadUp));
            //label2.Text = Convert.ToString(GamepadButtonFlags.HasFlag(GamepadButtonFlags.DPadDown));

            pad.Checkstate(controller);

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
                    }else if (pad.four)//1+2+4 !3
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
                }else if (pad.three)//1+3 !2
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
                }else if (pad.four)//1+4 !2 !3
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
            }else if (pad.two)//2 !1
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
                }else if (pad.four)//2+4 !1 !3
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
            }else if (pad.three)//3 !2 !1
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
            }else if (pad.four)//4 !3 !2 !1
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

            if (pad.stateChanged)
            {
                if (pad.DstateChanged)
                {
                    label1.Text = listtostring(direction);
                    buttton.Insert(0, buttton[0]);
                    buttton.RemoveAt(buttton.Count - 1);
                    label2.Text = strlisttostr(buttton);
                }
                else if (pad.BstateChanged)
                {
                    label2.Text = strlisttostr(buttton);
                    direction.Insert(0, direction[0]);
                    direction.RemoveAt(direction.Count - 1);
                    label1.Text = listtostring(direction);
                }
            }
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
    }
}
