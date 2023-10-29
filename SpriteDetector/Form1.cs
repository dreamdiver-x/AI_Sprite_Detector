using Google.Protobuf.WellKnownTypes;
using Microsoft.ML;
using Microsoft.ML.Data;
using SkiaSharp;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using TorchSharp;
using static SpriteDetector.MLModel1;
using static System.Net.Mime.MediaTypeNames;

namespace SpriteDetector
{
    public partial class SpriteDetectorMainFrm : Form
    {
        MLModel1.ModelOutput SpriteDetectorOne;
        string filePathToTest = string.Empty;
        MLModel1.ModelInput sampleData;
        Rectangle rect;
        int boxRealWidth = 1;
        int boxRealHeight = 1;
        int boxWidth = 1;
        int boxHeight = 1;
        float scaleX = 1;
        float scaleY = 1;
        List<Rectangle> rectangulars;
        List<string> scoreVal;
        List<Rectangle> rectangularsBackups;
        List<string> scoreValBackups;
        const int ScoreShowCharLength = 5;

        public SpriteDetectorMainFrm()
        {
            InitializeComponent();
            MLModel1.ModelOutput SpriteDetectorOne = new MLModel1.ModelOutput();
            rectangulars = new List<Rectangle>();
            scoreVal = new List<string>();
            rectangularsBackups = new List<Rectangle>();
            scoreValBackups = new List<string>();
        }

        /// <summary>
        /// start evaluation of loaded image, found results as boundary boxes stored in lists 
        /// and ploted in richtextbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            rtbResult.Clear();
            changeStartBtn(true);
            //clear rectangular and score text list boxes with possible old boundary box data before reloading a new picture
            rectangulars.Clear();
            scoreVal.Clear();
            pictureBox1.Refresh();

            if (pictureBox1.Image == null)
            {
                rtbResult.Text = "Error: No Image loaded for detection!";
                changeStartBtn(false);
            }
            else
            {
                backgroundWorkerDetection.RunWorkerAsync();
            }
        }


        private void btnLoadImg_Click(object sender, EventArgs e)
        {
            rectangulars.Clear();
            scoreVal.Clear();
            pictureBox1.Refresh();
            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                pictureBox1.Image = new Bitmap(open.FileName);
                // get height and width values to later calc scale between loaded img and pic box
                boxRealHeight = pictureBox1.Image.Height;
                boxRealWidth = pictureBox1.Image.Width;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                boxWidth = pictureBox1.Size.Width;
                boxHeight = pictureBox1.Size.Height;
                // image file path  
                filePathToTest = open.FileName;
                // plot image details in txt box
                string imagepath = filePathToTest.Substring(filePathToTest.LastIndexOf("\\"));
                imagepath = imagepath.Remove(0, 1);
                rtbImgDetails.Text = "Image name: " + imagepath + ", size: " + Convert.ToString(boxRealWidth) +
                    "px * " + Convert.ToString(boxRealHeight) + "px, Resolution: " +
                    Convert.ToString(pictureBox1.Image.HorizontalResolution) + "dpi, " +
                    Convert.ToString(pictureBox1.Image.VerticalResolution) + "dpi;";
            }

