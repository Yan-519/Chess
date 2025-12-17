namespace Chess
{
    partial class Change_pawn_window
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
            queen_button = new Button();
            rook_button = new Button();
            knight_button = new Button();
            bishop_button = new Button();
            SuspendLayout();
            // 
            // queen_button
            // 
            queen_button.Location = new Point(12, 12);
            queen_button.Name = "queen_button";
            queen_button.Size = new Size(117, 144);
            queen_button.TabIndex = 0;
            queen_button.UseVisualStyleBackColor = true;
            queen_button.Click += change_pawn_click;
            // 
            // rook_button
            // 
            rook_button.Location = new Point(135, 12);
            rook_button.Name = "rook_button";
            rook_button.Size = new Size(117, 144);
            rook_button.TabIndex = 1;
            rook_button.UseVisualStyleBackColor = true;
            rook_button.Click += change_pawn_click;
            // 
            // knight_button
            // 
            knight_button.Location = new Point(258, 12);
            knight_button.Name = "knight_button";
            knight_button.Size = new Size(117, 144);
            knight_button.TabIndex = 2;
            knight_button.UseVisualStyleBackColor = true;
            knight_button.Click += change_pawn_click;
            // 
            // bishop_button
            // 
            bishop_button.Location = new Point(381, 12);
            bishop_button.Name = "bishop_button";
            bishop_button.Size = new Size(117, 144);
            bishop_button.TabIndex = 3;
            bishop_button.UseVisualStyleBackColor = true;
            bishop_button.Click += change_pawn_click;
            // 
            // change_pawn_window
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(508, 168);
            Controls.Add(bishop_button);
            Controls.Add(knight_button);
            Controls.Add(rook_button);
            Controls.Add(queen_button);
            Name = "change_pawn_window";
            Text = "change_pawn_window";
            ResumeLayout(false);
        }

        #endregion

        private Button queen_button;
        private Button rook_button;
        private Button knight_button;
        private Button bishop_button;
    }
}