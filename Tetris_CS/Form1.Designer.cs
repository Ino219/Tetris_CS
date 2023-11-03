namespace Tetris_CS
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Field = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.NextBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Field)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // Field
            // 
            this.Field.BackColor = System.Drawing.Color.Tan;
            this.Field.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Field.Location = new System.Drawing.Point(33, 22);
            this.Field.Name = "Field";
            this.Field.Size = new System.Drawing.Size(195, 348);
            this.Field.TabIndex = 0;
            this.Field.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // NextBox
            // 
            this.NextBox.BackColor = System.Drawing.Color.Moccasin;
            this.NextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NextBox.Location = new System.Drawing.Point(318, 42);
            this.NextBox.Name = "NextBox";
            this.NextBox.Size = new System.Drawing.Size(80, 62);
            this.NextBox.TabIndex = 2;
            this.NextBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 450);
            this.Controls.Add(this.NextBox);
            this.Controls.Add(this.Field);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Field)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Field;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox NextBox;
    }
}

