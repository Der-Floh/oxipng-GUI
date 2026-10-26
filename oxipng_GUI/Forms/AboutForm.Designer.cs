namespace OxipngGui.Forms;

sealed partial class AboutForm
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
            this.CloseButton = new System.Windows.Forms.Button();
            this.ProductLabel = new System.Windows.Forms.Label();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.OxipngLinkLabel = new System.Windows.Forms.LinkLabel();
            this.LicensesLabel = new System.Windows.Forms.Label();
            this.LicensesTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // CloseButton
            //
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.CloseButton.Location = new System.Drawing.Point(473, 430);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "OK";
            this.CloseButton.UseVisualStyleBackColor = true;
            //
            // ProductLabel
            //
            this.ProductLabel.AutoSize = true;
            this.ProductLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductLabel.Location = new System.Drawing.Point(12, 12);
            this.ProductLabel.Name = "ProductLabel";
            this.ProductLabel.Size = new System.Drawing.Size(97, 20);
            this.ProductLabel.TabIndex = 1;
            this.ProductLabel.Text = "oxipng GUI";
            //
            // DescriptionLabel
            //
            this.DescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionLabel.Location = new System.Drawing.Point(13, 40);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(535, 30);
            this.DescriptionLabel.TabIndex = 2;
            this.DescriptionLabel.Text = "A simple Windows front end for oxipng, the lossless PNG optimizer. This is an independent project and isn\'t affiliated with the oxipng developers.";
            //
            // OxipngLinkLabel
            //
            this.OxipngLinkLabel.AutoSize = true;
            this.OxipngLinkLabel.Location = new System.Drawing.Point(13, 76);
            this.OxipngLinkLabel.Name = "OxipngLinkLabel";
            this.OxipngLinkLabel.Size = new System.Drawing.Size(95, 13);
            this.OxipngLinkLabel.TabIndex = 3;
            this.OxipngLinkLabel.TabStop = true;
            this.OxipngLinkLabel.Text = "Powered by oxipng";
            this.OxipngLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OxipngLinkLabel_LinkClicked);
            //
            // LicensesLabel
            //
            this.LicensesLabel.AutoSize = true;
            this.LicensesLabel.Location = new System.Drawing.Point(13, 104);
            this.LicensesLabel.Name = "LicensesLabel";
            this.LicensesLabel.Size = new System.Drawing.Size(49, 13);
            this.LicensesLabel.TabIndex = 4;
            this.LicensesLabel.Text = "Licenses";
            //
            // LicensesTextBox
            //
            this.LicensesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LicensesTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.LicensesTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LicensesTextBox.Location = new System.Drawing.Point(16, 120);
            this.LicensesTextBox.Multiline = true;
            this.LicensesTextBox.Name = "LicensesTextBox";
            this.LicensesTextBox.ReadOnly = true;
            this.LicensesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LicensesTextBox.Size = new System.Drawing.Size(532, 300);
            this.LicensesTextBox.TabIndex = 5;
            //
            // AboutForm
            //
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(560, 465);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ProductLabel);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.OxipngLinkLabel);
            this.Controls.Add(this.LicensesLabel);
            this.Controls.Add(this.LicensesTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About oxipng GUI";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button CloseButton;
    private System.Windows.Forms.Label ProductLabel;
    private System.Windows.Forms.Label DescriptionLabel;
    private System.Windows.Forms.LinkLabel OxipngLinkLabel;
    private System.Windows.Forms.Label LicensesLabel;
    private System.Windows.Forms.TextBox LicensesTextBox;
}
