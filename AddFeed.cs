﻿using System;
using System.Drawing;
using System.Windows.Forms;
using TrayReader.Properties;

namespace TrayReader
{
    public partial class AddFeed : Form
    {
        private string currentURLInput;
        private bool can_add;

        public AddFeed()
        {
            InitializeComponent();
        }

        private void AddFeed_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            if (!text.Contains("http"))
            {
                text = "http://" + text;
            }
            can_add = Helper.ValidateURL(text);
            if (can_add)
            {
                textBox1.BackColor = Color.Empty;
                can_add = true;
            }
            else
            {
                textBox1.BackColor = Color.Red;
                can_add = false;
            }
            currentURLInput = text;
        }

        private void FeedSubmitButton_Click(object sender, EventArgs e)
        {
            // Get value of field
            if (!can_add)
            {
                return;
            }
            try
            {
                var potential_url = this.currentURLInput;
                if (Helper.ValidateURL(potential_url))
                {
                    Settings.Default.SettingFeedList.Add(potential_url.TrimEnd('/'));
                    Settings.Default.Save();
                    TrayReader.ProcessIcon.ni.ContextMenuStrip = new ContextMenus().CreateFeedsMenu();
                    this.Close();
                }
            }
            catch (Exception exception)
            {
                Program.ExceptionHandler(exception);
            }
        }
    }
}