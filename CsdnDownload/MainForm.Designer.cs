namespace CsdnDownload
{
    partial class MainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_clearcookie = new System.Windows.Forms.Button();
            this.btn_csdnurl = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 836);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(10, 12);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(75, 23);
            this.btn_start.TabIndex = 2;
            this.btn_start.Text = "开始接单";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(102, 12);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 3;
            this.btn_stop.Text = "停止工作";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1286, 409);
            this.panel1.TabIndex = 5;
            // 
            // btn_clearcookie
            // 
            this.btn_clearcookie.Location = new System.Drawing.Point(202, 12);
            this.btn_clearcookie.Name = "btn_clearcookie";
            this.btn_clearcookie.Size = new System.Drawing.Size(115, 23);
            this.btn_clearcookie.TabIndex = 6;
            this.btn_clearcookie.Text = "清除cookie";
            this.btn_clearcookie.UseVisualStyleBackColor = true;
            this.btn_clearcookie.Click += new System.EventHandler(this.btn_clearcookie_Click);
            // 
            // btn_csdnurl
            // 
            this.btn_csdnurl.Location = new System.Drawing.Point(346, 12);
            this.btn_csdnurl.Name = "btn_csdnurl";
            this.btn_csdnurl.Size = new System.Drawing.Size(127, 23);
            this.btn_csdnurl.TabIndex = 7;
            this.btn_csdnurl.Text = "打开CSDN页面";
            this.btn_csdnurl.UseVisualStyleBackColor = true;
            this.btn_csdnurl.Click += new System.EventHandler(this.btn_csdnurl_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(515, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1310, 476);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_csdnurl);
            this.Controls.Add(this.btn_clearcookie);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "工作台";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_clearcookie;
        private System.Windows.Forms.Button btn_csdnurl;
        private System.Windows.Forms.Button button2;
    }
}

