namespace ArcaeaParty
{
    partial class ArcaeaParty
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.WorldMapTree = new System.Windows.Forms.TreeView();
            this.CurrentMap = new System.Windows.Forms.Label();
            this.StepList = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DoParty = new System.Windows.Forms.Button();
            this.Stamina = new System.Windows.Forms.Label();
            this.Character = new System.Windows.Forms.Label();
            this.ConvertStamina = new System.Windows.Forms.Button();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.TotalStep = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // WorldMapTree
            // 
            this.WorldMapTree.Location = new System.Drawing.Point(12, 29);
            this.WorldMapTree.Name = "WorldMapTree";
            this.WorldMapTree.Size = new System.Drawing.Size(186, 383);
            this.WorldMapTree.TabIndex = 0;
            this.WorldMapTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnWorldMapTreeSelect);
            // 
            // CurrentMap
            // 
            this.CurrentMap.AutoSize = true;
            this.CurrentMap.BackColor = System.Drawing.SystemColors.Control;
            this.CurrentMap.Location = new System.Drawing.Point(12, 423);
            this.CurrentMap.Name = "CurrentMap";
            this.CurrentMap.Size = new System.Drawing.Size(0, 12);
            this.CurrentMap.TabIndex = 1;
            // 
            // StepList
            // 
            this.StepList.Location = new System.Drawing.Point(204, 29);
            this.StepList.Name = "StepList";
            this.StepList.Size = new System.Drawing.Size(280, 383);
            this.StepList.TabIndex = 2;
            this.StepList.UseCompatibleStateImageBehavior = false;
            this.StepList.View = System.Windows.Forms.View.List;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Maps";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(204, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Steps";
            // 
            // DoParty
            // 
            this.DoParty.Location = new System.Drawing.Point(491, 385);
            this.DoParty.Name = "DoParty";
            this.DoParty.Size = new System.Drawing.Size(75, 26);
            this.DoParty.TabIndex = 5;
            this.DoParty.Text = "Play";
            this.DoParty.UseVisualStyleBackColor = true;
            this.DoParty.Click += new System.EventHandler(this.DoParty_Click);
            // 
            // Stamina
            // 
            this.Stamina.AutoSize = true;
            this.Stamina.Location = new System.Drawing.Point(490, 29);
            this.Stamina.Name = "Stamina";
            this.Stamina.Size = new System.Drawing.Size(47, 12);
            this.Stamina.TabIndex = 6;
            this.Stamina.Text = "Stamina";
            // 
            // Character
            // 
            this.Character.AutoSize = true;
            this.Character.Location = new System.Drawing.Point(489, 51);
            this.Character.Name = "Character";
            this.Character.Size = new System.Drawing.Size(59, 12);
            this.Character.TabIndex = 7;
            this.Character.Text = "Character";
            // 
            // ConvertStamina
            // 
            this.ConvertStamina.Enabled = false;
            this.ConvertStamina.Location = new System.Drawing.Point(491, 353);
            this.ConvertStamina.Name = "ConvertStamina";
            this.ConvertStamina.Size = new System.Drawing.Size(75, 26);
            this.ConvertStamina.TabIndex = 8;
            this.ConvertStamina.Text = "Convert";
            this.ConvertStamina.UseVisualStyleBackColor = true;
            this.ConvertStamina.Click += new System.EventHandler(this.ConvertStamina_Click);
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Location = new System.Drawing.Point(491, 321);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(75, 26);
            this.RefreshBtn.TabIndex = 9;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // TotalStep
            // 
            this.TotalStep.AutoSize = true;
            this.TotalStep.Location = new System.Drawing.Point(490, 73);
            this.TotalStep.Name = "TotalStep";
            this.TotalStep.Size = new System.Drawing.Size(59, 12);
            this.TotalStep.TabIndex = 10;
            this.TotalStep.Text = "TotalStep";
            // 
            // ArcaeaParty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 447);
            this.Controls.Add(this.TotalStep);
            this.Controls.Add(this.RefreshBtn);
            this.Controls.Add(this.ConvertStamina);
            this.Controls.Add(this.Character);
            this.Controls.Add(this.Stamina);
            this.Controls.Add(this.DoParty);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StepList);
            this.Controls.Add(this.CurrentMap);
            this.Controls.Add(this.WorldMapTree);
            this.MaximumSize = new System.Drawing.Size(594, 486);
            this.MinimumSize = new System.Drawing.Size(594, 486);
            this.Name = "ArcaeaParty";
            this.Text = "Arcaea Party";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView WorldMapTree;
        private System.Windows.Forms.Label CurrentMap;
        private System.Windows.Forms.ListView StepList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DoParty;
        private System.Windows.Forms.Label Stamina;
        private System.Windows.Forms.Label Character;
        private System.Windows.Forms.Button ConvertStamina;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.Label TotalStep;
    }
}

