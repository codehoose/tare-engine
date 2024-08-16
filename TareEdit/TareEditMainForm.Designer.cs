namespace TareEdit
{
    partial class TareEditMainForm
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            RightPanelRoom = new Panel();
            DirectionsPanel = new Panel();
            DownButton = new Button();
            UpButton = new Button();
            WestButton = new Button();
            EastButton = new Button();
            SouthButton = new Button();
            NorthButton = new Button();
            RoomListPanel = new Panel();
            panel2 = new Panel();
            button1 = new Button();
            button2 = new Button();
            lstRooms = new ListBox();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            RightPanelRoom.SuspendLayout();
            DirectionsPanel.SuspendLayout();
            RoomListPanel.SuspendLayout();
            panel2.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 24);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1054, 629);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(RightPanelRoom);
            tabPage1.Controls.Add(RoomListPanel);
            tabPage1.Location = new Point(4, 30);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1046, 595);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Rooms";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // RightPanelRoom
            // 
            RightPanelRoom.Controls.Add(DirectionsPanel);
            RightPanelRoom.Dock = DockStyle.Fill;
            RightPanelRoom.Location = new Point(280, 3);
            RightPanelRoom.Name = "RightPanelRoom";
            RightPanelRoom.Size = new Size(763, 589);
            RightPanelRoom.TabIndex = 3;
            // 
            // DirectionsPanel
            // 
            DirectionsPanel.Controls.Add(DownButton);
            DirectionsPanel.Controls.Add(UpButton);
            DirectionsPanel.Controls.Add(WestButton);
            DirectionsPanel.Controls.Add(EastButton);
            DirectionsPanel.Controls.Add(SouthButton);
            DirectionsPanel.Controls.Add(NorthButton);
            DirectionsPanel.Dock = DockStyle.Bottom;
            DirectionsPanel.Location = new Point(0, 412);
            DirectionsPanel.Name = "DirectionsPanel";
            DirectionsPanel.Size = new Size(763, 177);
            DirectionsPanel.TabIndex = 2;
            // 
            // DownButton
            // 
            DownButton.Dock = DockStyle.Bottom;
            DownButton.Location = new Point(245, 91);
            DownButton.Name = "DownButton";
            DownButton.Size = new Size(272, 43);
            DownButton.TabIndex = 5;
            DownButton.Text = "Down";
            DownButton.UseVisualStyleBackColor = true;
            // 
            // UpButton
            // 
            UpButton.Dock = DockStyle.Top;
            UpButton.Location = new Point(245, 43);
            UpButton.Name = "UpButton";
            UpButton.Size = new Size(272, 43);
            UpButton.TabIndex = 4;
            UpButton.Text = "Up";
            UpButton.UseVisualStyleBackColor = true;
            // 
            // WestButton
            // 
            WestButton.Dock = DockStyle.Left;
            WestButton.Location = new Point(0, 43);
            WestButton.Name = "WestButton";
            WestButton.Size = new Size(245, 91);
            WestButton.TabIndex = 3;
            WestButton.Text = "West";
            WestButton.UseVisualStyleBackColor = true;
            // 
            // EastButton
            // 
            EastButton.Dock = DockStyle.Right;
            EastButton.Location = new Point(517, 43);
            EastButton.Name = "EastButton";
            EastButton.Size = new Size(246, 91);
            EastButton.TabIndex = 2;
            EastButton.Text = "East";
            EastButton.UseVisualStyleBackColor = true;
            // 
            // SouthButton
            // 
            SouthButton.Dock = DockStyle.Bottom;
            SouthButton.Location = new Point(0, 134);
            SouthButton.Name = "SouthButton";
            SouthButton.Size = new Size(763, 43);
            SouthButton.TabIndex = 1;
            SouthButton.Text = "South";
            SouthButton.UseVisualStyleBackColor = true;
            // 
            // NorthButton
            // 
            NorthButton.Dock = DockStyle.Top;
            NorthButton.Location = new Point(0, 0);
            NorthButton.Name = "NorthButton";
            NorthButton.Size = new Size(763, 43);
            NorthButton.TabIndex = 0;
            NorthButton.Text = "North";
            NorthButton.UseVisualStyleBackColor = true;
            // 
            // RoomListPanel
            // 
            RoomListPanel.Controls.Add(panel2);
            RoomListPanel.Controls.Add(lstRooms);
            RoomListPanel.Dock = DockStyle.Left;
            RoomListPanel.Location = new Point(3, 3);
            RoomListPanel.Name = "RoomListPanel";
            RoomListPanel.Size = new Size(277, 589);
            RoomListPanel.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(button1);
            panel2.Controls.Add(button2);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 535);
            panel2.Name = "panel2";
            panel2.Size = new Size(277, 54);
            panel2.TabIndex = 3;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Right;
            button1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(127, 0);
            button1.Name = "button1";
            button1.Size = new Size(75, 54);
            button1.TabIndex = 1;
            button1.Text = "-";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Right;
            button2.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(202, 0);
            button2.Name = "button2";
            button2.Size = new Size(75, 54);
            button2.TabIndex = 2;
            button2.Text = "+";
            button2.UseVisualStyleBackColor = true;
            // 
            // lstRooms
            // 
            lstRooms.Dock = DockStyle.Fill;
            lstRooms.FormattingEnabled = true;
            lstRooms.ItemHeight = 21;
            lstRooms.Location = new Point(0, 0);
            lstRooms.Name = "lstRooms";
            lstRooms.Size = new Size(277, 589);
            lstRooms.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1046, 601);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Items";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1046, 601);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Flags";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1054, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(155, 22);
            openToolStripMenuItem.Text = "&Open...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // TareEditMainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1054, 653);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(4);
            Name = "TareEditMainForm";
            Text = "TareEdit";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            RightPanelRoom.ResumeLayout(false);
            DirectionsPanel.ResumeLayout(false);
            RoomListPanel.ResumeLayout(false);
            panel2.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ListBox lstRooms;
        private Panel RoomListPanel;
        private Button button2;
        private Button button1;
        private Panel panel2;
        private Panel DirectionsPanel;
        private Button UpButton;
        private Button WestButton;
        private Button EastButton;
        private Button SouthButton;
        private Button NorthButton;
        private Button DownButton;
        private Panel RightPanelRoom;
    }
}
