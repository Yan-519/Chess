namespace Chess_project
{
    partial class Start_Page
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Bot_switch_box = new CheckBox();
            play_button = new Button();
            top_bar = new MenuStrip();
            top_bar_levels = new ToolStripMenuItem();
            easy_bar = new ToolStripMenuItem();
            normal_bar = new ToolStripMenuItem();
            hard_bar = new ToolStripMenuItem();
            top_bar_player_color = new ToolStripMenuItem();
            random_bar = new ToolStripMenuItem();
            white_bar = new ToolStripMenuItem();
            black_bar = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            top_bar.SuspendLayout();
            SuspendLayout();
            // 
            // Bot_switch_box
            // 
            Bot_switch_box.AutoSize = true;
            Bot_switch_box.Location = new Point(12, 26);
            Bot_switch_box.Margin = new Padding(3, 2, 3, 2);
            Bot_switch_box.Name = "Bot_switch_box";
            Bot_switch_box.Size = new Size(95, 19);
            Bot_switch_box.TabIndex = 0;
            Bot_switch_box.Text = "Play with bot";
            Bot_switch_box.UseVisualStyleBackColor = true;
            Bot_switch_box.CheckedChanged += bot_switch;
            // 
            // play_button
            // 
            play_button.AutoSize = true;
            play_button.Location = new Point(172, 26);
            play_button.Margin = new Padding(3, 2, 3, 2);
            play_button.Name = "play_button";
            play_button.Size = new Size(82, 25);
            play_button.TabIndex = 2;
            play_button.Text = "Play";
            play_button.UseVisualStyleBackColor = true;
            play_button.Click += start_button_Click;
            // 
            // top_bar
            // 
            top_bar.ImageScalingSize = new Size(20, 20);
            top_bar.Items.AddRange(new ToolStripItem[] { top_bar_levels, top_bar_player_color });
            top_bar.Location = new Point(0, 0);
            top_bar.Name = "top_bar";
            top_bar.Size = new Size(264, 24);
            top_bar.TabIndex = 3;
            top_bar.Text = "menuStrip1";
            // 
            // top_bar_levels
            // 
            top_bar_levels.DropDownItems.AddRange(new ToolStripItem[] { easy_bar, normal_bar, hard_bar });
            top_bar_levels.Name = "top_bar_levels";
            top_bar_levels.Size = new Size(98, 20);
            top_bar_levels.Text = "Select bot level";
            // 
            // easy_bar
            // 
            easy_bar.Checked = true;
            easy_bar.CheckState = CheckState.Checked;
            easy_bar.Name = "easy_bar";
            easy_bar.Size = new Size(180, 22);
            easy_bar.Text = "Easy";
            easy_bar.Click += bot_level_menu_Click;
            // 
            // normal_bar
            // 
            normal_bar.Name = "normal_bar";
            normal_bar.Size = new Size(180, 22);
            normal_bar.Text = "Normal";
            normal_bar.Click += bot_level_menu_Click;
            // 
            // hard_bar
            // 
            hard_bar.Name = "hard_bar";
            hard_bar.Size = new Size(180, 22);
            hard_bar.Text = "Hard";
            hard_bar.Click += bot_level_menu_Click;
            // 
            // top_bar_player_color
            // 
            top_bar_player_color.DropDownItems.AddRange(new ToolStripItem[] { random_bar, white_bar, black_bar });
            top_bar_player_color.Name = "top_bar_player_color";
            top_bar_player_color.Size = new Size(115, 20);
            top_bar_player_color.Text = "Select player color";
            // 
            // random_bar
            // 
            random_bar.Checked = true;
            random_bar.CheckState = CheckState.Checked;
            random_bar.Name = "random_bar";
            random_bar.Size = new Size(180, 22);
            random_bar.Text = "Random";
            random_bar.Click += color_menu_Click;
            // 
            // white_bar
            // 
            white_bar.Name = "white_bar";
            white_bar.Size = new Size(180, 22);
            white_bar.Text = "White";
            white_bar.Click += color_menu_Click;
            // 
            // black_bar
            // 
            black_bar.Name = "black_bar";
            black_bar.Size = new Size(180, 22);
            black_bar.Text = "Black";
            black_bar.Click += color_menu_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(67, 22);
            // 
            // Start_Page
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(264, 66);
            Controls.Add(play_button);
            Controls.Add(Bot_switch_box);
            Controls.Add(top_bar);
            MainMenuStrip = top_bar;
            Margin = new Padding(3, 2, 3, 2);
            Name = "Start_Page";
            Text = "Start page";
            top_bar.ResumeLayout(false);
            top_bar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox Bot_switch_box;
        private Button play_button;
        private MenuStrip top_bar;
        private ToolStripMenuItem top_bar_levels;
        private ToolStripMenuItem normal_bar;
        private ToolStripMenuItem hard_bar;
        private ToolStripMenuItem easy_bar;
        //private ToolStripMenuItem top_bar_start_color;
        private ToolStripMenuItem top_bar_player_color;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem random_bar;
        private ToolStripMenuItem white_bar;
        private ToolStripMenuItem black_bar;
    }
}
