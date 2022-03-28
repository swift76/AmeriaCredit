namespace BankProcessingSimulator
{
    partial class MainForm
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
            this.buttonApprove = new System.Windows.Forms.Button();
            this.buttonRefuse = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelClientCode = new System.Windows.Forms.Label();
            this.textClientCode = new System.Windows.Forms.TextBox();
            this.checkHasCards = new System.Windows.Forms.CheckBox();
            this.buttonManual = new System.Windows.Forms.Button();
            this.checkIsDataComplete = new System.Windows.Forms.CheckBox();
            this.checkHasMultipleOptions = new System.Windows.Forms.CheckBox();
            this.buttonGive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonApprove
            // 
            this.buttonApprove.Location = new System.Drawing.Point(61, 218);
            this.buttonApprove.Name = "buttonApprove";
            this.buttonApprove.Size = new System.Drawing.Size(92, 32);
            this.buttonApprove.TabIndex = 0;
            this.buttonApprove.Text = "Approve";
            this.buttonApprove.UseVisualStyleBackColor = true;
            this.buttonApprove.Click += new System.EventHandler(this.buttonApprove_Click);
            // 
            // buttonRefuse
            // 
            this.buttonRefuse.Location = new System.Drawing.Point(203, 218);
            this.buttonRefuse.Name = "buttonRefuse";
            this.buttonRefuse.Size = new System.Drawing.Size(92, 32);
            this.buttonRefuse.TabIndex = 1;
            this.buttonRefuse.Text = "Refuse";
            this.buttonRefuse.UseVisualStyleBackColor = true;
            this.buttonRefuse.Click += new System.EventHandler(this.buttonRefuse_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(470, 218);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(92, 32);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelClientCode
            // 
            this.labelClientCode.AutoSize = true;
            this.labelClientCode.Location = new System.Drawing.Point(58, 61);
            this.labelClientCode.Name = "labelClientCode";
            this.labelClientCode.Size = new System.Drawing.Size(78, 17);
            this.labelClientCode.TabIndex = 3;
            this.labelClientCode.Text = "Client code";
            // 
            // textClientCode
            // 
            this.textClientCode.Location = new System.Drawing.Point(142, 61);
            this.textClientCode.Name = "textClientCode";
            this.textClientCode.Size = new System.Drawing.Size(102, 22);
            this.textClientCode.TabIndex = 4;
            // 
            // checkHasCards
            // 
            this.checkHasCards.AutoSize = true;
            this.checkHasCards.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkHasCards.Location = new System.Drawing.Point(298, 63);
            this.checkHasCards.Name = "checkHasCards";
            this.checkHasCards.Size = new System.Drawing.Size(94, 21);
            this.checkHasCards.TabIndex = 5;
            this.checkHasCards.Text = "Has cards";
            this.checkHasCards.UseVisualStyleBackColor = true;
            // 
            // buttonManual
            // 
            this.buttonManual.Location = new System.Drawing.Point(337, 218);
            this.buttonManual.Name = "buttonManual";
            this.buttonManual.Size = new System.Drawing.Size(92, 32);
            this.buttonManual.TabIndex = 6;
            this.buttonManual.Text = "Manual";
            this.buttonManual.UseVisualStyleBackColor = true;
            this.buttonManual.Click += new System.EventHandler(this.buttonManual_Click);
            // 
            // checkIsDataComplete
            // 
            this.checkIsDataComplete.AutoSize = true;
            this.checkIsDataComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkIsDataComplete.Location = new System.Drawing.Point(429, 63);
            this.checkIsDataComplete.Name = "checkIsDataComplete";
            this.checkIsDataComplete.Size = new System.Drawing.Size(133, 21);
            this.checkIsDataComplete.TabIndex = 7;
            this.checkIsDataComplete.Text = "Is data complete";
            this.checkIsDataComplete.UseVisualStyleBackColor = true;
            // 
            // checkHasMultipleOptions
            // 
            this.checkHasMultipleOptions.AutoSize = true;
            this.checkHasMultipleOptions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkHasMultipleOptions.Location = new System.Drawing.Point(252, 136);
            this.checkHasMultipleOptions.Name = "checkHasMultipleOptions";
            this.checkHasMultipleOptions.Size = new System.Drawing.Size(157, 21);
            this.checkHasMultipleOptions.TabIndex = 8;
            this.checkHasMultipleOptions.Text = "Has multiple options";
            this.checkHasMultipleOptions.UseVisualStyleBackColor = true;
            // 
            // buttonGive
            // 
            this.buttonGive.Location = new System.Drawing.Point(267, 280);
            this.buttonGive.Name = "buttonGive";
            this.buttonGive.Size = new System.Drawing.Size(92, 32);
            this.buttonGive.TabIndex = 9;
            this.buttonGive.Text = "Complete";
            this.buttonGive.UseVisualStyleBackColor = true;
            this.buttonGive.Click += new System.EventHandler(this.buttonGive_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 324);
            this.Controls.Add(this.buttonGive);
            this.Controls.Add(this.checkHasMultipleOptions);
            this.Controls.Add(this.checkIsDataComplete);
            this.Controls.Add(this.buttonManual);
            this.Controls.Add(this.checkHasCards);
            this.Controls.Add(this.textClientCode);
            this.Controls.Add(this.labelClientCode);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRefuse);
            this.Controls.Add(this.buttonApprove);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Process applications";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApprove;
        private System.Windows.Forms.Button buttonRefuse;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelClientCode;
        private System.Windows.Forms.TextBox textClientCode;
        private System.Windows.Forms.CheckBox checkHasCards;
        private System.Windows.Forms.Button buttonManual;
        private System.Windows.Forms.CheckBox checkIsDataComplete;
        private System.Windows.Forms.CheckBox checkHasMultipleOptions;
        private System.Windows.Forms.Button buttonGive;
    }
}

