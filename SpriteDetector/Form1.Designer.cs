namespace SpriteDetector
{
    partial class SpriteDetectorMainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpriteDetectorMainFrm));
            btnStart = new Button();
            pictureBox1 = new PictureBox();
            btnLoadImg = new Button();
            rtbResult = new RichTextBox();
            backgroundWorkerDetection = new System.ComponentModel.BackgroundWorker();
            rtbImgDetails = new RichTextBox();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 732);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(106, 89);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start Detection";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 100);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(982, 626);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            // 
            // btnLoadImg
            // 
            btnLoadImg.Location = new Point(12, 23);
            btnLoadImg.Name = "btnLoadImg";
            btnLoadImg.Size = new Size(106, 71);
            btnLoadImg.TabIndex = 2;
            btnLoadImg.Text = "Load Image";
            btnLoadImg.UseVisualStyleBackColor = true;
            btnLoadImg.Click += btnLoadImg_Click;
            // 
            // rtbResult
            // 
            rtbResult.Location = new Point(124, 732);
            rtbResult.Name = "rtbResult";
            rtbResult.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbResult.Size = new Size(870, 89);
            rtbResult.TabIndex = 3;
            rtbResult.Text = "";
            // 
            // backgroundWorkerDetection
            // 
            backgroundWorkerDetection.DoWork += backgroundWorkerDetection_DoWork;
            // 
            // rtbImgDetails
            // 
            rtbImgDetails.BackColor = SystemColors.Control;
            rtbImgDetails.BorderStyle = BorderStyle.None;
            rtbImgDetails.Location = new Point(-1, 0);
            rtbImgDetails.Name = "rtbImgDetails";
            rtbImgDetails.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbImgDetails.Size = new Size(870, 70);
            rtbImgDetails.TabIndex = 4;
            rtbImgDetails.Text = "Image details...";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(rtbImgDetails);
            panel1.Location = new Point(124, 23);
            panel1.Name = "panel1";
            panel1.Size = new Size(870, 71);
            panel1.TabIndex = 5;
            // 
            // SpriteDetectorMainFrm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1006, 833);
            Controls.Add(panel1);
            Controls.Add(rtbResult);
            Controls.Add(btnLoadImg);
            Controls.Add(pictureBox1);
            Controls.Add(btnStart);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SpriteDetectorMainFrm";
            Text = "Sprite Detector";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnStart;
        private PictureBox pictureBox1;
        private Button btnLoadImg;
        private RichTextBox rtbResult;
        private System.ComponentModel.BackgroundWorker backgroundWorkerDetection;
        private RichTextBox rtbImgDetails;
        private Panel panel1;
    }
}