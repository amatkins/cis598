namespace PolyE
{
    partial class AppWindow
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
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabLayoutPanel = new System.Windows.Forms.TabControl();
            this.branchesPage = new System.Windows.Forms.TabPage();
            this.leavesPage = new System.Windows.Forms.TabPage();
            this.agentsPage = new System.Windows.Forms.TabPage();
            this.branchesText = new System.Windows.Forms.TextBox();
            this.leavesText = new System.Windows.Forms.TextBox();
            this.agentsText = new System.Windows.Forms.TextBox();
            this.runButton = new System.Windows.Forms.Button();
            this.mainLayoutPanel.SuspendLayout();
            this.tabLayoutPanel.SuspendLayout();
            this.branchesPage.SuspendLayout();
            this.leavesPage.SuspendLayout();
            this.agentsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.tabLayoutPanel, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.runButton, 0, 1);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(1468, 812);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // tabLayoutPanel
            // 
            this.tabLayoutPanel.Controls.Add(this.branchesPage);
            this.tabLayoutPanel.Controls.Add(this.leavesPage);
            this.tabLayoutPanel.Controls.Add(this.agentsPage);
            this.tabLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.tabLayoutPanel.Name = "tabLayoutPanel";
            this.tabLayoutPanel.SelectedIndex = 0;
            this.tabLayoutPanel.Size = new System.Drawing.Size(1462, 724);
            this.tabLayoutPanel.TabIndex = 0;
            // 
            // branchesPage
            // 
            this.branchesPage.Controls.Add(this.branchesText);
            this.branchesPage.Location = new System.Drawing.Point(10, 48);
            this.branchesPage.Name = "branchesPage";
            this.branchesPage.Padding = new System.Windows.Forms.Padding(3);
            this.branchesPage.Size = new System.Drawing.Size(1442, 666);
            this.branchesPage.TabIndex = 0;
            this.branchesPage.Text = "Branches";
            this.branchesPage.UseVisualStyleBackColor = true;
            // 
            // leavesPage
            // 
            this.leavesPage.Controls.Add(this.leavesText);
            this.leavesPage.Location = new System.Drawing.Point(10, 48);
            this.leavesPage.Name = "leavesPage";
            this.leavesPage.Padding = new System.Windows.Forms.Padding(3);
            this.leavesPage.Size = new System.Drawing.Size(1442, 666);
            this.leavesPage.TabIndex = 1;
            this.leavesPage.Text = "Leaves";
            this.leavesPage.UseVisualStyleBackColor = true;
            // 
            // agentsPage
            // 
            this.agentsPage.Controls.Add(this.agentsText);
            this.agentsPage.Location = new System.Drawing.Point(10, 48);
            this.agentsPage.Name = "agentsPage";
            this.agentsPage.Padding = new System.Windows.Forms.Padding(3);
            this.agentsPage.Size = new System.Drawing.Size(1442, 666);
            this.agentsPage.TabIndex = 2;
            this.agentsPage.Text = "Agents";
            this.agentsPage.UseVisualStyleBackColor = true;
            // 
            // branchesText
            // 
            this.branchesText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.branchesText.Enabled = false;
            this.branchesText.Location = new System.Drawing.Point(3, 3);
            this.branchesText.Multiline = true;
            this.branchesText.Name = "branchesText";
            this.branchesText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.branchesText.Size = new System.Drawing.Size(1436, 660);
            this.branchesText.TabIndex = 0;
            this.branchesText.WordWrap = false;
            // 
            // leavesText
            // 
            this.leavesText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leavesText.Enabled = false;
            this.leavesText.Location = new System.Drawing.Point(3, 3);
            this.leavesText.Multiline = true;
            this.leavesText.Name = "leavesText";
            this.leavesText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.leavesText.Size = new System.Drawing.Size(1436, 660);
            this.leavesText.TabIndex = 1;
            this.leavesText.WordWrap = false;
            // 
            // agentsText
            // 
            this.agentsText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.agentsText.Enabled = false;
            this.agentsText.Location = new System.Drawing.Point(3, 3);
            this.agentsText.Multiline = true;
            this.agentsText.Name = "agentsText";
            this.agentsText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.agentsText.Size = new System.Drawing.Size(1436, 660);
            this.agentsText.TabIndex = 1;
            this.agentsText.WordWrap = false;
            // 
            // runButton
            // 
            this.runButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runButton.Location = new System.Drawing.Point(3, 733);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(1462, 76);
            this.runButton.TabIndex = 1;
            this.runButton.Text = "Tick";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // AppWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(240F, 240F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1468, 812);
            this.Controls.Add(this.mainLayoutPanel);
            this.Name = "AppWindow";
            this.Text = "PolyE";
            this.mainLayoutPanel.ResumeLayout(false);
            this.tabLayoutPanel.ResumeLayout(false);
            this.branchesPage.ResumeLayout(false);
            this.branchesPage.PerformLayout();
            this.leavesPage.ResumeLayout(false);
            this.leavesPage.PerformLayout();
            this.agentsPage.ResumeLayout(false);
            this.agentsPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.TabControl tabLayoutPanel;
        private System.Windows.Forms.TabPage branchesPage;
        private System.Windows.Forms.TextBox branchesText;
        private System.Windows.Forms.TabPage leavesPage;
        private System.Windows.Forms.TextBox leavesText;
        private System.Windows.Forms.TabPage agentsPage;
        private System.Windows.Forms.TextBox agentsText;
        private System.Windows.Forms.Button runButton;
    }
}

