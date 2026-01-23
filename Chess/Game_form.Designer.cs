namespace Chess
{
    partial class Game_form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            top_bar = new MenuStrip();
            endGameToolStripMenuItem = new ToolStripMenuItem();
            draw_bar_op = new ToolStripMenuItem();
            surrender_bar_op = new ToolStripMenuItem();
            top_bar.SuspendLayout();
            SuspendLayout();
            // 
            // top_bar
            // 
            top_bar.Items.AddRange(new ToolStripItem[] { endGameToolStripMenuItem });
            top_bar.Location = new Point(0, 0);
            top_bar.Name = "top_bar";
            top_bar.Size = new Size(686, 24);
            top_bar.TabIndex = 0;
            // 
            // endGameToolStripMenuItem
            // 
            endGameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { draw_bar_op, surrender_bar_op });
            endGameToolStripMenuItem.Name = "endGameToolStripMenuItem";
            endGameToolStripMenuItem.Size = new Size(72, 20);
            endGameToolStripMenuItem.Text = "End game";
            // 
            // draw_bar_op
            // 
            draw_bar_op.Name = "draw_bar_op";
            draw_bar_op.Size = new Size(180, 22);
            draw_bar_op.Text = "Draw";
            draw_bar_op.Click += game_end_option_click;
            // 
            // surrender_bar_op
            // 
            surrender_bar_op.Name = "surrender_bar_op";
            surrender_bar_op.Size = new Size(180, 22);
            surrender_bar_op.Text = "Surrender";
            surrender_bar_op.Click += game_end_option_click;
            // 
            // Game_form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(686, 571);
            Controls.Add(top_bar);
            MainMenuStrip = top_bar;
            Margin = new Padding(3, 2, 3, 2);
            MaximumSize = new Size(702, 610);
            MinimumSize = new Size(702, 610);
            Name = "Game_form";
            Text = "Game_form";
            top_bar.ResumeLayout(false);
            top_bar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip top_bar;
        private ToolStripMenuItem endGameToolStripMenuItem;
        private ToolStripMenuItem draw_bar_op;
        private ToolStripMenuItem surrender_bar_op;
    }
}