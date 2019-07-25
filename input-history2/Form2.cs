﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//using SlimDX.XInput;
//using SlimDX.DirectInput;

namespace input_history2
{
    public partial class Form2 : Form
    {
        Form1 parent;
        public int[] ids = new int[7];

        public Form2(Form1 form)
        {
            InitializeComponent();
            parent = form;
            for(int i = 0; i < 7; ++i)
            {
                ids[i] = -1;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Waiting";
            Form3 form = new Form3("1+2",this);
            form.ShowDialog();
            if (ids[0] == -1) button1.Text = "";
            else button1.Text = Convert.ToString(ids[0]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Text = "Waiting";
            Form3 form = new Form3("1+3", this);
            form.ShowDialog();
            if (ids[1] == -1) button2.Text = "";
            else button2.Text = Convert.ToString(ids[1]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "Waiting";
            Form3 form = new Form3("1+4", this);
            form.ShowDialog();
            if (ids[2] == -1) button3.Text = "";
            else button3.Text = Convert.ToString(ids[2]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Text = "Waiting";
            Form3 form = new Form3("2+3", this);
            form.ShowDialog();
            if (ids[3] == -1) button4.Text = "";
            else button4.Text = Convert.ToString(ids[3]);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Text = "Waiting";
            Form3 form = new Form3("2+4", this);
            form.ShowDialog();
            if (ids[4] == -1) button5.Text = "";
            else button5.Text = Convert.ToString(ids[4]);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Text = "Waiting";
            Form3 form = new Form3("3+4", this);
            form.ShowDialog();
            if (ids[5] == -1) button6.Text = "";
            else button6.Text = Convert.ToString(ids[5]);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Text = "Waiting";
            Form3 form = new Form3("1+2+3+4", this);
            form.ShowDialog();
            if (ids[6] == -1) button7.Text = "";
            else button7.Text = Convert.ToString(ids[6]);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int temp = -1;
            bool swit = true;

            for (int i = 0; i < 7; ++i)
            {
                temp = ids[i];
                ids[i] = -1;
                if (ids.Contains(temp)&&temp!=-1)
                {
                    MessageBox.Show("There are 2 or more binds for 1 button, can not proceed");
                    swit = false;
                    break;
                }
                ids[i] = temp;
            }
            if (swit&&CheckForm())
            {
                for (int i = 0; i < 7; ++i)
                {
                    parent.pad.ids[i] = ids[i];
                }

                this.Close();
            }
        }

        static private bool CheckForm()
        {
            DialogResult dialogResult = MessageBox.Show("Closing window", "Are you sure?", MessageBoxButtons.YesNo);
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

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            GC.Collect();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 7; ++i)
            {
                ids[i] = parent.pad.ids[i];
            }
            if (ids[6] == -1) button7.Text = "";
            else button7.Text = Convert.ToString(ids[6]);
            if (ids[5] == -1) button6.Text = "";
            else button6.Text = Convert.ToString(ids[5]);
            if (ids[4] == -1) button5.Text = "";
            else button5.Text = Convert.ToString(ids[4]);
            if (ids[3] == -1) button4.Text = "";
            else button4.Text = Convert.ToString(ids[3]);
            if (ids[2] == -1) button3.Text = "";
            else button3.Text = Convert.ToString(ids[2]);
            if (ids[1] == -1) button2.Text = "";
            else button2.Text = Convert.ToString(ids[1]);
            if (ids[0] == -1) button1.Text = "";
            else button1.Text = Convert.ToString(ids[0]);
        }
    }
}