            // Create single instance of sample data from first line of dataset for model input.
            var image = MLImage.CreateFromFile(filePathToTest);
            sampleData = new MLModel1.ModelInput()
            {
                Image = image,
            };
        }

        /// <summary>
        /// method to change beahviour of start-evaluation-button
        /// </summary>
        /// <param name="state"></param>
        private void changeStartBtn(bool state)
        {
            if (state == false)
            {
                btnStart.BackColor = SystemColors.Control;
                btnStart.Text = "Start Detection";
            }
            if (state == true)
            {
                btnStart.BackColor = Color.OrangeRed;
                btnStart.Text = "Ongoing Detection";
            }
        }

        /// <summary>
        /// event method to plot rectangulars and text of boundary boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (rectangulars.Count > 0)
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    // plot bounding box with score for each detected element in list
                    foreach (var item in rectangulars)
                    {
                        e.Graphics.DrawRectangle(pen, item);
                        using (Font font1 = new Font("Arial", 8, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            // get index of corresponding rectangular for text plot of score value
                            int index = rectangulars.FindIndex(a => a.Contains(item));
                            e.Graphics.DrawRectangle(pen, item);
                            string score = scoreVal.ElementAt(index);
                            // limit printed score string length
                            if (score.Length > ScoreShowCharLength)
                            {
                                score = score.Substring(0, ScoreShowCharLength);
                            }
                            // plot text with score value of bounding box rectangular
                            e.Graphics.DrawString(score, font1, Brushes.OrangeRed, item);
                            // plot bounding box as rectangular
                            e.Graphics.DrawRectangle(pen, item);
                        }
                    }
                }
                changeStartBtn(false);
            }
            else
            {
                using (Pen pen = new Pen(Color.FromArgb(0, 0, 0, 0), 2))
                {
                    foreach (var item in rectangularsBackups)
                    {
                        e.Graphics.DrawRectangle(pen, item);
                    }
                }
            } 
        }

        private void backgroundWorkerDetection_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Make a single prediction on the sample data and print results.
            var predictionResult = MLModel1.Predict(sampleData);
            rtbMessage("Predicted Boxes:\n");
            if (predictionResult.PredictedBoundingBoxes == null)
            {
                rtbMessage("No Predicted Bounding Boxes\n");
                refreshStartBtn();
                return;
            }
            var boxes =
                predictionResult.PredictedBoundingBoxes.Chunk(4)
                    .Select(x => new { XTop = x[0], YTop = x[1], XBottom = x[2], YBottom = x[3] })
                    .Zip(predictionResult.Score, (a, b) => new { Box = a, Score = b });

            //clear rectangular and score text list boxes with possible old boundary box data
            rectangulars.Clear();
            scoreVal.Clear();
            rectangularsBackups.Clear();
            int count = 0;
            foreach (var item in boxes)
            {
                // plot boundary boxes if found
                count++;
                rtbMessage(Convert.ToString(count) + $": XTop: {item.Box.XTop},YTop: {item.Box.YTop},XBottom: {item.Box.XBottom}" +
                    $",YBottom: {item.Box.YBottom}, Score: {item.Score}\n");

                // calc start points and width of each boundary box
                float xi = Math.Min((int)item.Box.XTop, (int)item.Box.XBottom);
                float yi = Math.Min((int)item.Box.YTop, (int)item.Box.YBottom);
                float wX = Math.Abs((int)item.Box.XTop - (int)item.Box.XBottom);
                float wY = Math.Abs((int)item.Box.YTop - (int)item.Box.YBottom);

                // calc scale in respect of picture box size to real loaded image size
                scaleX = (float)((float)boxWidth / (float)boxRealWidth);
                scaleY = (float)((float)boxHeight / (float)boxRealHeight);
                int width = (int)((float)wX * scaleX);
                int height = (int)((float)wX * scaleY);
                int x = (int)(xi * scaleX);
                int y = (int)(yi * scaleY);

                // add boundary box values for rectangular plot and text plot for score to lists
                rect = new Rectangle(x, y, width, height);
                rectangulars.Add(rect);
                rectangularsBackups.Add(rect);
                scoreVal.Add(Convert.ToString(item.Score));
            }
            // force a refresh to fire paint event for taking list data to be plotted on top of image and picture box
            refreshPicBox();
        }

        /// <summary>
        /// invoke method for rtb to prevent cross thread exceptions in backgroudnworker with main thread
        /// </summary>
        /// <param name="message"></param>
        private void rtbMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    rtbResult.AppendText(message);
                });
            }
        }

        /// <summary>
        /// invoke method to avoid thread exceptions with picBox setting from backgroudnworker to main thread
        /// </summary>
        private void refreshPicBox()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    pictureBox1.Refresh();
                });
            }
        }

        /// <summary>
        /// invoke method to avoid thread exceptions with startBtn setting from backgroudnworker to main thread
        /// </summary>
        private void refreshStartBtn()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    changeStartBtn(false);
                });
            }
        }
    }
}